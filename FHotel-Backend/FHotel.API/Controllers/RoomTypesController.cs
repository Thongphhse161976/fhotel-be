using FHotel.Services.DTOs.Cities;
using FHotel.Services.DTOs.RoomTypes;
using FHotel.Services.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FHotel.API.Controllers
{
    /// <summary>
    /// Controller for managing room-type.
    /// </summary>
    [Route("api/room-types")]
    [ApiController]
    public class RoomTypesController : ControllerBase
    {
        private readonly IRoomTypeService _roomTypeService;

        public RoomTypesController(IRoomTypeService roomTypeService)
        {
            _roomTypeService = roomTypeService;
        }

        /// <summary>
        /// Get a list of all room-types.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<RoomTypeResponse>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<List<RoomTypeResponse>>> GetAll()
        {
            try
            {
                var rs = await _roomTypeService.GetAll();
                return Ok(rs);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Get room-type by room-type id.
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RoomTypeResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<RoomTypeResponse>> Get(Guid id)
        {
            try
            {
                var rs = await _roomTypeService.Get(id);
                return Ok(rs);
            }
            catch
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Create new room-type.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<RoomTypeResponse>> Create([FromBody] RoomTypeRequest request)
        {
            try
            {
                var result = await _roomTypeService.Create(request);
                return CreatedAtAction(nameof(Create), result);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Delete room-type by room-type id.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult<RoomTypeResponse>> Delete(Guid id)
        {
            var rs = await _roomTypeService.Delete(id);
            return Ok(rs);
        }

        /// <summary>
        /// Update room-type by room-type id.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<RoomTypeResponse>> Update(Guid id, [FromBody] RoomTypeRequest request)
        {
            try
            {
                var rs = await _roomTypeService.Update(id, request);
                return Ok(rs);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
