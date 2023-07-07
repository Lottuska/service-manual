using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EtteplanMORE.ServiceManual.ApplicationCore.Entities;
using EtteplanMORE.ServiceManual.ApplicationCore.Exceptions;
using EtteplanMORE.ServiceManual.ApplicationCore.Interfaces;
using EtteplanMORE.ServiceManual.ApplicationCore.Data;
using EtteplanMORE.ServiceManual.ApplicationCore.Dtos;
using Microsoft.EntityFrameworkCore;

namespace EtteplanMORE.ServiceManual.ApplicationCore.Services
{
    public class MaintenanceTaskService : IMaintenanceTaskService
    {
        private readonly ApplicationDbContext _dbContext;

        public MaintenanceTaskService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<MaintenanceTaskDto> GetAllMaintenanceTasks()
        {
            try
            {
                var tasks = _dbContext.MaintenanceTasks.ToList();
                if (tasks == null)
                {
                    throw new NotFoundException("Tasks not found.");
                }

                var maintenanceTasks = tasks.Select(mt => new MaintenanceTaskDto
                {
                    Id = mt.Id,
                    DeviceId = mt.DeviceId,
                    EntryDate = mt.EntryDate,
                    Description = mt.Description,
                    CriticalityLevel = mt.CriticalityLevel,
                    Done = mt.Done
                }).ToList();

                // Order tasks by criticality level and then by entry date starting from the newest entry
                var orderedMaintenanceTasks = maintenanceTasks
                    .OrderBy(mt => mt.CriticalityLevel)
                    .ThenByDescending(mt => mt.EntryDate)
                    .ToList();

                return orderedMaintenanceTasks;
            }
            catch (DbUpdateException ex)
            {
                throw new DatabaseException("Error occurred while getting the tasks from the database. " + ex.InnerException?.Message ?? ex.Message);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error occurred while getting the tasks. " + ex.InnerException?.Message ?? ex.Message);
            }
        }

        public MaintenanceTaskDto GetMaintenanceTask(int taskId)
        {
            try
            {
                var task = _dbContext.MaintenanceTasks.FirstOrDefault(mt => mt.Id == taskId);
                if (task == null)
                {
                    throw new NotFoundException("Task not found.");
                }

                var TaskDto = new MaintenanceTaskDto
                {
                    Id = task.Id,
                    DeviceId = task.DeviceId,
                    EntryDate = task.EntryDate,
                    Description = task.Description,
                    CriticalityLevel = task.CriticalityLevel,
                    Done = task.Done
                };

                return TaskDto;
            }
            catch (DbUpdateException ex)
            {
                throw new DatabaseException("Error occurred while getting the task from the database. " + ex.InnerException?.Message ?? ex.Message);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error occurred while getting the task. " + ex.InnerException?.Message ?? ex.Message);
            }
        }

        public async Task<MaintenanceTask> AddTask(MaintenanceTaskDto taskDto)
        {
            try
            {
                var newTask = new MaintenanceTask
                {
                    DeviceId = taskDto.DeviceId,
                    EntryDate = DateTime.Now, // Automatically choose the current time, despite the user's input
                    Description = taskDto.Description,
                    CriticalityLevel = taskDto.CriticalityLevel,
                    Done = false // All new tasks are 'undone' by default
                };

                _dbContext.MaintenanceTasks.Add(newTask);
                await _dbContext.SaveChangesAsync();

                return newTask;
            }
            catch (DbUpdateException ex)
            {
                throw new DatabaseException("Error occurred while adding the task to the database. " + ex.InnerException?.Message ?? ex.Message);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error occurred while adding the task. " + ex.InnerException?.Message ?? ex.Message);
            }
        }

        public async Task<MaintenanceTask> UpdateTask(int taskId, MaintenanceTaskDto updatedTaskDto)
        {
            try
            {
                var taskToUpdate = _dbContext.MaintenanceTasks.FirstOrDefault(fd => fd.Id == taskId);
                if (taskToUpdate == null)
                {
                    throw new NotFoundException("Task not found, no task updated.");
                }

                // Note: Id, deviceId and EntryDate can't be updated in this case
                taskToUpdate.Description = updatedTaskDto.Description;
                taskToUpdate.CriticalityLevel = updatedTaskDto.CriticalityLevel;
                taskToUpdate.Done = updatedTaskDto.Done;

                await _dbContext.SaveChangesAsync();

                return taskToUpdate;
            }
            catch (DbUpdateException ex)
            {
                throw new DatabaseException("Error occurred while updating the task in the database. " + ex.InnerException?.Message ?? ex.Message);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error occurred while updating the task. " + ex.InnerException?.Message ?? ex.Message);
            }
        }

        public async Task DeleteTask(int taskId)
        {
            try
            {
                var taskToDelete = _dbContext.MaintenanceTasks.Find(taskId);
                if (taskToDelete == null)
                {
                    throw new NotFoundException("Task not found, no task deleted.");
                }

                _dbContext.MaintenanceTasks.Remove(taskToDelete);
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                throw new DatabaseException("Error occurred while deleting the task from the database. " + ex.InnerException?.Message ?? ex.Message);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error occurred while deleting the task. " + ex.InnerException?.Message ?? ex.Message);
            }
        }
    }
}
