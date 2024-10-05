using FHotel.Services.DTOs.Cities;
using FHotel.Services.DTOs.PaymentMethods;
using FHotel.Services.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FHotel.API.Controllers
{
    [Route("api/payment-methods")]
    [ApiController]
    public class PaymentMethodsController : ControllerBase
    {
        private readonly IPaymentMethodService _paymentMethodService;

        public PaymentMethodsController(IPaymentMethodService paymentMethodService)
        {
            _paymentMethodService = paymentMethodService;
        }

        /// <summary>
        /// Get a list of all paymentMethods.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<PaymentMethodResponse>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<List<PaymentMethodResponse>>> GetAll()
        {
            try
            {
                var rs = await _paymentMethodService.GetAll();
                return Ok(rs);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Get paymentMethod by paymentMethod id.
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaymentMethodResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<PaymentMethodResponse>> Get(Guid id)
        {
            try
            {
                var rs = await _paymentMethodService.Get(id);
                return Ok(rs);
            }
            catch
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Create new paymentMethod.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<PaymentMethodResponse>> Create([FromBody] PaymentMethodRequest request)
        {
            try
            {
                var result = await _paymentMethodService.Create(request);
                return CreatedAtAction(nameof(Create), result);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Delete paymentMethod by paymentMethod id.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult<PaymentMethodResponse>> Delete(Guid id)
        {
            var rs = await _paymentMethodService.Delete(id);
            return Ok(rs);
        }

        /// <summary>
        /// Update paymentMethod by paymentMethod id.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<PaymentMethodResponse>> Update(Guid id, [FromBody] PaymentMethodRequest request)
        {
            try
            {
                var rs = await _paymentMethodService.Update(id, request);
                return Ok(rs);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
