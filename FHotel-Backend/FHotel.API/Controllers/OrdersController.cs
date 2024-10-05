using FHotel.Services.DTOs.Cities;
using FHotel.Services.DTOs.Orders;
using FHotel.Services.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FHotel.API.Controllers
{
    /// <summary>
    /// Controller for managing order.
    /// </summary>
    [Route("api/orders")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        /// <summary>
        /// Get a list of all orders.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<OrderResponse>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<List<OrderResponse>>> GetAll()
        {
            try
            {
                var rs = await _orderService.GetAll();
                return Ok(rs);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Get order by order id.
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OrderResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<OrderResponse>> Get(Guid id)
        {
            try
            {
                var rs = await _orderService.Get(id);
                return Ok(rs);
            }
            catch
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Create new order.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<OrderResponse>> Create([FromBody] OrderRequest request)
        {
            try
            {
                var result = await _orderService.Create(request);
                return CreatedAtAction(nameof(Create), result);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Delete order by order id.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult<OrderResponse>> Delete(Guid id)
        {
            var rs = await _orderService.Delete(id);
            return Ok(rs);
        }

        /// <summary>
        /// Update order by order id.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<OrderResponse>> Update(Guid id, [FromBody] OrderRequest request)
        {
            try
            {
                var rs = await _orderService.Update(id, request);
                return Ok(rs);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
