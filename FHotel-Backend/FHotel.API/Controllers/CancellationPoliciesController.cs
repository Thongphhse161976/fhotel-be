using FHotel.Service.DTOs.CancellationPolicies;
using FHotel.Service.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FHotel.API.Controllers
{
    /// <summary>
    /// Controller for managing cancellation policy.
    /// </summary>
    [Route("api/cancellation-policies")]
    [ApiController]
    public class CancellationPoliciesController : ControllerBase
    {
        private readonly ICancellationPolicyService _cancellationPolicyService;

        public CancellationPoliciesController(ICancellationPolicyService cancellationPolicyService)
        {
            _cancellationPolicyService = cancellationPolicyService;
        }

        /// <summary>
        /// Get a list of all cancellation policies.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<CancellationPolicyResponse>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<List<CancellationPolicyResponse>>> GetAll()
        {
            try
            {
                var rs = await _cancellationPolicyService.GetAll();
                return Ok(rs);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Get cancellation policy by cancellation policy id.
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CancellationPolicyResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<CancellationPolicyResponse>> Get(Guid id)
        {
            try
            {
                var rs = await _cancellationPolicyService.Get(id);
                return Ok(rs);
            }
            catch
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Create new cancellation policy.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CancellationPolicyResponse>> Create([FromBody] CancellationPolicyRequest request)
        {
            try
            {
                var result = await _cancellationPolicyService.Create(request);
                return CreatedAtAction(nameof(Create), result);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Delete cancellation policy by cancellation policy id.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult<CancellationPolicyResponse>> Delete(Guid id)
        {
            var rs = await _cancellationPolicyService.Delete(id);
            return Ok(rs);
        }

        /// <summary>
        /// Update cancellation policy by cancellation policy id.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<CancellationPolicyResponse>> Update(Guid id, [FromBody] CancellationPolicyRequest request)
        {
            try
            {
                var rs = await _cancellationPolicyService.Update(id, request);
                return Ok(rs);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
