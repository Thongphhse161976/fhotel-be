using FHotel.Service.DTOs.RefundPolicies;
using FHotel.Service.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FHotel.API.Controllers
{
    [Route("api/refund-policies")]
    [ApiController]
    public class RefundPoliciesController : ControllerBase
    {
        private readonly IRefundPolicyService _refundPolicyService;

        public RefundPoliciesController(IRefundPolicyService refundPolicyService)
        {
            _refundPolicyService = refundPolicyService;
        }

        /// <summary>
        /// Get a list of all refundPolicys.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<RefundPolicyResponse>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<List<RefundPolicyResponse>>> GetAll()
        {
            try
            {
                var rs = await _refundPolicyService.GetAll();
                return Ok(rs);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Get refundPolicy by refundPolicy id.
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RefundPolicyResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<RefundPolicyResponse>> Get(Guid id)
        {
            try
            {
                var rs = await _refundPolicyService.Get(id);
                return Ok(rs);
            }
            catch
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Create new refundPolicy.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<RefundPolicyResponse>> Create([FromBody] RefundPolicyRequest request)
        {
            try
            {
                var result = await _refundPolicyService.Create(request);
                return CreatedAtAction(nameof(Create), result);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Delete refundPolicy by refundPolicy id.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult<RefundPolicyResponse>> Delete(Guid id)
        {
            var rs = await _refundPolicyService.Delete(id);
            return Ok(rs);
        }

        /// <summary>
        /// Update refundPolicy by refundPolicy id.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<RefundPolicyResponse>> Update(Guid id, [FromBody] RefundPolicyRequest request)
        {
            try
            {
                var rs = await _refundPolicyService.Update(id, request);
                return Ok(rs);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
