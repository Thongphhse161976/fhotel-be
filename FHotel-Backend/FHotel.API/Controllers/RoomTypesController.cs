using FHotel.Service.DTOs.RoomTypes;
using FHotel.Services.DTOs.Cities;
using FHotel.Services.DTOs.RoomTypes;
using FHotel.Services.Services.Implementations;
using FHotel.Services.Services.Interfaces;
using FluentValidation;
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
        public async Task<ActionResult<RoomTypeResponse>> Create([FromBody] RoomTypeCreateRequest request)
        {
            try
            {
                var result = await _roomTypeService.Create(request);
                return CreatedAtAction(nameof(Create), result);
            }
            catch (ValidationException ex)
            {
                // Access validation errors from ex.Errors
                return BadRequest(new
                {
                    message = "Validation failed",
                    errors = ex.Errors.Select(e => e.ErrorMessage).ToList()
                });
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
        public async Task<ActionResult<RoomTypeResponse>> Update(Guid id, [FromBody] RoomTypeUpdateRequest request)
        {
            try
            {
                var rs = await _roomTypeService.Update(id, request);
                return Ok(rs);
            }
            catch (ValidationException ex)
            {
                // Access validation errors from ex.Errors
                return BadRequest(new
                {
                    message = "Validation failed",
                    errors = ex.Errors.Select(e => e.ErrorMessage).ToList()
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Upload room type image.
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
                var fileLink = await _roomTypeService.UploadImage(file);

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
