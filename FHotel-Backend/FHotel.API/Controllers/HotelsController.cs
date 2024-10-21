using FHotel.Service.DTOs.Amenities;
using FHotel.Service.DTOs.Hotels;
using FHotel.Service.DTOs.HotelStaffs;
using FHotel.Service.Services.Interfaces;
using FHotel.Services.DTOs.HotelAmenities;
using FHotel.Services.DTOs.Hotels;
using FHotel.Services.DTOs.Reservations;
using FHotel.Services.DTOs.RoomTypes;
using FHotel.Services.Services.Implementations;
using FHotel.Services.Services.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FHotel.API.Controllers
{
    /// <summary>
    /// Controller for managing hotel.
    /// </summary>
    [Route("api/hotels")]
    [ApiController]
    public class HotelsController : ControllerBase
    {
        private readonly IHotelService _hotelService;
        private readonly IHotelAmenityService _hotelAmenityService;
        private readonly IHotelStaffService _hotelStaffService;
        private readonly IRoomTypeService _roomTypeService;
        private readonly IReservationService _reservationService;

        public HotelsController(IHotelService hotelService, IHotelStaffService hotelStaffService, IRoomTypeService roomTypeService, IHotelAmenityService hotelAmenityService, IReservationService reservationService)
        {
            _hotelService = hotelService;
            _hotelStaffService = hotelStaffService;
            _roomTypeService = roomTypeService;
            _hotelAmenityService = hotelAmenityService;
            _reservationService = reservationService;
        }

        /// <summary>
        /// Get a list of all hotels.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<HotelResponse>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<List<HotelResponse>>> GetAll()
        {
            try
            {
                var rs = await _hotelService.GetAll();
                return Ok(rs);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Get hotel by hotel id.
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(HotelResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<HotelResponse>> Get(Guid id)
        {
            try
            {
                var rs = await _hotelService.Get(id);
                return Ok(rs);
            }
            catch
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Get all hotel amenities by hotel id.
        /// </summary>
        [HttpGet("{id}/hotel-amenities")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(HotelAmenityResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<List<HotelAmenityResponse>>> GetHotelAmenityByHotel(Guid id)
        {
            try
            {
                var amenities = await _hotelService.GetHotelAmenityByHotel(id);
                return Ok(amenities);
            }
            catch
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Create new hotel.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<HotelResponse>> Create([FromBody] HotelCreateRequest request)
        {
            try
            {
                var result = await _hotelService.Create(request);
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
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Delete hotel by hotel id.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult<HotelResponse>> Delete(Guid id)
        {
            var rs = await _hotelService.Delete(id);
            return Ok(rs);
        }

        /// <summary>
        /// Update hotel by hotel id.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<HotelResponse>> Update(Guid id, [FromBody] HotelUpdateRequest request)
        {
            try
            {
                var rs = await _hotelService.Update(id, request);
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
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Upload hotel image.
        /// </summary>
        [HttpPost("image")]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            // Check if file is present in the request
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file was uploaded.");
            }

            try
            {
                // Call the upload service method
                var fileLink = await _hotelService.UploadImage(file);

                if (string.IsNullOrEmpty(fileLink))
                {
                    return StatusCode(500, "An error occurred while uploading the file.");
                }

                // Return the link to the uploaded file
                return Ok(new { link = fileLink });
            }
            catch (Exception ex)
            {
                // Handle exceptions, log if necessary
                return StatusCode(500, $"An error occurred: {ex.Message}");
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
            if (request.UserId == null)
            {
                return BadRequest(new { message = "UserId is required." });
            }

            try
            {
                var result = await _hotelStaffService.Create(request); // Pass hotelId to the service
                return CreatedAtAction(nameof(CreateHotelStaff),result);
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
                // Consider logging the exception
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An unexpected error occurred.", details = ex.Message });
            }
        }

        /// <summary>
        /// Get all staff members by hotel ID.
        /// </summary>
        /// <param name="hotelId">The ID of the hotel.</param>
        /// <returns>A list of hotel staff members.</returns>
        [HttpGet("{hotelId}/hotel-staffs")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<HotelStaffResponse>>> GetAllStaffByHotelId(Guid hotelId)
        {
            try
            {
                var staffList = await _hotelStaffService.GetAllStaffByHotelId(hotelId);

                if (staffList == null || !staffList.Any())
                {
                    return NotFound(new { message = "No staff found for this hotel." });
                }

                return Ok(staffList);
            }
            catch (Exception ex)
            {
                // Log the exception if you have logging set up
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An unexpected error occurred.", details = ex.Message });
            }
        }

        /// <summary>
        /// Get all room types by hotel ID.
        /// </summary>
        /// <param name="hotelId">The ID of the hotel.</param>
        /// <returns>A list of hotel room types.</returns>
        [HttpGet("{hotelId}/room-types")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<RoomTypeResponse>>> GetAllRoomTypeByHotelId(Guid hotelId)
        {
            try
            {
                var staffList = await _roomTypeService.GetAllRoomTypeByHotelId(hotelId);

                if (staffList == null || !staffList.Any())
                {
                    return NotFound(new { message = "No room type found for this hotel." });
                }

                return Ok(staffList);
            }
            catch (Exception ex)
            {
                // Log the exception if you have logging set up
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An unexpected error occurred.", details = ex.Message });
            }
        }
        
        /// <summary>
        /// Get all reservations by hotel id.
        /// </summary>
        /// <param name="hotelId">The ID of the hotel.</param>
        /// <returns>A list of hotel room types.</returns>
        [HttpGet("{hotelId}/reservations")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<ReservationResponse>>> GetAllReservationByHotelId(Guid hotelId)
        {
            try
            {
                var staffList = await _reservationService.GetAllByHotelId(hotelId);

                if (staffList == null || !staffList.Any())
                {
                    return NotFound(new { message = "No room type found for this hotel." });
                }

                return Ok(staffList);
            }
            catch (Exception ex)
            {
                // Log the exception if you have logging set up
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An unexpected error occurred.", details = ex.Message });
            }
        }
        ///// <summary>
        ///// Get all amenities by hotel ID.
        ///// </summary>
        ///// <param name="hotelId">The ID of the hotel.</param>
        ///// <returns>A list of hotel room types.</returns>
        //[HttpGet("{hotelId}/amenities")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //public async Task<ActionResult<IEnumerable<AmenityResponse>>> GetAllAmenityByHotelId(Guid hotelId)
        //{
        //    try
        //    {
        //        var amenityList = await _hotelAmenityService.GetAllAmenityByHotelId(hotelId);

        //        if (amenityList == null || !amenityList.Any())
        //        {
        //            return NotFound(new { message = "No amenity found for this hotel." });
        //        }

        //        return Ok(amenityList);
        //    }
        //    catch (Exception ex)
        //    {
        //        // Log the exception if you have logging set up
        //        return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An unexpected error occurred.", details = ex.Message });
        //    }
        //}

    }
}
