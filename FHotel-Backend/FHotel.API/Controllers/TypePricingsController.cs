using FHotel.Service.DTOs.TypePricings;
using FHotel.Service.Services.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FHotel.API.Controllers
{
    /// <summary>
    /// Controller for managing type-pricing.
    /// </summary>
    [Route("api/type-pricings")]
    [ApiController]
    public class TypePricingsController : ControllerBase
    {
        private readonly ITypePricingService _typePricingService;

        public TypePricingsController(ITypePricingService typePricingService)
        {
            _typePricingService = typePricingService;
        }

        /// <summary>
        /// Get a list of all type-pricings.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<TypePricingResponse>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<List<TypePricingResponse>>> GetAll()
        {
            try
            {
                var rs = await _typePricingService.GetAll();
                return Ok(rs);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Get type-pricing by type-pricing id.
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TypePricingResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<TypePricingResponse>> Get(Guid id)
        {
            try
            {
                var rs = await _typePricingService.Get(id);
                return Ok(rs);
            }
            catch
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Create new type-pricing.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<TypePricingResponse>> Create([FromBody] TypePricingCreateRequest request)
        {
            try
            {
                var result = await _typePricingService.Create(request);
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
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }

        /// <summary>
        /// Delete type-pricing by type-pricing id.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult<TypePricingResponse>> Delete(Guid id)
        {
            var rs = await _typePricingService.Delete(id);
            return Ok(rs);
        }

        /// <summary>
        /// Update type-pricing by type-pricing id.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<TypePricingResponse>> Update(Guid id, [FromBody] TypePricingUpdateRequest request)
        {
            try
            {
                var rs = await _typePricingService.Update(id, request);
                return Ok(rs);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
