using FHotel.Services.DTOs.Cities;
using FHotel.Services.DTOs.RoomImages;
using FHotel.Services.Services.Implementations;
using FHotel.Services.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FHotel.API.Controllers
{
    /// <summary>
    /// Controller for managing room-image.
    /// </summary>
    [Route("api/room-images")]
    [ApiController]
    public class RoomImagesController : ControllerBase
    {
        private readonly IRoomImageService _roomImageService;

        public RoomImagesController(IRoomImageService roomImageService)
        {
            _roomImageService = roomImageService;
        }

        /// <summary>
        /// Get a list of all room-images.
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
        /// Get room-image by room-image id.
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
        /// Create new room-image.
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
        /// Delete room-image by room-image id.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult<RoomImageResponse>> Delete(Guid id)
        {
            var rs = await _roomImageService.Delete(id);
            return Ok(rs);
        }

        /// <summary>
        /// Update room-image by room-image id.
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

        /// <summary>
        /// Upload room image.
        /// </summary>
        [HttpPost("image")]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            // Check if file is present in the request
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file was uploaded.");
            }

            try
            {
                // Call the upload service method
                var fileLink = await _roomImageService.UploadImage(file);

                if (string.IsNullOrEmpty(fileLink))
                {
                    return StatusCode(500, "An error occurred while uploading the file.");
                }

                // Return the link to the uploaded file
                return Ok(new { link = fileLink });
            }
            catch (Exception ex)
            {
                // Handle exceptions, log if necessary
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
    }
}
