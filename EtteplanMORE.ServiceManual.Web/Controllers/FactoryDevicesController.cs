using EtteplanMORE.ServiceManual.ApplicationCore.Dtos;
using EtteplanMORE.ServiceManual.ApplicationCore.Interfaces;
using EtteplanMORE.ServiceManual.ApplicationCore.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace EtteplanMORE.ServiceManual.Web.Controllers
{
    [Route("api/[controller]")]
    public class FactoryDevicesController : Controller
    {
        private readonly IFactoryDeviceService _factoryDeviceService;

        public FactoryDevicesController(IFactoryDeviceService factoryDeviceService)
        {
            _factoryDeviceService = factoryDeviceService;
        }

        /// <summary>
        ///     HTTP GET: api/factorydevices/
        /// </summary>

        [HttpGet]
        public IActionResult GetAllFactoryDevices()
        {
            try
            {
                var devices = _factoryDeviceService.GetAllFactoryDevices();
                return Ok(devices);
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
        ///     HTTP GET: api/factorydevices/3
        /// </summary>
        [HttpGet("{id}")]
        public ActionResult<FactoryDeviceDto> GetFactoryDevice(int id)
        {
            try
            {
                var device = _factoryDeviceService.GetFactoryDevice(id);
                return Ok(device);
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
        ///     HTTP GET: api/factorydevices/3/tasks
        /// </summary>
        [HttpGet("{id}/tasks")]
        public ActionResult<List<MaintenanceTaskDto>> GetMaintenanceTasksForFactoryDevice(int id)
        {
            try
            {
                var tasks = _factoryDeviceService.GetMaintenanceTasksForFactoryDevice(id);
                return Ok(tasks);
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