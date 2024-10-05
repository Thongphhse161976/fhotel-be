using FHotel.Services.DTOs.Cities;
using FHotel.Services.DTOs.HotelRegistations;
using FHotel.Services.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FHotel.API.Controllers
{
    /// <summary>
    /// Controller for managing hotel-registration.
    /// </summary>
    [Route("api/hotel-registrations")]
    [ApiController]
    public class HotelRegistrationsController : ControllerBase
    {
        private readonly IHotelRegistrationService _hotelRegistrationService;

        public HotelRegistrationsController(IHotelRegistrationService hotelRegistrationService)
        {
            _hotelRegistrationService = hotelRegistrationService;
        }

        /// <summary>
        /// Get a list of all hotel-registrations.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<HotelRegistrationResponse>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<List<HotelRegistrationResponse>>> GetAll()
        {
            try
            {
                var rs = await _hotelRegistrationService.GetAll();
                return Ok(rs);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Get hotel-registration by hotel-registration id.
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(HotelRegistrationResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<HotelRegistrationResponse>> Get(Guid id)
        {
            try
            {
                var rs = await _hotelRegistrationService.Get(id);
                return Ok(rs);
            }
            catch
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Create new hotel-registration.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<HotelRegistrationResponse>> Create([FromBody] HotelRegistrationRequest request)
        {
            try
            {
                var result = await _hotelRegistrationService.Create(request);
                return CreatedAtAction(nameof(Create), result);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Delete hotel-registration by hotel-registration id.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult<HotelRegistrationResponse>> Delete(Guid id)
        {
            var rs = await _hotelRegistrationService.Delete(id);
            return Ok(rs);
        }

        /// <summary>
        /// Update hotel-registration by hotel-registration id.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<HotelRegistrationResponse>> Update(Guid id, [FromBody] HotelRegistrationRequest request)
        {
            try
            {
                var rs = await _hotelRegistrationService.Update(id, request);
                return Ok(rs);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
