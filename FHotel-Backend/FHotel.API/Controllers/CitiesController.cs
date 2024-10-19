using FHotel.Service.DTOs.Districts;
using FHotel.Service.Services.Interfaces;
using FHotel.Services.DTOs.Cities;
using FHotel.Services.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FHotel.API.Controllers
{
    /// <summary>
    /// Controller for managing city.
    /// </summary>
    [Route("api/cities")]
    [ApiController]
    public class CitiesController : ControllerBase
    {
        private readonly ICityService _cityService;
        private readonly IDistrictService _districtService;

        public CitiesController(ICityService cityService, IDistrictService districtService)
        {
            _cityService = cityService;
            _districtService = districtService;
        }

        /// <summary>
        /// Get a list of all cities.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<CityResponse>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<List<CityResponse>>> GetAll()
        {
            try
            {
                var rs = await _cityService.GetAll();
                return Ok(rs);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Get city by city id.
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CityResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<CityResponse>> Get(Guid id)
        {
            try
            {
                var rs = await _cityService.Get(id);
                return Ok(rs);
            }
            catch
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Create new city.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CityResponse>> Create([FromBody] CityRequest request)
        {
            try
            {
                var result = await _cityService.Create(request);
                return CreatedAtAction(nameof(Create), result);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Delete city by city id.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult<CityResponse>> Delete(Guid id)
        {
            var rs = await _cityService.Delete(id);
            return Ok(rs);
        }

        /// <summary>
        /// Update city by city id.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<CityResponse>> Update(Guid id, [FromBody] CityRequest request)
        {
            try
            {
                var rs = await _cityService.Update(id, request);
                return Ok(rs);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Get a list of all districts by city id.
        /// </summary>
        [HttpGet("{id}/districts")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<DistrictResponse>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<List<DistrictResponse>>> GetAllDistrictByCityId(Guid id)
        {
            try
            {
                var rs = await _districtService.GetAllByCityId(id);
                return Ok(rs);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
