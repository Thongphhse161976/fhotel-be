using FHotel.Services.DTOs.Cities;
using FHotel.Services.DTOs.Timetable;
using FHotel.Services.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FHotel.API.Controllers
{
    [Route("api/timetables")]
    [ApiController]
    public class TimetablesController : ControllerBase
    {
        private readonly ITimetableService _timetableService;

        public TimetablesController(ITimetableService timetableService)
        {
            _timetableService = timetableService;
        }

        /// <summary>
        /// Get a list of all timetables.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<TimetableResponse>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<List<TimetableResponse>>> GetAll()
        {
            try
            {
                var rs = await _timetableService.GetAll();
                return Ok(rs);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Get timetable by timetable id.
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TimetableResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<TimetableResponse>> Get(Guid id)
        {
            try
            {
                var rs = await _timetableService.Get(id);
                return Ok(rs);
            }
            catch
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Create new timetable.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<TimetableResponse>> Create([FromBody] TimetableRequest request)
        {
            try
            {
                var result = await _timetableService.Create(request);
                return CreatedAtAction(nameof(Create), result);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Delete timetable by timetable id.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult<TimetableResponse>> Delete(Guid id)
        {
            var rs = await _timetableService.Delete(id);
            return Ok(rs);
        }

        /// <summary>
        /// Update timetable by timetable id.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<TimetableResponse>> Update(Guid id, [FromBody] TimetableRequest request)
        {
            try
            {
                var rs = await _timetableService.Update(id, request);
                return Ok(rs);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
