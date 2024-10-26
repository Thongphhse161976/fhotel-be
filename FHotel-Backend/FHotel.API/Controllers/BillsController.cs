using FHotel.Service.DTOs.Bills;
using FHotel.Service.DTOs.BillTransactionImages;
using FHotel.Service.Services.Implementations;
using FHotel.Service.Services.Interfaces;
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
        private readonly IBillTransactionImageService _billTransactionImageService;

        public BillsController(IBillService billService, IBillTransactionImageService billTransactionImageService)
        {
            _billService = billService;
            _billTransactionImageService = billTransactionImageService;
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
        public async Task<ActionResult<BillResponse>> Delete(Guid id)
        {
            var rs = await _billService.Delete(id);
            return Ok(rs);
        }

        /// <summary>
        /// Update bill by bill id.
        /// </summary>
        [HttpPut("{id}")]
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
                var amenities = await _billTransactionImageService.GetBillTransactionImageByBill(id);
                return Ok(amenities);
            }
            catch
            {
                return NotFound();
            }
        }
    }
}
