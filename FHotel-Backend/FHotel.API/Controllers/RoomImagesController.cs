using FHotel.Services.DTOs.Cities;
using FHotel.Services.DTOs.RoomImages;
using FHotel.Services.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FHotel.API.Controllers
{
    [Route("api/roomImages")]
    [ApiController]
    public class RoomImagesController : ControllerBase
    {
        private readonly IRoomImageService _roomImageService;

        public RoomImagesController(IRoomImageService roomImageService)
        {
            _roomImageService = roomImageService;
        }

        /// <summary>
        /// Get a list of all roomImages.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<RoomImageResponse>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<List<RoomImageResponse>>> GetAll()
        {
            try
            {
                var rs = await _roomImageService.GetAll();
                return Ok(rs);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Get roomImage by roomImage id.
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RoomImageResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<RoomImageResponse>> Get(Guid id)
        {
            try
            {
                var rs = await _roomImageService.Get(id);
                return Ok(rs);
            }
            catch
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Create new roomImage.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<RoomImageResponse>> Create([FromBody] RoomImageRequest request)
        {
            try
            {
                var result = await _roomImageService.Create(request);
                return CreatedAtAction(nameof(Create), result);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Delete roomImage by roomImage id.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult<RoomImageResponse>> Delete(Guid id)
        {
            var rs = await _roomImageService.Delete(id);
            return Ok(rs);
        }

        /// <summary>
        /// Update roomImage by roomImage id.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<RoomImageResponse>> Update(Guid id, [FromBody] RoomImageRequest request)
        {
            try
            {
                var rs = await _roomImageService.Update(id, request);
                return Ok(rs);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
