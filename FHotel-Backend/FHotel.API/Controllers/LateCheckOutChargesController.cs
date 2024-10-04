using FHotel.Service.DTOs.LateCheckOutCharges;
using FHotel.Service.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FHotel.API.Controllers
{
    [Route("api/lateCheckOutCharges")]
    [ApiController]
    public class LateCheckOutChargesController : ControllerBase
    {
        private readonly ILateCheckOutChargeService _lateCheckOutChargeService;

        public LateCheckOutChargesController(ILateCheckOutChargeService lateCheckOutChargeService)
        {
            _lateCheckOutChargeService = lateCheckOutChargeService;
        }

        /// <summary>
        /// Get a list of all lateCheckOutCharges.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<LateCheckOutChargeResponse>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<List<LateCheckOutChargeResponse>>> GetAll()
        {
            try
            {
                var rs = await _lateCheckOutChargeService.GetAll();
                return Ok(rs);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Get lateCheckOutCharge by lateCheckOutCharge id.
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LateCheckOutChargeResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<LateCheckOutChargeResponse>> Get(Guid id)
        {
            try
            {
                var rs = await _lateCheckOutChargeService.Get(id);
                return Ok(rs);
            }
            catch
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Create new lateCheckOutCharge.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<LateCheckOutChargeResponse>> Create([FromBody] LateCheckOutChargeRequest request)
        {
            try
            {
                var result = await _lateCheckOutChargeService.Create(request);
                return CreatedAtAction(nameof(Create), result);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Delete lateCheckOutCharge by lateCheckOutCharge id.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult<LateCheckOutChargeResponse>> Delete(Guid id)
        {
            var rs = await _lateCheckOutChargeService.Delete(id);
            return Ok(rs);
        }

        /// <summary>
        /// Update lateCheckOutCharge by lateCheckOutCharge id.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<LateCheckOutChargeResponse>> Update(Guid id, [FromBody] LateCheckOutChargeRequest request)
        {
            try
            {
                var rs = await _lateCheckOutChargeService.Update(id, request);
                return Ok(rs);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
