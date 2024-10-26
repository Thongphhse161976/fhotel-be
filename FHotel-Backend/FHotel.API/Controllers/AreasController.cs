using FHotel.Service.DTOs.Areas;
using FHotel.Service.DTOs.Districts;
using FHotel.Service.Services.Interfaces;
using FHotel.Services.DTOs.Cities;
using FHotel.Services.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FHotel.API.Controllers
{
    /// <summary>
    /// Controller for managing area.
    /// </summary>
    [Route("api/areas")]
    [ApiController]
    public class AreasController : ControllerBase
    {
        private readonly IAreaService _areaService;
        private readonly IDistrictService _districtService;

        public AreasController(IAreaService areaService, IDistrictService districtService)
        {
            _areaService = areaService;
            _districtService = districtService;
        }

        /// <summary>
        /// Get a list of all areas.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<AreaResponse>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<List<AreaResponse>>> GetAll()
        {
            try
            {
                var rs = await _areaService.GetAll();
                return Ok(rs);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Get area by area id.
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AreaResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<AreaResponse>> Get(Guid id)
        {
            try
            {
                var rs = await _areaService.Get(id);
                return Ok(rs);
            }
            catch
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Create new area.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<AreaResponse>> Create([FromBody] AreaRequest request)
        {
            try
            {
                var result = await _areaService.Create(request);
                return CreatedAtAction(nameof(Create), result);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Delete area by area id.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult<AreaResponse>> Delete(Guid id)
        {
            var rs = await _areaService.Delete(id);
            return Ok(rs);
        }

        /// <summary>
        /// Update area by area id.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<AreaResponse>> Update(Guid id, [FromBody] AreaRequest request)
        {
            try
            {
                var rs = await _areaService.Update(id, request);
                return Ok(rs);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
