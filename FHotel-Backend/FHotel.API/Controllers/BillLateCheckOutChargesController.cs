using FHotel.Service.DTOs.BillLateCheckOutCharges;
using FHotel.Service.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FHotel.API.Controllers
{
    [Route("api/billLateCheckOutCharges")]
    [ApiController]
    public class BillLateCheckOutChargesController : ControllerBase
    {
        private readonly IBillLateCheckOutChargeService _billLateCheckOutChargeService;

        public BillLateCheckOutChargesController(IBillLateCheckOutChargeService billLateCheckOutChargeService)
        {
            _billLateCheckOutChargeService = billLateCheckOutChargeService;
        }

        /// <summary>
        /// Get a list of all billLateCheckOutCharges.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<BillLateCheckOutChargeResponse>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<List<BillLateCheckOutChargeResponse>>> GetAll()
        {
            try
            {
                var rs = await _billLateCheckOutChargeService.GetAll();
                return Ok(rs);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Get billLateCheckOutCharge by billLateCheckOutCharge id.
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BillLateCheckOutChargeResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<BillLateCheckOutChargeResponse>> Get(Guid id)
        {
            try
            {
                var rs = await _billLateCheckOutChargeService.Get(id);
                return Ok(rs);
            }
            catch
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Create new billLateCheckOutCharge.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BillLateCheckOutChargeResponse>> Create([FromBody] BillLateCheckOutChargeRequest request)
        {
            try
            {
                var result = await _billLateCheckOutChargeService.Create(request);
                return CreatedAtAction(nameof(Create), result);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Delete BillLateCheckOutCharge by BillLateCheckOutCharge id.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult<BillLateCheckOutChargeResponse>> Delete(Guid id)
        {
            var rs = await _billLateCheckOutChargeService.Delete(id);
            return Ok(rs);
        }

        /// <summary>
        /// Update BillLateCheckOutCharge by BillLateCheckOutCharge id.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<BillLateCheckOutChargeResponse>> Update(Guid id, [FromBody] BillLateCheckOutChargeRequest request)
        {
            try
            {
                var rs = await _billLateCheckOutChargeService.Update(id, request);
                return Ok(rs);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
