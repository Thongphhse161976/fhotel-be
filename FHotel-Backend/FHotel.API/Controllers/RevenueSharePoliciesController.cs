using FHotel.Service.Services.Interfaces;
using FHotel.Services.DTOs.RevenueSharePolicies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FHotel.API.Controllers
{
    /// <summary>
    /// Controller for managing revenue share policy.
    /// </summary>
    [Route("api/revenue-share-policies")]
    [ApiController]
    public class RevenueSharePoliciesController : ControllerBase
    {
        private readonly IRevenueSharePolicyService _refundService;

        public RevenueSharePoliciesController(IRevenueSharePolicyService refundService)
        {
            _refundService = refundService;
        }

        /// <summary>
        /// Get a list of all refunds.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<RevenueSharePolicyResponse>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<List<RevenueSharePolicyResponse>>> GetAll()
        {
            try
            {
                var rs = await _refundService.GetAll();
                return Ok(rs);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Get revenue share policy by revenue share policy id.
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RevenueSharePolicyResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<RevenueSharePolicyResponse>> Get(Guid id)
        {
            try
            {
                var rs = await _refundService.Get(id);
                return Ok(rs);
            }
            catch
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Create new revenue share policy.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<RevenueSharePolicyResponse>> Create([FromBody] RevenueSharePolicyRequest request)
        {
            try
            {
                var result = await _refundService.Create(request);
                return CreatedAtAction(nameof(Create), result);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Delete revenue share policy by revenue share policy id.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult<RevenueSharePolicyResponse>> Delete(Guid id)
        {
            var rs = await _refundService.Delete(id);
            return Ok(rs);
        }

        /// <summary>
        /// Update revenue share policy by revenue share policy id.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<RevenueSharePolicyResponse>> Update(Guid id, [FromBody] RevenueSharePolicyRequest request)
        {
            try
            {
                var rs = await _refundService.Update(id, request);
                return Ok(rs);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
