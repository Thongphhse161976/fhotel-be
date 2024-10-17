using FHotel.Service.DTOs.Reservations;
using FHotel.Services.DTOs.Reservations;
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
        private readonly IReservationDetailService _reservationdetailService;

        public ReservationsController(IReservationService reservationService, IReservationDetailService reservationDetail)
        {
            _reservationService = reservationService;
            _reservationdetailService = reservationDetail;
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
        public async Task<ActionResult<ReservationResponse>> Create([FromBody] CombinedReservationRequest combinedRequest)
        {
            try
            {
                var reservation = await _reservationService.Create(combinedRequest.Reservation);
                if (reservation != null)
                {
                    combinedRequest.Detail.ReservationId = reservation.ReservationId;
                    var reservationDetail = await _reservationdetailService.Create(combinedRequest.Detail);

                    var combinedResponse = new CombinedReservationResponse
                    {
                        Reservation = reservation,
                        ReservationDetail = reservationDetail
                    };

                    
                    return CreatedAtAction(nameof(Create), new { combinedResponse });
                }

                return BadRequest("Reservation creation failed.");
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

        /// <summary>
        /// Get a list of all reservation-details by reservation id.
        /// </summary>
        [HttpGet("{id}/reservation-details")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ReservationResponse>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<List<ReservationResponse>>> GetAllReservationDetailByReservationId(Guid id)
        {
            try
            {
                var rs = await _reservationdetailService.GetAllByReservationId(id);
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
    }
}
