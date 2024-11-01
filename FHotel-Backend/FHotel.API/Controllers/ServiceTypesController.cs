using FHotel.Services.DTOs.Cities;
using FHotel.Services.DTOs.OrderDetails;
using FHotel.Services.DTOs.Services;
using FHotel.Services.DTOs.ServiceTypes;
using FHotel.Services.Services.Implementations;
using FHotel.Services.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FHotel.API.Controllers
{
    /// <summary>
    /// Controller for managing service-type.
    /// </summary>
    [Route("api/service-types")]
    [ApiController]
    public class ServiceTypesController : ControllerBase
    {
        private readonly IServiceTypeService _serviceTypeService;
        private readonly IServiceService _serviceService;

        public ServiceTypesController(IServiceTypeService serviceTypeService, IServiceService serviceService)
        {
            _serviceTypeService = serviceTypeService;
            _serviceService = serviceService;
        }

        /// <summary>
        /// Get a list of all service-types.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ServiceTypeResponse>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<List<ServiceTypeResponse>>> GetAll()
        {
            try
            {
                var rs = await _serviceTypeService.GetAll();
                return Ok(rs);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Get service-type by service-type id.
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ServiceTypeResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<ServiceTypeResponse>> Get(Guid id)
        {
            try
            {
                var rs = await _serviceTypeService.Get(id);
                return Ok(rs);
            }
            catch
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Create new service-type.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ServiceTypeResponse>> Create([FromBody] ServiceTypeRequest request)
        {
            try
            {
                var result = await _serviceTypeService.Create(request);
                return CreatedAtAction(nameof(Create), result);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Delete service-type by service-type id.
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ServiceTypeResponse>> Delete(Guid id)
        {
            var rs = await _serviceTypeService.Delete(id);
            return Ok(rs);
        }

        /// <summary>
        /// Update service-type by service-type id.
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ServiceTypeResponse>> Update(Guid id, [FromBody] ServiceTypeRequest request)
        {
            try
            {
                var rs = await _serviceTypeService.Update(id, request);
                return Ok(rs);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        /// <summary>
        /// Get all service by service type id.
        /// </summary>
        [HttpGet("{id}/services")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<ServiceResponse>>> GetAllOrderDetailByOrder(Guid id)
        {
            try
            {
                var services = await _serviceService.GetAllServiceByServiceTypeId(id);

                if (services == null || !services.Any())
                {
                    return NotFound(new { message = "No services found for this type." });
                }

                return Ok(services);
            }
            catch (Exception ex)
            {
                // Log the exception if you have logging set up
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An unexpected error occurred.", details = ex.Message });
            }
        }
    }
}
