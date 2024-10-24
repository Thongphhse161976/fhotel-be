﻿using FHotel.API.VnPay;
using FHotel.Repository.Models;
using FHotel.Service.DTOs.Bills;
using FHotel.Service.DTOs.Reservations;
using FHotel.Service.DTOs.RoomStayHistories;
using FHotel.Service.DTOs.VnPayConfigs;
using FHotel.Service.Services.Interfaces;
using FHotel.Services.DTOs.HotelDocuments;
using FHotel.Services.DTOs.Orders;
using FHotel.Services.DTOs.Reservations;
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
        private readonly IVnPayService _vnPayService;
        private readonly IUserService _userService;
        private readonly IRoomStayHistoryService _roomStayHistoryService;

        public ReservationsController(IReservationService reservationService, IOrderService orderService,
            IUserDocumentService userDocumentService, IVnPayService vnPayService, IUserService userService, IRoomStayHistoryService roomStayHistoryService)
        {
            _reservationService = reservationService;
            _orderService = orderService;
            _userDocumentService = userDocumentService;
            _vnPayService = vnPayService;
            _userService = userService;
            _roomStayHistoryService = roomStayHistoryService;
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
                var totalAmount = await _reservationService.CalculateTotalAmount(
                    request.RoomTypeId,
                    request.CheckInDate,
                    request.CheckOutDate,
                    request.NumberOfRooms);

                return Ok(new { TotalAmount = totalAmount });
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
        /// Pay through VnPay by reservation id.
        /// </summary>
        [HttpPost("{id}/pay")]
        public async Task<ActionResult<string>> Pay(Guid id)
        {
            // Set the UTC offset for UTC+7
            TimeSpan utcOffset = TimeSpan.FromHours(7);

            // Get the current UTC time
            DateTime utcNow = DateTime.UtcNow;

            // Convert the UTC time to UTC+7
            DateTime localTime = utcNow + utcOffset;

            var reservation = await _reservationService.Get(id);
            if (reservation == null)
            {
                return NotFound();
            }
            else
            {
                var customer = await _userService.Get(reservation.CustomerId.Value);
                var vnPayModel = new VnPaymentRequestModel
                {
                    Amount = (int)(reservation.TotalAmount),
                    CreatedDate = localTime,
                    Description = "Payment-For-Reservation:",
                    FullName = customer.Name,
                    OrderId = id
                };
                return _vnPayService.CreatePaymentUrl(HttpContext, vnPayModel);
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
