using FHotel.Services.DTOs.Cities;
using FHotel.Services.DTOs.HotelAmenities;
using FHotel.Services.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FHotel.API.Controllers
{
    [Route("api/hotelAmenities")]
    [ApiController]
    public class HotelAmenitiesController : ControllerBase
    {
        private readonly IHotelAmenityService _hotelAmenityService;

        public HotelAmenitiesController(IHotelAmenityService hotelAmenityService)
        {
            _hotelAmenityService = hotelAmenityService;
        }

        /// <summary>
        /// Get a list of all hotelAmenitys.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<HotelAmenityResponse>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<List<HotelAmenityResponse>>> GetAll()
        {
            try
            {
                var rs = await _hotelAmenityService.GetAll();
                return Ok(rs);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Get hotelAmenity by hotelAmenity id.
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(HotelAmenityResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<HotelAmenityResponse>> Get(Guid id)
        {
            try
            {
                var rs = await _hotelAmenityService.Get(id);
                return Ok(rs);
            }
            catch
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Create new hotelAmenity.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<HotelAmenityResponse>> Create([FromBody] HotelAmenityRequest request)
        {
            try
            {
                var result = await _hotelAmenityService.Create(request);
                return CreatedAtAction(nameof(Create), result);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Delete hotelAmenity by hotelAmenity id.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult<HotelAmenityResponse>> Delete(Guid id)
        {
            var rs = await _hotelAmenityService.Delete(id);
            return Ok(rs);
        }

        /// <summary>
        /// Update hotelAmenity by hotelAmenity id.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<HotelAmenityResponse>> Update(Guid id, [FromBody] HotelAmenityRequest request)
        {
            try
            {
                var rs = await _hotelAmenityService.Update(id, request);
                return Ok(rs);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
