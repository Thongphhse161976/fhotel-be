using FHotel.Service.DTOs.HotelStaffs;
using FHotel.Service.DTOs.Users;
using FHotel.Service.Profiles;
using FHotel.Service.Services.Implementations;
using FHotel.Service.Services.Interfaces;
using FHotel.Services.DTOs.Cities;
using FHotel.Services.DTOs.HotelAmenities;
using FHotel.Services.DTOs.HotelDocuments;
using FHotel.Services.DTOs.Hotels;
using FHotel.Services.DTOs.Orders;
using FHotel.Services.DTOs.Reservations;
using FHotel.Services.DTOs.Users;
using FHotel.Services.Services.Implementations;
using FHotel.Services.Services.Interfaces;
using Firebase.Auth;
using Firebase.Storage;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;

namespace FHotel.API.Controllers
{
    /// <summary>
    /// Controller for managing user.
    /// </summary>
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IReservationService _reservationService;
        private readonly IHotelStaffService _hotelStaffService;
        private readonly IHotelVerificationService _hotelVerificationService;
        private readonly IOrderService _orderService;
        public UsersController(IUserService userService, IReservationService reservationService,IHotelStaffService hotelStaffService, IHotelVerificationService hotelVerificationService, IOrderService orderService)
        {
            _userService = userService;
            _reservationService = reservationService;
            _hotelStaffService = hotelStaffService;
            _hotelVerificationService = hotelVerificationService;
            _orderService = orderService;
        }

        /// <summary>
        /// Get a list of all users.
        /// </summary>
        [HttpGet]
        //[Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<UserResponse>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<List<UserResponse>>> GetAll()
        {
            try
            {
                var rs = await _userService.GetAll();
                if (rs.IsNullOrEmpty()) // check list is null or empty
                {
                    return NotFound(new ApiResponse
                    {
                        Success = false,
                        Message = "No users found."
                    });
                }
                return Ok(rs);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Get user by user id.
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<UserResponse>> Get(Guid id)
        {
            try
            {
                var rs = await _userService.Get(id);
                return Ok(rs);
            }
            catch
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Create new user.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<UserResponse>> Create([FromBody] UserCreateRequest request)
        {
            try
            {
                var result = await _userService.Create(request);
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
        /// Delete user by user id.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult<UserResponse>> Delete(Guid id)
        {
            var rs = await _userService.Delete(id);
            return Ok(rs);
        }

        /// <summary>
        /// Update user by user id.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<UserResponse>> Update(Guid id, [FromBody] UserUpdateRequest request)
        {
            try
            {
                var rs = await _userService.Update(id, request);
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
        /// Upload user image.
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
                var fileLink = await _userService.UploadImage(file);

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
        /// Get all hotel by user id.
        /// </summary>
        [HttpGet("{id}/hotels")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(HotelResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<List<HotelResponse>>> GetHotelByUser(Guid id)
        {
            try
            {
                var hotel = await _userService.GetHotelByUser(id);
                return Ok(hotel);
            }
            catch
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Get all reservations by user id.
        /// </summary>
        /// <param name="Id">The ID of the user.</param>
        /// <returns>A list of hotel room types.</returns>
        [HttpGet("{id}/reservations")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<ReservationResponse>>> GetAllReservationByUserId(Guid id)
        {
            try
            {
                var staffList = await _reservationService.GetAllByUserId(id);

                if (staffList == null || !staffList.Any())
                {
                    return NotFound(new { message = "No reservation found for this user." });
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
        /// Get all staff members by owner ID.
        /// </summary>
        /// <param name="hotelId">The ID of the owner.</param>
        /// <returns>A list of hotel staff members.</returns>
        [HttpGet("{ownerId}/hotel-staffs")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<HotelStaffResponse>>> GetAllStaffByOnwerId(Guid ownerId)
        {
            try
            {
                var staffList = await _hotelStaffService.GetAllStaffByOwnerlId(ownerId);

                if (staffList == null || !staffList.Any())
                {
                    return NotFound(new { message = "No staff found for this owner." });
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
        /// Get all reservations by staff ID.
        /// </summary>
        [HttpGet("{staffId}/staff-reservations")]

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<ReservationResponse>>> GetAllReservationByStaffId(Guid staffId)
        {
            try
            {
                var reservationList = await _reservationService.GetAllReservationByStaffId(staffId);

                if (reservationList == null || !reservationList.Any())
                {
                    return NotFound(new { message = "No reservations found for this staff." });
                }

                return Ok(reservationList);
            }
            catch (Exception ex)
            {
                // Log the exception if you have logging set up
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An unexpected error occurred.", details = ex.Message });
            }
        }
        /// <summary>
        /// Get all reservations by owner id.
        /// </summary>
        /// <param name="Id">The ID of the user.</param>
        /// <returns>A list of hotel room types.</returns>
        [HttpGet("{id}/owner-reservations")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<ReservationResponse>>> GetAllReservationByOwnerId(Guid id)
        {
            try
            {
                var staffList = await _reservationService.GetAllByOwnerId(id);

                if (staffList == null || !staffList.Any())
                {
                    return NotFound(new { message = "No reservation found for this user." });
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
        /// Get all hotel verifications by assign manager id.
        /// </summary>
        [HttpGet("{id}/hotel-verifications")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<HotelDocumentResponse>>> GetAllHotelVerificationByAssignManagerId(Guid id)
        {
            try
            {
                var hotelVerification = await _hotelVerificationService.GetAllByAssignManagerId(id);

                if (hotelVerification == null || !hotelVerification.Any())
                {
                    return NotFound(new { message = "No hotel verifications found for this manager." });
                }

                return Ok(hotelVerification);
            }
            catch (Exception ex)
            {
                // Log the exception if you have logging set up
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An unexpected error occurred.", details = ex.Message });
            }
        }

        /// <summary>
        /// Get all orders by staff ID.
        /// </summary>
        [HttpGet("{staffId}/staff-orders")]

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<OrderResponse>>> GetAllOrderByStaffId(Guid staffId)
        {
            try
            {
                var orderList = await _orderService.GetAllOrderByStaffId(staffId);

                if (orderList == null || !orderList.Any())
                {
                    return NotFound(new { message = "No orders found for this staff." });
                }

                return Ok(orderList);
            }
            catch (Exception ex)
            {
                // Log the exception if you have logging set up
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An unexpected error occurred.", details = ex.Message });
            }
        }

        /// <summary>
        /// Get all orders by customer ID.
        /// </summary>
        [HttpGet("{customerId}/orders")]

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<OrderResponse>>> GetAllOrderByCustomerId(Guid customerId)
        {
            try
            {
                var orderList = await _orderService.GetAllOrderByCustomerId(customerId);

                if (orderList == null || !orderList.Any())
                {
                    return NotFound(new { message = "No orders found for this staff." });
                }

                return Ok(orderList);
            }
            catch (Exception ex)
            {
                // Log the exception if you have logging set up
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An unexpected error occurred.", details = ex.Message });
            }
        }
    }
}
