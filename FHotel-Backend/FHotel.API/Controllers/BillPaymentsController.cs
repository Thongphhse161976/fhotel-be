using FHotel.Service.DTOs.BillPayments;
using FHotel.Service.Services.Interfaces;
using FHotel.Services.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FHotel.API.Controllers
{
    /// <summary>
    /// Controller for managing bill-payment.
    /// </summary>
    [Route("api/bill-payments")]
    [ApiController]
    public class BillPaymentsController : ControllerBase
    {
        private readonly IBillPaymentService _billPaymentService;

        public BillPaymentsController(IBillPaymentService billPaymentService)
        {
            _billPaymentService = billPaymentService;
        }

        /// <summary>
        /// Get a list of all bill-payments.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<BillPaymentResponse>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<List<BillPaymentResponse>>> GetAll()
        {
            try
            {
                var rs = await _billPaymentService.GetAll();
                return Ok(rs);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Get bill-payment by bill-payment id.
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BillPaymentResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<BillPaymentResponse>> Get(Guid id)
        {
            try
            {
                var rs = await _billPaymentService.Get(id);
                return Ok(rs);
            }
            catch
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Create new bill-payment.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BillPaymentResponse>> Create([FromBody] BillPaymentRequest request)
        {
            try
            {
                var result = await _billPaymentService.Create(request);
                return CreatedAtAction(nameof(Create), result);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Delete bill-payment by bill-payment id.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult<BillPaymentResponse>> Delete(Guid id)
        {
            var rs = await _billPaymentService.Delete(id);
            return Ok(rs);
        }

        /// <summary>
        /// Update bill-payment by bill-payment id.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<BillPaymentResponse>> Update(Guid id, [FromBody] BillPaymentRequest request)
        {
            try
            {
                var rs = await _billPaymentService.Update(id, request);
                return Ok(rs);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
