using FHotel.Service.DTOs.Refunds;
using FHotel.Service.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FHotel.API.Controllers
{
    /// <summary>
    /// Controller for managing refund.
    /// </summary>
    [Route("api/refunds")]
    [ApiController]
    public class RefundsController : ControllerBase
    {
        private readonly IRefundService _refundService;

        public RefundsController(IRefundService refundService)
        {
            _refundService = refundService;
        }

        /// <summary>
        /// Get a list of all refunds.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<RefundResponse>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<List<RefundResponse>>> GetAll()
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
        /// Get refund by refund id.
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RefundResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<RefundResponse>> Get(Guid id)
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
        /// Create new refund.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<RefundResponse>> Create([FromBody] RefundRequest request)
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
        /// Delete refund by refund id.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult<RefundResponse>> Delete(Guid id)
        {
            var rs = await _refundService.Delete(id);
            return Ok(rs);
        }

        /// <summary>
        /// Update refund by refund id.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<RefundResponse>> Update(Guid id, [FromBody] RefundRequest request)
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
