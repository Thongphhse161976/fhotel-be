using FHotel.Services.DTOs.Cities;
using FHotel.Services.DTOs.Countries;
using FHotel.Services.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FHotel.API.Controllers
{
    /// <summary>
    /// Controller for managing country.
    /// </summary>
    [Route("api/countries")]
    [ApiController]
    public class CountriesController : ControllerBase
    {
        private readonly ICountryService _countryService;

        public CountriesController(ICountryService countryService)
        {
            _countryService = countryService;
        }

        /// <summary>
        /// Get a list of all countries.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<CountryResponse>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<List<CountryResponse>>> GetAll()
        {
            try
            {
                var rs = await _countryService.GetAll();
                return Ok(rs);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Get country by country id.
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CountryResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<CountryResponse>> Get(Guid id)
        {
            try
            {
                var rs = await _countryService.Get(id);
                return Ok(rs);
            }
            catch
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Create new country.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CountryResponse>> Create([FromBody] CountryRequest request)
        {
            try
            {
                var result = await _countryService.Create(request);
                return CreatedAtAction(nameof(Create), result);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Delete country by country id.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult<CountryResponse>> Delete(Guid id)
        {
            var rs = await _countryService.Delete(id);
            return Ok(rs);
        }

        /// <summary>
        /// Update country by country id.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<CountryResponse>> Update(Guid id, [FromBody] CountryRequest request)
        {
            try
            {
                var rs = await _countryService.Update(id, request);
                return Ok(rs);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
