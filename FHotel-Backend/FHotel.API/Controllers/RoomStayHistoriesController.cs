using FHotel.Service.DTOs.RoomStayHistories;
using FHotel.Service.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FHotel.API.Controllers
{
    /// <summary>
    /// Controller for managing room-stay-history.
    /// </summary>
    [Route("api/room-stay-histories")]
    [ApiController]
    public class RoomStayHistoriesController : ControllerBase
    {
        private readonly IRoomStayHistoryService _roomStayHistoryService;

        public RoomStayHistoriesController(IRoomStayHistoryService roomStayHistoryService)
        {
            _roomStayHistoryService = roomStayHistoryService;
        }

        /// <summary>
        /// Get a list of all room-stay-histories.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<RoomStayHistoryResponse>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<List<RoomStayHistoryResponse>>> GetAll()
        {
            try
            {
                var rs = await _roomStayHistoryService.GetAll();
                return Ok(rs);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Get room-stay-history by room-stay-history id.
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RoomStayHistoryResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<RoomStayHistoryResponse>> Get(Guid id)
        {
            try
            {
                var rs = await _roomStayHistoryService.Get(id);
                return Ok(rs);
            }
            catch
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Create new room-stay-history.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<RoomStayHistoryResponse>> Create([FromBody] RoomStayHistoryRequest request)
        {
            try
            {
                var result = await _roomStayHistoryService.Create(request);
                return CreatedAtAction(nameof(Create), result);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Delete room-stay-history by room-stay-history id.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult<RoomStayHistoryResponse>> Delete(Guid id)
        {
            var rs = await _roomStayHistoryService.Delete(id);
            return Ok(rs);
        }

        /// <summary>
        /// Update room-stay-history by room-stay-history id.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<RoomStayHistoryResponse>> Update(Guid id, [FromBody] RoomStayHistoryRequest request)
        {
            try
            {
                var rs = await _roomStayHistoryService.Update(id, request);
                return Ok(rs);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
