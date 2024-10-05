using FHotel.Services.DTOs.Cities;
using FHotel.Services.DTOs.OrderDetails;
using FHotel.Services.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FHotel.API.Controllers
{
    /// <summary>
    /// Controller for managing order-detail.
    /// </summary>
    [Route("api/order-details")]
    [ApiController]
    public class OrderDetailsController : ControllerBase
    {
        private readonly IOrderDetailService _orderDetailService;

        public OrderDetailsController(IOrderDetailService orderDetailService)
        {
            _orderDetailService = orderDetailService;
        }

        /// <summary>
        /// Get a list of all order-details.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<OrderDetailResponse>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<List<OrderDetailResponse>>> GetAll()
        {
            try
            {
                var rs = await _orderDetailService.GetAll();
                return Ok(rs);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Get order-detail by order-detail id.
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OrderDetailResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<OrderDetailResponse>> Get(Guid id)
        {
            try
            {
                var rs = await _orderDetailService.Get(id);
                return Ok(rs);
            }
            catch
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Create new order-detail.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<OrderDetailResponse>> Create([FromBody] OrderDetailRequest request)
        {
            try
            {
                var result = await _orderDetailService.Create(request);
                return CreatedAtAction(nameof(Create), result);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Delete order-detail by order-detail id.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult<OrderDetailResponse>> Delete(Guid id)
        {
            var rs = await _orderDetailService.Delete(id);
            return Ok(rs);
        }

        /// <summary>
        /// Update order-detail by order-detail id.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<OrderDetailResponse>> Update(Guid id, [FromBody] OrderDetailRequest request)
        {
            try
            {
                var rs = await _orderDetailService.Update(id, request);
                return Ok(rs);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
