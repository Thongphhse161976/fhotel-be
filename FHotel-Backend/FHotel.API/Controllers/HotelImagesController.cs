using FHotel.Services.DTOs.HotelImages;
using FHotel.Services.Interfaces;
using FHotel.Services.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FHotel.API.Controllers
{
    [Route("api/hotel-images")]
    [ApiController]
    public class HotelImagesController : ControllerBase
    {
        private readonly IHotelImageService _hotelImageService;

        public HotelImagesController(IHotelImageService hotelImageService)
        {
            _hotelImageService = hotelImageService;
        }

        /// <summary>
        /// Get a list of all hotel-images.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<HotelImageResponse>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<List<HotelImageResponse>>> GetAll()
        {
            try
            {
                var rs = await _hotelImageService.GetAll();
                return Ok(rs);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Get hotel-image by hotel-image id.
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(HotelImageResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<HotelImageResponse>> Get(Guid id)
        {
            try
            {
                var rs = await _hotelImageService.Get(id);
                return Ok(rs);
            }
            catch
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Create new hotel-image.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<HotelImageResponse>> Create([FromBody] HotelImageRequest request)
        {
            try
            {
                var result = await _hotelImageService.Create(request);
                return CreatedAtAction(nameof(Create), result);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Delete hotel-image by hotel-image id.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult<HotelImageResponse>> Delete(Guid id)
        {
            var rs = await _hotelImageService.Delete(id);
            return Ok(rs);
        }

        /// <summary>
        /// Update hotel-image by hotel-image id.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<HotelImageResponse>> Update(Guid id, [FromBody] HotelImageRequest request)
        {
            try
            {
                var rs = await _hotelImageService.Update(id, request);
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
                var fileLink = await _hotelImageService.UploadImage(file);

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
