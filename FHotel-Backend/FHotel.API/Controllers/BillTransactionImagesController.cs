using FHotel.Service.DTOs.BillTransactionImages;
using FHotel.Service.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FHotel.API.Controllers
{
    /// <summary>
    /// Controller for managing bill-transaction-image.
    /// </summary>
    [Route("api/bill-transaction-images")]
    [ApiController]
    public class BillTransactionImagesController : ControllerBase
    {
        private readonly IBillTransactionImageService _billTransactionImageService;

        public BillTransactionImagesController(IBillTransactionImageService billTransactionImageService)
        {
            _billTransactionImageService = billTransactionImageService;
        }

        /// <summary>
        /// Get a list of all bill-transaction-images.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<BillTransactionImageResponse>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<List<BillTransactionImageResponse>>> GetAll()
        {
            try
            {
                var rs = await _billTransactionImageService.GetAll();
                return Ok(rs);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Get bill-transaction-image by bill-transaction-image id.
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BillTransactionImageResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<BillTransactionImageResponse>> Get(Guid id)
        {
            try
            {
                var rs = await _billTransactionImageService.Get(id);
                return Ok(rs);
            }
            catch
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Create new bill-transaction-image.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BillTransactionImageResponse>> Create([FromBody] BillTransactionImageRequest request)
        {
            try
            {
                var result = await _billTransactionImageService.Create(request);
                return CreatedAtAction(nameof(Create), result);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Delete bill-transaction-image by bill-transaction-image id.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult<BillTransactionImageResponse>> Delete(Guid id)
        {
            var rs = await _billTransactionImageService.Delete(id);
            return Ok(rs);
        }

        /// <summary>
        /// Update bill-transaction-image by bill-transaction-image id.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<BillTransactionImageResponse>> Update(Guid id, [FromBody] BillTransactionImageRequest request)
        {
            try
            {
                var rs = await _billTransactionImageService.Update(id, request);
                return Ok(rs);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
