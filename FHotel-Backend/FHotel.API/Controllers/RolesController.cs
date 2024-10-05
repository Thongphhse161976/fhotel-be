using FHotel.Services.DTOs.Roles;
using FHotel.Services.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FHotel.API.Controllers
{
    /// <summary>
    /// Controller for managing role.
    /// </summary>
    [Route("api/roles")]
    [ApiController]
    /// <summary>
    /// Controller for managing roles.
    /// </summary>
    public class RolesController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RolesController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        /// <summary>
        /// Get a list of all roles.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<RoleResponse>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<List<RoleResponse>>> GetAll()
        {
            try
            {
                var rs = await _roleService.GetAll();
                return Ok(rs);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Get role by role id.
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RoleResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<RoleResponse>> Get(Guid id)
        {
            try
            {
                var rs = await _roleService.Get(id);
                return Ok(rs);
            }
            catch
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Create new role.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<RoleResponse>> Create([FromBody] RoleRequest request)
        {
            try
            {
                var result = await _roleService.Create(request);
                return CreatedAtAction(nameof(Create), result);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Delete role by role id.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult<RoleResponse>> Delete(Guid id)
        {
            var rs = await _roleService.Delete(id);
            return Ok(rs);
        }

        /// <summary>
        /// Update role by role id.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<RoleResponse>> Update(Guid id, [FromBody] RoleRequest request)
        {
            try
            {
                var rs = await _roleService.Update(id, request);
                return Ok(rs);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
