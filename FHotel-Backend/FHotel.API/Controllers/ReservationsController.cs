using FHotel.Repository.Models;
using FHotel.Service.DTOs.Bills;
using FHotel.Service.DTOs.Reservations;
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

        public ReservationsController(IReservationService reservationService, IOrderService orderService, IUserDocumentService userDocumentService)
        {
            _reservationService = reservationService;
            _orderService = orderService;
            _userDocumentService = userDocumentService;
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

        // POST: api/reservations/calculate
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
    }

    public class CalculateTotalAmountRequest
    {
        public Guid RoomTypeId { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public int NumberOfRooms { get; set; }
    }



    
}
