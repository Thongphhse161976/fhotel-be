using FHotel.Service.DTOs.HotelStaffs;
using FHotel.Service.Services.Interfaces;
using FHotel.Services.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace FHotel.API.Controllers
{
    /// <summary>
    /// Controller for managing hotel-staffs.
    /// </summary>
    [Route("api/hotel-staffs")]
    [ApiController]
    public class HotelStaffsController : ControllerBase
    {
        private readonly IHotelStaffService _hotelStaffService;

        public HotelStaffsController(IHotelStaffService hotelStaffService)
        {
            _hotelStaffService = hotelStaffService;
        }

        /// <summary>
        /// Get a list of all hotel-staffs.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<HotelStaffResponse>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<List<HotelStaffResponse>>> GetAll()
        {
            try
            {
                var rs = await _hotelStaffService.GetAll();
                return Ok(rs);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Get hotelStaff by hotel-staff id.
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(HotelStaffResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<HotelStaffResponse>> Get(Guid id)
        {
            try
            {
                var rs = await _hotelStaffService.Get(id);
                return Ok(rs);
            }
            catch
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Assign staff to a hotel.
        /// </summary>
        [HttpPost("assign-staff")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<HotelStaffResponse>> CreateHotelStaff([FromBody] HotelStaffCreateRequest request)
        {
            if (request ==null)
            {
                return BadRequest(new { message = "UserId is required." });
            }

            try
            {
                var result = await _hotelStaffService.Create(request); // Call service with hotelId and UserId
                return CreatedAtAction(nameof(CreateHotelStaff), result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An unexpected error occurred.", details = ex.Message });
            }
        }

        /// <summary>
        /// Delete hotelStaff by hotel-staff id.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult<HotelStaffResponse>> Delete(Guid id)
        {
            var rs = await _hotelStaffService.Delete(id);
            return Ok(rs);
        }

        /// <summary>
        /// Update hotelStaff by hotel-staff id.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<HotelStaffResponse>> Update(Guid id, [FromBody] HotelStaffCreateRequest request)
        {
            try
            {
                var rs = await _hotelStaffService.Update(id, request);
                return Ok(rs);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
