using System.Collections.Generic;
using System.Threading.Tasks;
using EtteplanMORE.ServiceManual.ApplicationCore.Entities;
using EtteplanMORE.ServiceManual.ApplicationCore.Data;
using EtteplanMORE.ServiceManual.ApplicationCore.Dtos;

namespace EtteplanMORE.ServiceManual.ApplicationCore.Interfaces
{
    public interface IMaintenanceTaskService
    {
        // Get all tasks
        List<MaintenanceTaskDto> GetAllMaintenanceTasks();

        // Get one task
        MaintenanceTaskDto GetMaintenanceTask(int taskId);

        // Add new task
        Task<MaintenanceTask> AddTask(MaintenanceTaskDto taskDto);

        // Update specific task
        Task<MaintenanceTask> UpdateTask(int taskId, MaintenanceTaskDto updateTaskDto);

        // Delete specific task
        Task DeleteTask(int taskId);
    }
}