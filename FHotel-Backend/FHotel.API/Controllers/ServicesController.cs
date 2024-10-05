using FHotel.Services.DTOs.Cities;
using FHotel.Services.DTOs.Services;
using FHotel.Services.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FHotel.API.Controllers
{
    /// <summary>
    /// Controller for managing service.
    /// </summary>
    [Route("api/services")]
    [ApiController]
    public class ServicesController : ControllerBase
    {
        private readonly IServiceService _serviceService;

        public ServicesController(IServiceService serviceService)
        {
            _serviceService = serviceService;
        }

        /// <summary>
        /// Get a list of all services.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ServiceResponse>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<List<ServiceResponse>>> GetAll()
        {
            try
            {
                var rs = await _serviceService.GetAll();
                return Ok(rs);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Get service by service id.
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ServiceResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<ServiceResponse>> Get(Guid id)
        {
            try
            {
                var rs = await _serviceService.Get(id);
                return Ok(rs);
            }
            catch
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Create new service.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ServiceResponse>> Create([FromBody] ServiceRequest request)
        {
            try
            {
                var result = await _serviceService.Create(request);
                return CreatedAtAction(nameof(Create), result);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Delete service by service id.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult<ServiceResponse>> Delete(Guid id)
        {
            var rs = await _serviceService.Delete(id);
            return Ok(rs);
        }

        /// <summary>
        /// Update service by service id.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<ServiceResponse>> Update(Guid id, [FromBody] ServiceRequest request)
        {
            try
            {
                var rs = await _serviceService.Update(id, request);
                return Ok(rs);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
