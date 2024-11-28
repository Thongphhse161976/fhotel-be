using FHotel.Service.DTOs.Districts;
using FHotel.Service.DTOs.HotelPolicies;
using FHotel.Service.DTOs.Policies;
using FHotel.Service.Services.Implementations;
using FHotel.Service.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FHotel.API.Controllers
{
    /// <summary>
    /// Controller for managing policy.
    /// </summary>
    [Route("api/policies")]
    [ApiController]
    public class PoliciesController : ControllerBase
    {
        private readonly IPolicyService _policyService;
        private readonly IHotelPolicyService _hotelPolicyService;

        public PoliciesController(IPolicyService policyService, IHotelPolicyService hotelPolicyService)
        {
            _policyService = policyService;
            _hotelPolicyService = hotelPolicyService;
        }

        /// <summary>
        /// Get a list of all policys.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<PolicyResponse>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<List<PolicyResponse>>> GetAll()
        {
            try
            {
                var rs = await _policyService.GetAll();
                return Ok(rs);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Get policy by policy id.
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PolicyResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<PolicyResponse>> Get(Guid id)
        {
            try
            {
                var rs = await _policyService.Get(id);
                return Ok(rs);
            }
            catch
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Create new policy.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<PolicyResponse>> Create([FromBody] PolicyRequest request)
        {
            try
            {
                var result = await _policyService.Create(request);
                return CreatedAtAction(nameof(Create), result);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Delete policy by policy id.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult<PolicyResponse>> Delete(Guid id)
        {
            var rs = await _policyService.Delete(id);
            return Ok(rs);
        }

        /// <summary>
        /// Update policy by policy id.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<PolicyResponse>> Update(Guid id, [FromBody] PolicyRequest request)
        {
            try
            {
                var rs = await _policyService.Update(id, request);
                return Ok(rs);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Get a list of all hotel policies by policy id.
        /// </summary>
        [HttpGet("{id}/hotel-policies")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<HotelPolicyResponse>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<List<HotelPolicyResponse>>> GetAllHotelPolicyByPolicyId(Guid id)
        {
            try
            {
                var rs = await _hotelPolicyService.GetAllHotelPolicyByPolicyId(id);
                return Ok(rs);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
