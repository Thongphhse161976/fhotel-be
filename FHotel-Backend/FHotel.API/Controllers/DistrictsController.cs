using FHotel.Service.DTOs.Districts;
using FHotel.Service.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FHotel.API.Controllers
{
    /// <summary>
    /// Controller for managing district.
    /// </summary>
    [Route("api/districts")]
    [ApiController]
    public class DistrictsController : ControllerBase
    {
        private readonly IDistrictService _districtService;

        public DistrictsController(IDistrictService districtService)
        {
            _districtService = districtService;
        }

        /// <summary>
        /// Get a list of all districts.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<DistrictResponse>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<List<DistrictResponse>>> GetAll()
        {
            try
            {
                var rs = await _districtService.GetAll();
                return Ok(rs);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Get district by district id.
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DistrictResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<DistrictResponse>> Get(Guid id)
        {
            try
            {
                var rs = await _districtService.Get(id);
                return Ok(rs);
            }
            catch
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Create new district.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<DistrictResponse>> Create([FromBody] DistrictRequest request)
        {
            try
            {
                var result = await _districtService.Create(request);
                return CreatedAtAction(nameof(Create), result);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Delete district by district id.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult<DistrictResponse>> Delete(Guid id)
        {
            var rs = await _districtService.Delete(id);
            return Ok(rs);
        }

        /// <summary>
        /// Update district by district id.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<DistrictResponse>> Update(Guid id, [FromBody] DistrictRequest request)
        {
            try
            {
                var rs = await _districtService.Update(id, request);
                return Ok(rs);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
