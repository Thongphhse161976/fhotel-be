using FHotel.Services.DTOs.Cities;
using FHotel.Services.DTOs.RoomTypePrices;
using FHotel.Services.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FHotel.API.Controllers
{
    [Route("api/roomTypePrices")]
    [ApiController]
    public class RoomTypePricesController : ControllerBase
    {
        private readonly IRoomTypePriceService _roomTypePriceService;

        public RoomTypePricesController(IRoomTypePriceService roomTypePriceService)
        {
            _roomTypePriceService = roomTypePriceService;
        }

        /// <summary>
        /// Get a list of all roomTypePrices.
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
        /// Get roomTypePrice by roomTypePrice id.
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
        /// Create new roomTypePrice.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<RoomTypePriceResponse>> Create([FromBody] RoomTypePriceRequest request)
        {
            try
            {
                var result = await _roomTypePriceService.Create(request);
                return CreatedAtAction(nameof(Create), result);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Delete roomTypePrice by roomTypePrice id.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult<RoomTypePriceResponse>> Delete(Guid id)
        {
            var rs = await _roomTypePriceService.Delete(id);
            return Ok(rs);
        }

        /// <summary>
        /// Update roomTypePrice by roomTypePrice id.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<RoomTypePriceResponse>> Update(Guid id, [FromBody] RoomTypePriceRequest request)
        {
            try
            {
                var rs = await _roomTypePriceService.Update(id, request);
                return Ok(rs);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
