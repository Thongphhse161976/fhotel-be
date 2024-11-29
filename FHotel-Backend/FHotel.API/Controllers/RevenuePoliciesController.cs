using FHotel.Service.DTOs.RevenuePolicies;
using FHotel.Service.Services.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FHotel.API.Controllers
{
    /// <summary>
    /// Controller for managing revenue policy.
    /// </summary>
    [Route("api/revenue-policies")]
    [ApiController]
    public class RevenuePoliciesController : ControllerBase
    {
        private readonly IRevenuePolicyService _revenuePolicyService;

        public RevenuePoliciesController(IRevenuePolicyService revenuePolicyService)
        {
            _revenuePolicyService = revenuePolicyService;
        }

        /// <summary>
        /// Get a list of all revenue policies.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<RevenuePolicyResponse>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<List<RevenuePolicyResponse>>> GetAll()
        {
            try
            {
                var rs = await _revenuePolicyService.GetAll();
                return Ok(rs);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Get revenue policy by revenue policy id.
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RevenuePolicyResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<RevenuePolicyResponse>> Get(Guid id)
        {
            try
            {
                var rs = await _revenuePolicyService.Get(id);
                return Ok(rs);
            }
            catch
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Create new revenue policy.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<RevenuePolicyResponse>> Create([FromBody] RevenuePolicyRequest request)
        {
            try
            {
                var result = await _revenuePolicyService.Create(request);
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
        /// Delete revenue policy by revenue policy id.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult<RevenuePolicyResponse>> Delete(Guid id)
        {
            var rs = await _revenuePolicyService.Delete(id);
            return Ok(rs);
        }

        /// <summary>
        /// Update revenue policy by revenue policy id.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<RevenuePolicyResponse>> Update(Guid id, [FromBody] RevenuePolicyRequest request)
        {
            try
            {
                var rs = await _revenuePolicyService.Update(id, request);
                return Ok(rs);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
