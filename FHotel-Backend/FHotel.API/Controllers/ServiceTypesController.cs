using FHotel.Services.DTOs.Cities;
using FHotel.Services.DTOs.ServiceTypes;
using FHotel.Services.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FHotel.API.Controllers
{
    [Route("api/service-types")]
    [ApiController]
    public class ServiceTypesController : ControllerBase
    {
        private readonly IServiceTypeService _serviceTypeService;

        public ServiceTypesController(IServiceTypeService serviceTypeService)
        {
            _serviceTypeService = serviceTypeService;
        }

        /// <summary>
        /// Get a list of all serviceTypes.
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
        /// Get serviceType by serviceType id.
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
        /// Create new serviceType.
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
        /// Delete serviceType by serviceType id.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult<ServiceTypeResponse>> Delete(Guid id)
        {
            var rs = await _serviceTypeService.Delete(id);
            return Ok(rs);
        }

        /// <summary>
        /// Update serviceType by serviceType id.
        /// </summary>
        [HttpPut("{id}")]
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
    }
}
