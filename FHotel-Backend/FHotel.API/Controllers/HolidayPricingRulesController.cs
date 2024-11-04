using FHotel.Service.DTOs.HolidayPricingRules;
using FHotel.Service.Services.Interfaces;
using FHotel.Services.Services.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FHotel.API.Controllers
{
    /// <summary>
    /// Controller for managing holidayPricingRule.
    /// </summary>
    [Route("api/holiday-pricing-rules")]
    [ApiController]
    public class HolidayPricingRulesController : ControllerBase
    {
        private readonly IHolidayPricingRuleService _holidayPricingRuleService;

        public HolidayPricingRulesController(IHolidayPricingRuleService holidayPricingRuleService)
        {
            _holidayPricingRuleService = holidayPricingRuleService;
        }

        /// <summary>
        /// Get a list of all holiday pricing rule.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<HolidayPricingRuleResponse>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<List<HolidayPricingRuleResponse>>> GetAll()
        {
            try
            {
                var rs = await _holidayPricingRuleService.GetAll();
                return Ok(rs);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Get holiday pricing rule by holiday pricing rule id.
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(HolidayPricingRuleResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<HolidayPricingRuleResponse>> Get(Guid id)
        {
            try
            {
                var rs = await _holidayPricingRuleService.Get(id);
                return Ok(rs);
            }
            catch
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Create new holiday pricing rule.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<HolidayPricingRuleResponse>> Create([FromBody] HolidayPricingRuleCreateRequest request)
        {
            try
            {
                var result = await _holidayPricingRuleService.Create(request);
                return CreatedAtAction(nameof(Create), result);
            }
            catch (ValidationException ex)
            {
                // Access validation errors from ex.Errors
                return BadRequest(new
                {
                    message = "Validation failed",
                    errors = ex.Errors.Select(e => e.ErrorMessage).ToList()
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }

        /// <summary>
        /// Delete holiday pricing rule by holiday pricing rule id.
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(HolidayPricingRuleResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<HolidayPricingRuleResponse>> Delete(Guid id)
        {
            var rs = await _holidayPricingRuleService.Delete(id);
            return Ok(rs);
        }

        /// <summary>
        /// Update holiday pricing rule by holiday pricing rule id.
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(HolidayPricingRuleResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<HolidayPricingRuleResponse>> Update(Guid id, [FromBody] HolidayPricingRuleUpdateRequest request)
        {
            try
            {
                var rs = await _holidayPricingRuleService.Update(id, request);
                return Ok(rs);
            }
            catch (ValidationException ex)
            {
                // Access validation errors from ex.Errors
                return BadRequest(new
                {
                    message = "Validation failed",
                    errors = ex.Errors.Select(e => e.ErrorMessage).ToList()
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }
    }
}
