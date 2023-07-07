using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using EtteplanMORE.ServiceManual.ApplicationCore.Entities;
using EtteplanMORE.ServiceManual.ApplicationCore.Enums;
using EtteplanMORE.ServiceManual.ApplicationCore.Exceptions;
using EtteplanMORE.ServiceManual.ApplicationCore.Interfaces;
using EtteplanMORE.ServiceManual.ApplicationCore.Data;
using EtteplanMORE.ServiceManual.ApplicationCore.Dtos;
using Microsoft.EntityFrameworkCore;

namespace EtteplanMORE.ServiceManual.ApplicationCore.Services
{
    public class FactoryDeviceService : IFactoryDeviceService
    {
        private readonly ApplicationDbContext _dbContext;

        public FactoryDeviceService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<FactoryDevice> ReadDataFromCsv()
        {
            List<FactoryDevice> devices = new List<FactoryDevice>();

            using (var reader = new StreamReader("../seeddata.csv"))
            {

                // Skip header row (Name,Year,Type)
                var headerLine = reader.ReadLine();

                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();

                    if (!string.IsNullOrWhiteSpace(line))
                    {
                        var values = line.Split(',');

                        var name = values[0];
                        var year = int.Parse(values[1]);
                        var type = values[2];

                        var device = new FactoryDevice
                        {
                            Name = name,
                            Year = year,
                            Type = type
                        };

                        devices.Add(device);
                    }
                }
            }

            return devices;
        }

        // Seed database, devices
        public async Task SeedData(ApplicationDbContext dbContext)
        {
            try
            {
                if (!dbContext.FactoryDevices.Any())
                {
                    List<FactoryDevice> devices = ReadDataFromCsv();
                    await dbContext.FactoryDevices.AddRangeAsync(devices);
                    await dbContext.SaveChangesAsync();
                }
            }
            catch (DbUpdateException ex)
            {
                throw new DatabaseException(ex.InnerException?.Message ?? ex.Message);
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.InnerException?.Message ?? ex.Message);
            }
        }
        // Seed database, tasks
        public async Task SeedTaskData(ApplicationDbContext dbContext)
        {
            try
            {
                if (!dbContext.MaintenanceTasks.Any())
                {
                    // Just generating different dates for variety
                    DateTime dateNow = DateTime.Now;
                    DateTime dateOlder = dateNow.AddDays(-1);
                    DateTime dateWayOlder = dateNow.AddDays(-3);

                    var tasks = new List<MaintenanceTask>
                    {
                        new MaintenanceTask {
                            DeviceId = 3,
                            EntryDate = dateNow,
                            Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Quisque blandit facilisis velit, ut mattis purus eleifend a.",
                            CriticalityLevel = CriticalityLevel.Minor,
                            Done = true
                        },
                        new MaintenanceTask { DeviceId = 3, EntryDate = dateOlder, Description = "Proin aliquam ex sit amet libero fermentum ultricies.", CriticalityLevel = CriticalityLevel.Important, Done = false },
                        new MaintenanceTask { DeviceId = 4, EntryDate = dateWayOlder, Description = "Maecenas interdum efficitur mauris, sit amet suscipit tellus porta sed.", CriticalityLevel = CriticalityLevel.Critical, Done = false },
                        new MaintenanceTask { DeviceId = 4, EntryDate = dateOlder, Description = "Quisque in elit in sem ullamcorper luctus. Sed eu faucibus nulla, ac venenatis metus.", CriticalityLevel = CriticalityLevel.Important, Done = true },
                        new MaintenanceTask { DeviceId = 4, EntryDate = dateNow, Description = "Ut ornare, ipsum non imperdiet elementum, urna magna hendrerit nunc, in mattis mauris nibh varius augue.", CriticalityLevel = CriticalityLevel.Minor, Done = false },
                    };
                    await dbContext.MaintenanceTasks.AddRangeAsync(tasks);
                    await dbContext.SaveChangesAsync();
                }
            }
            catch (DbUpdateException ex)
            {
                throw new DatabaseException(ex.InnerException?.Message ?? ex.Message);
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.InnerException?.Message ?? ex.Message);
            }
        }

        public List<FactoryDeviceDto> GetAllFactoryDevices()
        {
            try
            {
                var devices = _dbContext.FactoryDevices.ToList();
                if (devices == null)
                {
                    throw new NotFoundException("Devices not found.");
                }

                return devices.Select(fd => new FactoryDeviceDto
                {
                    Id = fd.Id,
                    Name = fd.Name,
                    Year = fd.Year,
                    Type = fd.Type
                }).ToList();
            }
            catch (DbUpdateException ex)
            {
                throw new DatabaseException(ex.InnerException?.Message ?? ex.Message);
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.InnerException?.Message ?? ex.Message);
            }
        }

        public FactoryDeviceDto GetFactoryDevice(int deviceId)
        {
            try
            {
                var device = _dbContext.FactoryDevices.FirstOrDefault(fd => fd.Id == deviceId);
                if (device == null)
                {
                    throw new NotFoundException("Device not found.");
                }

                var deviceDto = new FactoryDeviceDto
                {
                    Id = device.Id,
                    Name = device.Name,
                    Year = device.Year,
                    Type = device.Type
                };

                return deviceDto;
            }
            catch (DbUpdateException ex)
            {
                throw new DatabaseException(ex.InnerException?.Message ?? ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException?.Message ?? ex.Message);
            }
        }

        public List<MaintenanceTaskDto> GetMaintenanceTasksForFactoryDevice(int deviceId)
        {
            try
            {
                var device = _dbContext.FactoryDevices.Include(fd => fd.MaintenanceTasks).FirstOrDefault(fd => fd.Id == deviceId);
                if (device == null)
                {
                    throw new NotFoundException("Device not found.");
                }

                var maintenanceTasks = device.MaintenanceTasks?.Select(mt => new MaintenanceTaskDto
                {
                    Id = mt.Id,
                    DeviceId = mt.DeviceId,
                    EntryDate = mt.EntryDate,
                    Description = mt.Description,
                    CriticalityLevel = mt.CriticalityLevel,
                    Done = mt.Done
                }).ToList();

                // Order tasks by criticality level and then by entry date starting from the newest entry
                var orderedMaintenanceTasks = maintenanceTasks?.OrderBy(mt => mt.CriticalityLevel).ThenByDescending(mt => mt.EntryDate).ToList();

                if (orderedMaintenanceTasks != null)
                {
                    return orderedMaintenanceTasks;
                }
                else
                {
                    throw new NotFoundException("Tasks not found for device.");
                }
            }
            catch (DbUpdateException ex)
            {
                throw new DatabaseException(ex.InnerException?.Message ?? ex.Message);
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.InnerException?.Message ?? ex.Message);
            }
        }
    }
}