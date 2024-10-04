using FHotel.Service.DTOs.LateCheckOutPolicies;
using FHotel.Service.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FHotel.API.Controllers
{
    [Route("api/lateCheckOutPolicies")]
    [ApiController]
    public class LateCheckOutPoliciesController : ControllerBase
    {
        private readonly ILateCheckOutPolicyService _lateCheckOutPolicyService;

        public LateCheckOutPoliciesController(ILateCheckOutPolicyService lateCheckOutPolicyService)
        {
            _lateCheckOutPolicyService = lateCheckOutPolicyService;
        }

        /// <summary>
        /// Get a list of all lateCheckOutPolicys.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<LateCheckOutPolicyResponse>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<List<LateCheckOutPolicyResponse>>> GetAll()
        {
            try
            {
                var rs = await _lateCheckOutPolicyService.GetAll();
                return Ok(rs);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Get lateCheckOutPolicy by lateCheckOutPolicy id.
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LateCheckOutPolicyResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<LateCheckOutPolicyResponse>> Get(Guid id)
        {
            try
            {
                var rs = await _lateCheckOutPolicyService.Get(id);
                return Ok(rs);
            }
            catch
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Create new lateCheckOutPolicy.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<LateCheckOutPolicyResponse>> Create([FromBody] LateCheckOutPolicyRequest request)
        {
            try
            {
                var result = await _lateCheckOutPolicyService.Create(request);
                return CreatedAtAction(nameof(Create), result);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Delete lateCheckOutPolicy by lateCheckOutPolicy id.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult<LateCheckOutPolicyResponse>> Delete(Guid id)
        {
            var rs = await _lateCheckOutPolicyService.Delete(id);
            return Ok(rs);
        }

        /// <summary>
        /// Update lateCheckOutPolicy by lateCheckOutPolicy id.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<LateCheckOutPolicyResponse>> Update(Guid id, [FromBody] LateCheckOutPolicyRequest request)
        {
            try
            {
                var rs = await _lateCheckOutPolicyService.Update(id, request);
                return Ok(rs);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
