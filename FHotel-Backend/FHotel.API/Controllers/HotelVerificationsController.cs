using FHotel.Service.DTOs.HotelVerifications;
using FHotel.Service.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FHotel.API.Controllers
{
    /// <summary>
    /// Controller for managing hotel-verification.
    /// </summary>
    [Route("api/hotel-verifications")]
    [ApiController]
    public class HotelVerificationsController : ControllerBase
    {
        private readonly IHotelVerificationService _hotelVerificationService;
        private readonly IDistrictService _districtService;

        public HotelVerificationsController(IHotelVerificationService hotelVerificationService, IDistrictService districtService)
        {
            _hotelVerificationService = hotelVerificationService;
            _districtService = districtService;
        }

        /// <summary>
        /// Get a list of all hotel-verifications.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<HotelVerificationResponse>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<List<HotelVerificationResponse>>> GetAll()
        {
            try
            {
                var rs = await _hotelVerificationService.GetAll();
                return Ok(rs);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Get hotel-verification by hotel-verification id.
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(HotelVerificationResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<HotelVerificationResponse>> Get(Guid id)
        {
            try
            {
                var rs = await _hotelVerificationService.Get(id);
                return Ok(rs);
            }
            catch
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Create new hotel-verification.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<HotelVerificationResponse>> Create([FromBody] HotelVerificationRequest request)
        {
            try
            {
                var result = await _hotelVerificationService.Create(request);
                return CreatedAtAction(nameof(Create), result);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Delete hotel-verification by hotel-verification id.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult<HotelVerificationResponse>> Delete(Guid id)
        {
            var rs = await _hotelVerificationService.Delete(id);
            return Ok(rs);
        }

        /// <summary>
        /// Update hotel-verification by hotel-verification id.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<HotelVerificationResponse>> Update(Guid id, [FromBody] HotelVerificationRequest request)
        {
            try
            {
                var rs = await _hotelVerificationService.Update(id, request);
                return Ok(rs);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
