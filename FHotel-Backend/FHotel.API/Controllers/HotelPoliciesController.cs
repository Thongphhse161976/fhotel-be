using FHotel.Service.DTOs.HotelPolicies;
using FHotel.Service.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FHotel.API.Controllers
{
    /// <summary>
    /// Controller for managing hotel policy.
    /// </summary>
    [Route("api/hotel-policies")]
    [ApiController]
    public class HotelPoliciesController : ControllerBase
    {
        private readonly IHotelPolicyService _hotelPolicyService;

        public HotelPoliciesController(IHotelPolicyService hotelPolicyService)
        {
            _hotelPolicyService = hotelPolicyService;
        }

        /// <summary>
        /// Get a list of all hotelPolicys.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<HotelPolicyResponse>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<List<HotelPolicyResponse>>> GetAll()
        {
            try
            {
                var rs = await _hotelPolicyService.GetAll();
                return Ok(rs);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Get hotelPolicy by hotelPolicy id.
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(HotelPolicyResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<HotelPolicyResponse>> Get(Guid id)
        {
            try
            {
                var rs = await _hotelPolicyService.Get(id);
                return Ok(rs);
            }
            catch
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Create new hotelPolicy.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<HotelPolicyResponse>> Create([FromBody] HotelPolicyRequest request)
        {
            try
            {
                var result = await _hotelPolicyService.Create(request);
                return CreatedAtAction(nameof(Create), result);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Delete hotelPolicy by hotelPolicy id.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult<HotelPolicyResponse>> Delete(Guid id)
        {
            var rs = await _hotelPolicyService.Delete(id);
            return Ok(rs);
        }

        /// <summary>
        /// Update hotelPolicy by hotelPolicy id.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<HotelPolicyResponse>> Update(Guid id, [FromBody] HotelPolicyRequest request)
        {
            try
            {
                var rs = await _hotelPolicyService.Update(id, request);
                return Ok(rs);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
