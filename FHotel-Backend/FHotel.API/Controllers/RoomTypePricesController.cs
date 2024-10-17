using FHotel.Service.DTOs.RoomTypePrices;
using FHotel.Services.DTOs.Cities;
using FHotel.Services.DTOs.RoomTypePrices;
using FHotel.Services.Services.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FHotel.API.Controllers
{
    /// <summary>
    /// Controller for managing room-type-price.
    /// </summary>
    [Route("api/room-type-prices")]
    [ApiController]
    public class RoomTypePricesController : ControllerBase
    {
        private readonly IRoomTypePriceService _roomTypePriceService;

        public RoomTypePricesController(IRoomTypePriceService roomTypePriceService)
        {
            _roomTypePriceService = roomTypePriceService;
        }

        /// <summary>
        /// Get a list of all room-type-prices.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<RoomTypePriceResponse>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<List<RoomTypePriceResponse>>> GetAll()
        {
            try
            {
                var rs = await _roomTypePriceService.GetAll();
                return Ok(rs);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Get room-type-price by room-type-price id.
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RoomTypePriceResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<RoomTypePriceResponse>> Get(Guid id)
        {
            try
            {
                var rs = await _roomTypePriceService.Get(id);
                return Ok(rs);
            }
            catch
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Create new room-type-price.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<RoomTypePriceResponse>> Create([FromBody] RoomTypePriceCreateRequest request)
        {
            try
            {
                var result = await _roomTypePriceService.Create(request);
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
        /// Delete room-type-price by room-type-price id.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult<RoomTypePriceResponse>> Delete(Guid id)
        {
            var rs = await _roomTypePriceService.Delete(id);
            return Ok(rs);
        }

        /// <summary>
        /// Update room-type-price by room-type-price id.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<RoomTypePriceResponse>> Update(Guid id, [FromBody] RoomTypePriceUpdateRequest request)
        {
            try
            {
                var rs = await _roomTypePriceService.Update(id, request);
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
