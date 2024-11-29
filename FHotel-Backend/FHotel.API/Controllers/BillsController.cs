using FHotel.Repository.Models;
using FHotel.Service.DTOs.Bills;
using FHotel.Service.DTOs.BillTransactionImages;
using FHotel.Service.DTOs.Transactions;
using FHotel.Service.DTOs.VnPayConfigs;
using FHotel.Service.Services.Implementations;
using FHotel.Service.Services.Interfaces;
using FHotel.Services.DTOs.Orders;
using FHotel.Services.DTOs.Reservations;
using FHotel.Services.Services.Implementations;
using FHotel.Services.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FHotel.API.Controllers
{
    /// <summary>
    /// Controller for managing bill.
    /// </summary>
    [Route("api/bills")]
    [ApiController]
    public class BillsController : ControllerBase
    {
        private readonly IBillService _billService;
        private readonly IOrderService _orderService;
        private readonly ITransactionService _transactionService;
        private readonly IUserService _userService;
        private readonly IBillTransactionImageService _billTransactionImageService;

        public BillsController(IBillService billService, IBillTransactionImageService billTransactionImageService
            , IOrderService orderService, ITransactionService transactionService, IUserService userService)
        {
            _billService = billService;
            _billTransactionImageService = billTransactionImageService;
            _orderService = orderService;
            _transactionService = transactionService;
            _userService = userService;
        }

        /// <summary>
        /// Get a list of all bills.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<BillResponse>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<List<BillResponse>>> GetAll()
        {
            try
            {
                var rs = await _billService.GetAll();
                return Ok(rs);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Get bill by bill id.
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BillResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<BillResponse>> Get(Guid id)
        {
            try
            {
                var rs = await _billService.Get(id);
                return Ok(rs);
            }
            catch
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Create new bill.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BillResponse>> Create([FromBody] BillRequest request)
        {
            try
            {
                var result = await _billService.Create(request);
                return CreatedAtAction(nameof(Create), result);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Delete bill by bill id.
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BillResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<BillResponse>> Delete(Guid id)
        {
            var rs = await _billService.Delete(id);
            return Ok(rs);
        }

        /// <summary>
        /// Update bill by bill id.
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BillResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<BillResponse>> Update(Guid id, [FromBody] BillRequest request)
        {
            try
            {
                var rs = await _billService.Update(id, request);
                return Ok(rs);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Get all bill transaction images by bill id.
        /// </summary>
        [HttpGet("{id}/bill-transaction-images")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BillTransactionImageResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<List<BillTransactionImageResponse>>> GetBillTransactionImageByBill(Guid id)
        {
            try
            {
                var images = await _billTransactionImageService.GetBillTransactionImageByBill(id);
                return Ok(images);
            }
            catch
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Get all order by bill id.
        /// </summary>
        [HttpGet("{id}/orders")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OrderResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<List<OrderResponse>>> GetAllOrderByBill(Guid id)
        {
            try
            {
                var orders = await _orderService.GetAllOrderByBillId(id);
                return Ok(orders);
            }
            catch
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Get all transaction by bill id.
        /// </summary>
        [HttpGet("{id}/transactions")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TransactionResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<List<TransactionResponse>>> GetAllTransactionByBill(Guid id)
        {
            try
            {
                var transactions = await _transactionService.GetAllTransactionByBillId(id);
                return Ok(transactions);
            }
            catch
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Pay through VnPay by bill id.
        /// </summary>
        [HttpPost("{id}/pay")]
        public async Task<ActionResult<string>> Pay(Guid id)
        {
            try
            {
                var paymentUrl = await _billService.Pay(id, HttpContext);
                if (paymentUrl == null)
                {
                    return NotFound("Bill not found.");
                }

                return Ok(paymentUrl);
            }
            catch (Exception ex)
            {
                // Log the error if necessary
                return StatusCode(500, ex.Message);
            }
        }
    }
}
