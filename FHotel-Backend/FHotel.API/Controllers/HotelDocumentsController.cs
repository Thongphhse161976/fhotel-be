using FHotel.Services.DTOs.HotelDocuments;
using FHotel.Services.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FHotel.API.Controllers
{
    /// <summary>
    /// Controller for managing hotel document.
    /// </summary>
    [Route("api/hotel-documents")]
    [ApiController]
    public class HotelDocumentsController : ControllerBase
    {
        private readonly IHotelDocumentService _hotelDocumentService;

        public HotelDocumentsController(IHotelDocumentService hotelDocumentService)
        {
            _hotelDocumentService = hotelDocumentService;
        }

        /// <summary>
        /// Get a list of all hotel-documents.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<HotelDocumentResponse>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<List<HotelDocumentResponse>>> GetAll()
        {
            try
            {
                var rs = await _hotelDocumentService.GetAll();
                return Ok(rs);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Get hotel-document by hotel-document id.
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(HotelDocumentResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<HotelDocumentResponse>> Get(Guid id)
        {
            try
            {
                var rs = await _hotelDocumentService.Get(id);
                return Ok(rs);
            }
            catch
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Create new hotel-document.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<HotelDocumentResponse>> Create([FromBody] HotelDocumentRequest request)
        {
            try
            {
                var result = await _hotelDocumentService.Create(request);
                return CreatedAtAction(nameof(Create), result);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Delete hotel-document by hotel-document id.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult<HotelDocumentResponse>> Delete(Guid id)
        {
            var rs = await _hotelDocumentService.Delete(id);
            return Ok(rs);
        }

        /// <summary>
        /// Update hotel-document by hotel-document id.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<HotelDocumentResponse>> Update(Guid id, [FromBody] HotelDocumentRequest request)
        {
            try
            {
                var rs = await _hotelDocumentService.Update(id, request);
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
                var fileLink = await _hotelDocumentService.UploadImage(file);

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
