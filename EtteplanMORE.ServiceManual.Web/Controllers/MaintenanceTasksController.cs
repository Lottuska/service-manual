using EtteplanMORE.ServiceManual.ApplicationCore.Dtos;
using EtteplanMORE.ServiceManual.ApplicationCore.Interfaces;
using EtteplanMORE.ServiceManual.ApplicationCore.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace EtteplanMORE.ServiceManual.Web.Controllers
{
    [Route("api/[controller]")]
    public class MaintenanceTasksController : Controller
    {
        private readonly IMaintenanceTaskService _maintenanceTaskService;

        public MaintenanceTasksController(IMaintenanceTaskService maintenanceTaskService)
        {
            _maintenanceTaskService = maintenanceTaskService;
        }
        /// <summary>
        ///     HTTP GET: api/maintenancetasks/
        /// </summary>
        [HttpGet]
        public IActionResult GetAllMaintenanceTasks()
        {
            try
            {
                var tasks = _maintenanceTaskService.GetAllMaintenanceTasks();
                return Ok(tasks);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (DatabaseException ex)
            {
                return StatusCode(500, ex.Message); // Internal Server Error
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message); // Internal Server Error
            }
        }

        /// <summary>
        ///     HTTP GET: api/maintenancetasks/3
        /// </summary>
        [HttpGet("{id}")]
        public ActionResult<MaintenanceTaskDto> GetMaintenanceTask(int id)
        {
            try
            {
                var task = _maintenanceTaskService.GetMaintenanceTask(id);
                return Ok(task);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (DatabaseException ex)
            {
                return StatusCode(500, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        ///     HTTP POST: api/maintenancetasks/
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> AddTask([FromBody] MaintenanceTaskDto taskDto)
        {
            try
            {
                var task = await _maintenanceTaskService.AddTask(taskDto);
                return Ok(task);
            }
            catch (DatabaseException ex)
            {
                return StatusCode(500, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        ///     HTTP UPDATE: api/maintenancetasks/3
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTask(int id, [FromBody] MaintenanceTaskDto updateTaskDto)
        {
            try
            {
                var task = await _maintenanceTaskService.UpdateTask(id, updateTaskDto);
                return Ok(task);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (DatabaseException ex)
            {
                return StatusCode(500, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        ///     HTTP DELETE: api/maintenancetasks/3
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            try
            {
                await _maintenanceTaskService.DeleteTask(id);
                return Ok();
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (DatabaseException ex)
            {
                return StatusCode(500, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}