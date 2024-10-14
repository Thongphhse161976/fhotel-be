using FHotel.Services.DTOs.Cities;
using FHotel.Services.DTOs.Payments;
using FHotel.Services.Services.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FHotel.API.Controllers
{
    /// <summary>
    /// Controller for managing payment.
    /// </summary>
    [Route("api/payments")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentsController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        /// <summary>
        /// Get a list of all payments.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<PaymentResponse>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<List<PaymentResponse>>> GetAll()
        {
            try
            {
                var rs = await _paymentService.GetAll();
                return Ok(rs);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Get payment by payment id.
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaymentResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<PaymentResponse>> Get(Guid id)
        {
            try
            {
                var rs = await _paymentService.Get(id);
                return Ok(rs);
            }
            catch
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Create new payment.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<PaymentResponse>> Create([FromBody] PaymentRequest request)
        {
            try
            {
                var result = await _paymentService.Create(request);
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
        /// Delete payment by payment id.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult<PaymentResponse>> Delete(Guid id)
        {
            var rs = await _paymentService.Delete(id);
            return Ok(rs);
        }

        /// <summary>
        /// Update payment by payment id.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<PaymentResponse>> Update(Guid id, [FromBody] PaymentRequest request)
        {
            try
            {
                var rs = await _paymentService.Update(id, request);
                return Ok(rs);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
