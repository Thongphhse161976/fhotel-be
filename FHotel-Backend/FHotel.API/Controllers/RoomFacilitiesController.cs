using FHotel.Services.DTOs.Cities;
using FHotel.Services.DTOs.RoomFacilities;
using FHotel.Services.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FHotel.API.Controllers
{
    [Route("api/roomFacilities")]
    [ApiController]
    public class RoomFacilitiesController : ControllerBase
    {
        private readonly IRoomFacilityService _roomFacilityService;

        public RoomFacilitiesController(IRoomFacilityService roomFacilityService)
        {
            _roomFacilityService = roomFacilityService;
        }

        /// <summary>
        /// Get a list of all roomFacilitys.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<RoomFacilityResponse>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<List<RoomFacilityResponse>>> GetAll()
        {
            try
            {
                var rs = await _roomFacilityService.GetAll();
                return Ok(rs);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Get roomFacility by roomFacility id.
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RoomFacilityResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<RoomFacilityResponse>> Get(Guid id)
        {
            try
            {
                var rs = await _roomFacilityService.Get(id);
                return Ok(rs);
            }
            catch
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Create new roomFacility.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<RoomFacilityResponse>> Create([FromBody] RoomFacilityRequest request)
        {
            try
            {
                var result = await _roomFacilityService.Create(request);
                return CreatedAtAction(nameof(Create), result);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Delete roomFacility by roomFacility id.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult<RoomFacilityResponse>> Delete(Guid id)
        {
            var rs = await _roomFacilityService.Delete(id);
            return Ok(rs);
        }

        /// <summary>
        /// Update roomFacility by roomFacility id.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<RoomFacilityResponse>> Update(Guid id, [FromBody] RoomFacilityRequest request)
        {
            try
            {
                var rs = await _roomFacilityService.Update(id, request);
                return Ok(rs);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
