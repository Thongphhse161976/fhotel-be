using FHotel.Service.DTOs.Types;
using FHotel.Service.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FHotel.API.Controllers
{
    /// <summary>
    /// Controller for managing type.
    /// </summary>
    [Route("api/types")]
    [ApiController]
    public class TypesController : ControllerBase
    {
        private readonly ITypeService _typeService;

        public TypesController(ITypeService typeService)
        {
            _typeService = typeService;
        }

        /// <summary>
        /// Get a list of all types.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<TypeResponse>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<List<TypeResponse>>> GetAll()
        {
            try
            {
                var rs = await _typeService.GetAll();
                return Ok(rs);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Get type by type id.
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TypeResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<TypeResponse>> Get(Guid id)
        {
            try
            {
                var rs = await _typeService.Get(id);
                return Ok(rs);
            }
            catch
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Create new type.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<TypeResponse>> Create([FromBody] TypeCreateRequest request)
        {
            try
            {
                var result = await _typeService.Create(request);
                return CreatedAtAction(nameof(Create), result);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Delete type by type id.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult<TypeResponse>> Delete(Guid id)
        {
            var rs = await _typeService.Delete(id);
            return Ok(rs);
        }

        /// <summary>
        /// Update type by type id.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<TypeResponse>> Update(Guid id, [FromBody] TypeUpdateRequest request)
        {
            try
            {
                var rs = await _typeService.Update(id, request);
                return Ok(rs);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
