using FHotel.Service.DTOs.Amenities;
using FHotel.Service.Services.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FHotel.API.Controllers
{
    /// <summary>
    /// Controller for managing amenity.
    /// </summary>
    [Route("api/amenities")]
    [ApiController]
    public class AmenitiesController : ControllerBase
    {
        private readonly IAmenityService _amenityService;

        public AmenitiesController(IAmenityService amenityService)
        {
            _amenityService = amenityService;
        }

        /// <summary>
        /// Get a list of all amenities.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<AmenityResponse>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<List<AmenityResponse>>> GetAll()
        {
            try
            {
                var rs = await _amenityService.GetAll();
                return Ok(rs);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Get amenity by amenity id.
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AmenityResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<AmenityResponse>> Get(Guid id)
        {
            try
            {
                var rs = await _amenityService.Get(id);
                return Ok(rs);
            }
            catch
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Create new amenity.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<AmenityResponse>> Create([FromBody] AmenityRequest request)
        {
            try
            {
                var result = await _amenityService.Create(request);
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
        /// Delete amenity by amenity id.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult<AmenityResponse>> Delete(Guid id)
        {
            var rs = await _amenityService.Delete(id);
            return Ok(rs);
        }

        /// <summary>
        /// Update amenity by amenity id.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<AmenityResponse>> Update(Guid id, [FromBody] AmenityRequest request)
        {
            try
            {
                var rs = await _amenityService.Update(id, request);
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
    }
}
