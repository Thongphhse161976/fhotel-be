using FHotel.Service.DTOs.BillOrders;
using FHotel.Service.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FHotel.API.Controllers
{
    [Route("api/bill-orders")]
    [ApiController]
    public class BillOrdersController : ControllerBase
    {
        private readonly IBillOrderService _billOrderService;

        public BillOrdersController(IBillOrderService billOrderService)
        {
            _billOrderService = billOrderService;
        }

        /// <summary>
        /// Get a list of all billOrders.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<BillOrderResponse>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<List<BillOrderResponse>>> GetAll()
        {
            try
            {
                var rs = await _billOrderService.GetAll();
                return Ok(rs);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Get billOrder by billOrder id.
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BillOrderResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<BillOrderResponse>> Get(Guid id)
        {
            try
            {
                var rs = await _billOrderService.Get(id);
                return Ok(rs);
            }
            catch
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Create new billOrder.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BillOrderResponse>> Create([FromBody] BillOrderRequest request)
        {
            try
            {
                var result = await _billOrderService.Create(request);
                return CreatedAtAction(nameof(Create), result);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Delete billOrder by billOrder id.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult<BillOrderResponse>> Delete(Guid id)
        {
            var rs = await _billOrderService.Delete(id);
            return Ok(rs);
        }

        /// <summary>
        /// Update billOrder by billOrder id.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<BillOrderResponse>> Update(Guid id, [FromBody] BillOrderRequest request)
        {
            try
            {
                var rs = await _billOrderService.Update(id, request);
                return Ok(rs);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
