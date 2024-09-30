using FHotel.Services.DTOs.Cities;
using FHotel.Services.DTOs.DamagedFactilities;
using FHotel.Services.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FHotel.API.Controllers
{
    [Route("api/damagedFacilities")]
    [ApiController]
    public class DamagedFacilitiesController : ControllerBase
    {
        private readonly IDamagedFacilityService _damagedFacilityService;

        public DamagedFacilitiesController(IDamagedFacilityService damagedFacilityService)
        {
            _damagedFacilityService = damagedFacilityService;
        }

        /// <summary>
        /// Get a list of all damagedFacilitys.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<DamagedFacilityResponse>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<List<DamagedFacilityResponse>>> GetAll()
        {
            try
            {
                var rs = await _damagedFacilityService.GetAll();
                return Ok(rs);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Get damagedFacility by damagedFacility id.
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DamagedFacilityResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<DamagedFacilityResponse>> Get(Guid id)
        {
            try
            {
                var rs = await _damagedFacilityService.Get(id);
                return Ok(rs);
            }
            catch
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Create new damagedFacility.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<DamagedFacilityResponse>> Create([FromBody] DamagedFacilityRequest request)
        {
            try
            {
                var result = await _damagedFacilityService.Create(request);
                return CreatedAtAction(nameof(Create), result);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Delete damagedFacility by damagedFacility id.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult<DamagedFacilityResponse>> Delete(Guid id)
        {
            var rs = await _damagedFacilityService.Delete(id);
            return Ok(rs);
        }

        /// <summary>
        /// Update damagedFacility by damagedFacility id.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<DamagedFacilityResponse>> Update(Guid id, [FromBody] DamagedFacilityRequest request)
        {
            try
            {
                var rs = await _damagedFacilityService.Update(id, request);
                return Ok(rs);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
