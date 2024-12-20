﻿using FHotel.Repository.Models;
using FHotel.Service.DTOs.Bills;
using FHotel.Service.DTOs.Reservations;
using FHotel.Service.DTOs.RoomStayHistories;
using FHotel.Service.DTOs.RoomTypes;
using FHotel.Service.DTOs.VnPayConfigs;
using FHotel.Service.Services.Interfaces;
using FHotel.Services.DTOs.Feedbacks;
using FHotel.Services.DTOs.HotelDocuments;
using FHotel.Services.DTOs.Hotels;
using FHotel.Services.DTOs.OrderDetails;
using FHotel.Services.DTOs.Orders;
using FHotel.Services.DTOs.Reservations;
using FHotel.Services.DTOs.RoomTypes;
using FHotel.Services.DTOs.UserDocuments;
using FHotel.Services.Services.Implementations;
using FHotel.Services.Services.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FHotel.API.Controllers
{
    /// <summary>
    /// Controller for managing reservation.
    /// </summary>
    [Route("api/reservations")]
    [ApiController]
    public class ReservationsController : ControllerBase
    {
        private readonly IReservationService _reservationService;
        private readonly IOrderService _orderService;
        private readonly IUserDocumentService _userDocumentService;
        private readonly IUserService _userService;
        private readonly IRoomStayHistoryService _roomStayHistoryService;
        private readonly IOrderDetailService _orderDetailService;
        private readonly IFeedbackService _feedbackService;
        private readonly IBillService _billService;
        private readonly IRoomTypeService _roomTypeService;
        private readonly IHotelService _hotelService;

        public ReservationsController(IReservationService reservationService, IOrderService orderService,
            IUserDocumentService userDocumentService, IUserService userService, 
            IRoomStayHistoryService roomStayHistoryService, IOrderDetailService orderDetailService, IFeedbackService feedbackService
            , IBillService billService, IRoomTypeService roomTypeService, IHotelService hotelService)
        {
            _reservationService = reservationService;
            _orderService = orderService;
            _userDocumentService = userDocumentService;
            _userService = userService;
            _roomStayHistoryService = roomStayHistoryService;
            _orderDetailService = orderDetailService;
            _feedbackService = feedbackService;
            _billService = billService;
            _roomTypeService = roomTypeService;
            _hotelService = hotelService;
        }

        /// <summary>
        /// Get a list of all reservations.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ReservationResponse>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<List<ReservationResponse>>> GetAll()
        {
            try
            {
                var rs = await _reservationService.GetAll();
                return Ok(rs);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Get reservation by reservation id.
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ReservationResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<ReservationResponse>> Get(Guid id)
        {
            try
            {
                var rs = await _reservationService.Get(id);
                return Ok(rs);
            }
            catch
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Create new reservation.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ReservationResponse>> Create([FromBody] ReservationCreateRequest request)
        {
            try
            {
                var result = await _reservationService.Create(request);
                return CreatedAtAction(nameof(Create), result);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Delete reservation by reservation id.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult<ReservationResponse>> Delete(Guid id)
        {
            var rs = await _reservationService.Delete(id);
            return Ok(rs);
        }

        /// <summary>
        /// Update reservation by reservation id.
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ReservationResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<ReservationResponse>> Update(Guid id, [FromBody] ReservationUpdateRequest request)
        {
            try
            {
                var rs = await _reservationService.Update(id, request);
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
        /// Calculate total reservation amount.
        /// </summary>
        [HttpPost("calculate")]
        public async Task<IActionResult> CalculateTotalAmount([FromBody] CalculateTotalAmountRequest request)
        {
            try
            {
                var result = await _reservationService.CalculateTotalAmount(
                    request.RoomTypeId,
                    request.CheckInDate,
                    request.CheckOutDate,
                    request.NumberOfRooms);

                return Ok(result); // This will return both TotalAmount and PriceBreakdown
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Get a list of all orders by reservation id.
        /// </summary>
        [HttpGet("{id}/orders")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<OrderResponse>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<List<OrderResponse>>> GetAllOrderByReservationId(Guid id)
        {
            try
            {
                var rs = await _orderService.GetAllByReservationId(id);
                return Ok(rs);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Get a list of all order detail by reservation id.
        /// </summary>
        [HttpGet("{id}/order-details")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<OrderDetailResponse>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<List<OrderDetailResponse>>> GetAllOrderDetailByReservationId(Guid id)
        {
            try
            {
                var rs = await _orderDetailService.GetAllOrderDetailByReservation(id);
                return Ok(rs);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Get all user documents by reservation id.
        /// </summary>
        [HttpGet("{reservationId}/user-documents")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<UserDocumentResponse>>> GetAllUserDocumentByReservation(Guid reservationId)
        {
            try
            {
                var userDocuments = await _userDocumentService.GetAllUserDocumentByReservationId(reservationId);

                if (userDocuments == null || !userDocuments.Any())
                {
                    return NotFound(new { message = "No user documents found for this reservation." });
                }

                return Ok(userDocuments);
            }
            catch (Exception ex)
            {
                // Log the exception if you have logging set up
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An unexpected error occurred.", details = ex.Message });
            }
        }
        /// <summary>
        /// Get all room stay histories by reservation id.
        /// </summary>
        [HttpGet("{reservationId}/room-stay-histories")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<RoomStayHistoryResponse>>> GetAllRoomStayHistoryByReservation(Guid reservationId)
        {
            try
            {
                var roomStayHistories = await _roomStayHistoryService.GetAllByReservationId(reservationId);

                if (roomStayHistories == null || !roomStayHistories.Any())
                {
                    return NotFound(new { message = "No Room stay history found for this reservation." });
                }

                return Ok(roomStayHistories);
            }
            catch (Exception ex)
            {
                // Log the exception if you have logging set up
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An unexpected error occurred.", details = ex.Message });
            }
        }

        /// <summary>
        /// Get a list of all feedback by reservation id.
        /// </summary>
        [HttpGet("{id}/feedbacks")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<FeedbackResponse>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<List<FeedbackResponse>>> GetAllFeedbackByReservationId(Guid id)
        {
            try
            {
                var rs = await _feedbackService.GetAllFeedbackByReservationId(id);
                return Ok(rs);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Get bill by reservation id.
        /// </summary>
        [HttpGet("{id}/bills")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BillResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<BillResponse>> GetBillByReservation(Guid id)
        {
            try
            {
                var rs = await _billService.GetBillByReservation(id);
                return Ok(rs);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Pay through VnPay by reservation id.
        /// </summary>
        [HttpPost("{id}/pay")]
        public async Task<ActionResult<string>> Pay(Guid id)
        {
            try
            {
                var paymentUrl = await _reservationService.Pay(id, HttpContext);
                if (paymentUrl == null)
                {
                    return NotFound("Reservation not found.");
                }

                return Ok(paymentUrl);
            }
            catch (Exception ex)
            {
                // Log the exception if needed
                return StatusCode(500, ex.Message);
            }
        }

        ///// <summary>
        ///// Check available rooms.
        ///// </summary>
        //[HttpGet("api/roomtypes/{roomTypeId}/available-on-date")]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //public async Task<IActionResult> GetAvailableRoomsOnDate(Guid roomTypeId, DateTime checkinDate, DateTime checkoutDate)
        //{
        //    var availableRooms = await _roomTypeService.CountAvailableRoomsInRangeAsync(roomTypeId,checkinDate, checkoutDate);
        //    return Ok(new { AvailableRooms = availableRooms });
        //}

        //[HttpGet("hotels/available-on-date")]
        //[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<HotelResponse>))]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //public async Task<ActionResult<List<HotelResponse>>> SearchAvailableRoomsOnDate([FromQuery]DateTime checkinDate, [FromQuery] DateTime checkoutDate)
        //{
        //    var availableHotels = await _hotelService.GetHotelsWithAvailableRoomTypesInRangeAsync(checkinDate, checkoutDate);
        //    return Ok(availableHotels);
        //}

        /// <summary>
        /// Search reservation for staff.
        /// </summary>
        [HttpPost("search")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ReservationResponse>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<ReservationResponse>>> SearchReservations([FromBody] SearchRequest request)
        {
            try
            {
                // Validate the input
                if (request == null || request.UserId == Guid.Empty || string.IsNullOrWhiteSpace(request.Query))
                {
                    return BadRequest("Invalid input. Please provide valid customerId and query.");
                }

                // Call the service to search with multiple room types and quantities
                var result = await _reservationService.SearchReservations(request.UserId, request.Query);

                if (result == null || !result.Any())
                {
                    return NotFound("No reservation matching the search criteria.");
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest($"An error occurred: {ex.Message}");
            }
        }

        /// <summary>
        /// Search reservation for customer.
        /// </summary>
        [HttpPost("search-customer")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ReservationResponse>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<ReservationResponse>>> SearchReservationsCustomer([FromBody] SearchRequest request)
        {
            try
            {
                // Validate the input
                if (request == null || request.UserId == Guid.Empty || string.IsNullOrWhiteSpace(request.Query))
                {
                    return BadRequest("Invalid input. Please provide valid customerId and query.");
                }

                // Call the service to search with multiple room types and quantities
                var result = await _reservationService.SearchReservationsCustomer(request.UserId, request.Query);

                if (result == null || !result.Any())
                {
                    return NotFound("No reservation matching the search criteria.");
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest($"An error occurred: {ex.Message}");
            }
        }


        /// <summary>
        /// Create refund request.
        /// </summary>
        [HttpPost("refund")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<string>> CreateRefundRequest([FromQuery] Guid id)
        {
            try
            {
                var result = await _reservationService.Refund(id);
                return CreatedAtAction(nameof(Create), result);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }

    public class CalculateTotalAmountRequest
    {
        public Guid RoomTypeId { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public int NumberOfRooms { get; set; }
    }

   




}
