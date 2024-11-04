using FHotel.Repository.Models;
using FHotel.Service.DTOs.Bills;
using FHotel.Service.DTOs.HolidayPricingRules;
using FHotel.Service.DTOs.Holidays;
using FHotel.Service.Services.Implementations;
using FHotel.Service.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FHotel.API.Controllers
{
    /// <summary>
    /// Controller for managing holiday.
    /// </summary>
    [Route("api/holidays")]
    [ApiController]
    public class HolidaysController : ControllerBase
    {
        private readonly IHolidayService _holidayService;
        private readonly IHolidayPricingRuleService _holidayPricingRuleService;

        public HolidaysController(IHolidayService holidayService, IHolidayPricingRuleService holidayPricingRuleService)
        {
            _holidayService = holidayService;
            _holidayPricingRuleService = holidayPricingRuleService;
        }

        /// <summary>
        /// Get a list of all holidays.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<HolidayResponse>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<List<HolidayResponse>>> GetAll()
        {
            try
            {
                var rs = await _holidayService.GetAll();
                return Ok(rs);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Get holiday by holiday id.
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(HolidayResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<HolidayResponse>> Get(Guid id)
        {
            try
            {
                var rs = await _holidayService.Get(id);
                return Ok(rs);
            }
            catch
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Create new holiday.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<HolidayResponse>> Create([FromBody] HolidayRequest request)
        {
            try
            {
                var result = await _holidayService.Create(request);
                return CreatedAtAction(nameof(Create), result);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Delete holiday by holiday id.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult<HolidayResponse>> Delete(Guid id)
        {
            var rs = await _holidayService.Delete(id);
            return Ok(rs);
        }

        /// <summary>
        /// Update holiday by holiday id.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<HolidayResponse>> Update(Guid id, [FromBody] HolidayRequest request)
        {
            try
            {
                var rs = await _holidayService.Update(id, request);
                return Ok(rs);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Get all holiday pricing rule by holiday ID.
        /// </summary>
        [HttpGet("{id}/holiday-pricing-rules")]

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<HolidayPricingRuleResponse>>> GetAllBillByStaffId(Guid id)
        {
            try
            {
                var rules = await _holidayPricingRuleService.GetAllHolidayPricingRuleByHolidayId(id);

                if (rules == null || !rules.Any())
                {
                    return NotFound(new { message = "No rules found for this holiday." });
                }

                return Ok(rules);
            }
            catch (Exception ex)
            {
                // Log the exception if you have logging set up
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An unexpected error occurred.", details = ex.Message });
            }
        }
    }
}
