using System.Collections.Generic;
using System.Threading.Tasks;
using EtteplanMORE.ServiceManual.ApplicationCore.Entities;
using EtteplanMORE.ServiceManual.ApplicationCore.Data;
using EtteplanMORE.ServiceManual.ApplicationCore.Dtos;

namespace EtteplanMORE.ServiceManual.ApplicationCore.Interfaces
{
    public interface IFactoryDeviceService
    {
        List<FactoryDevice> ReadDataFromCsv();


        // For reading data from CSV, for testing purposes
        public List<FactoryDevice> GetAllDevices()
        {
            List<FactoryDevice> devices = ReadDataFromCsv();
            return devices;
        }

        // For seeding CSV data to database
        Task SeedData(ApplicationDbContext dbContext);

        // For seeding example tasks to database
        Task SeedTaskData(ApplicationDbContext dbContext);

        // Get all devices
        List<FactoryDeviceDto> GetAllFactoryDevices();

        // Get one device
        FactoryDeviceDto GetFactoryDevice(int deviceId);

        // Get tasks for one device
        List<MaintenanceTaskDto> GetMaintenanceTasksForFactoryDevice(int deviceId);
    }
}