using FHotel.Services.DTOs.Cities;
using FHotel.Services.DTOs.ReservationDetails;
using FHotel.Services.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FHotel.API.Controllers
{
    [Route("api/reservation-details")]
    [ApiController]
    public class ReservationDetailsController : ControllerBase
    {
        private readonly IReservationDetailService _reservationDetailService;

        public ReservationDetailsController(IReservationDetailService reservationDetailService)
        {
            _reservationDetailService = reservationDetailService;
        }

        /// <summary>
        /// Get a list of all reservationDetails.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ReservationDetailResponse>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<List<ReservationDetailResponse>>> GetAll()
        {
            try
            {
                var rs = await _reservationDetailService.GetAll();
                return Ok(rs);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Get reservationDetail by reservationDetail id.
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ReservationDetailResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<ReservationDetailResponse>> Get(Guid id)
        {
            try
            {
                var rs = await _reservationDetailService.Get(id);
                return Ok(rs);
            }
            catch
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Create new reservationDetail.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ReservationDetailResponse>> Create([FromBody] ReservationDetailRequest request)
        {
            try
            {
                var result = await _reservationDetailService.Create(request);
                return CreatedAtAction(nameof(Create), result);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Delete reservationDetail by reservationDetail id.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult<ReservationDetailResponse>> Delete(Guid id)
        {
            var rs = await _reservationDetailService.Delete(id);
            return Ok(rs);
        }

        /// <summary>
        /// Update reservationDetail by reservationDetail id.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<ReservationDetailResponse>> Update(Guid id, [FromBody] ReservationDetailRequest request)
        {
            try
            {
                var rs = await _reservationDetailService.Update(id, request);
                return Ok(rs);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
