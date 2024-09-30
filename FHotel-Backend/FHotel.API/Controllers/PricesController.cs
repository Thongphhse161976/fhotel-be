using FHotel.Services.DTOs.Cities;
using FHotel.Services.DTOs.Prices;
using FHotel.Services.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FHotel.API.Controllers
{
    [Route("api/prices")]
    [ApiController]
    public class PricesController : ControllerBase
    {
        private readonly IPriceService _priceService;

        public PricesController(IPriceService priceService)
        {
            _priceService = priceService;
        }

        /// <summary>
        /// Get a list of all prices.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<PriceResponse>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<List<PriceResponse>>> GetAll()
        {
            try
            {
                var rs = await _priceService.GetAll();
                return Ok(rs);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Get price by price id.
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PriceResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<PriceResponse>> Get(Guid id)
        {
            try
            {
                var rs = await _priceService.Get(id);
                return Ok(rs);
            }
            catch
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Create new price.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<PriceResponse>> Create([FromBody] PriceRequest request)
        {
            try
            {
                var result = await _priceService.Create(request);
                return CreatedAtAction(nameof(Create), result);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Delete price by price id.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult<PriceResponse>> Delete(Guid id)
        {
            var rs = await _priceService.Delete(id);
            return Ok(rs);
        }

        /// <summary>
        /// Update price by price id.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<PriceResponse>> Update(Guid id, [FromBody] PriceRequest request)
        {
            try
            {
                var rs = await _priceService.Update(id, request);
                return Ok(rs);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
