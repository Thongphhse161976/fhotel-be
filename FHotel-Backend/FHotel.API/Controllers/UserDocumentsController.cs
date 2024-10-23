using FHotel.Service.Services.Interfaces;
using FHotel.Services.DTOs.UserDocuments;
using FHotel.Services.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FHotel.API.Controllers
{
    /// <summary>
    /// Controller for managing user document.
    /// </summary>
    [Route("api/user-documents")]
    [ApiController]
    public class UserDocumentsController : ControllerBase
    {
        private readonly IUserDocumentService _userDocumentService;

        public UserDocumentsController(IUserDocumentService userDocumentService)
        {
            _userDocumentService = userDocumentService;
        }

        /// <summary>
        /// Get a list of all user-documents.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<UserDocumentResponse>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<List<UserDocumentResponse>>> GetAll()
        {
            try
            {
                var rs = await _userDocumentService.GetAll();
                return Ok(rs);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Get user-document by user-document id.
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserDocumentResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<UserDocumentResponse>> Get(Guid id)
        {
            try
            {
                var rs = await _userDocumentService.Get(id);
                return Ok(rs);
            }
            catch
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Create new user-document.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<UserDocumentResponse>> Create([FromBody] UserDocumentRequest request)
        {
            try
            {
                var result = await _userDocumentService.Create(request);
                return CreatedAtAction(nameof(Create), result);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Delete user-document by user-document id.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult<UserDocumentResponse>> Delete(Guid id)
        {
            var rs = await _userDocumentService.Delete(id);
            return Ok(rs);
        }

        /// <summary>
        /// Update user-document by user-document id.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<UserDocumentResponse>> Update(Guid id, [FromBody] UserDocumentRequest request)
        {
            try
            {
                var rs = await _userDocumentService.Update(id, request);
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
                var fileLink = await _userDocumentService.UploadImage(file);

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
