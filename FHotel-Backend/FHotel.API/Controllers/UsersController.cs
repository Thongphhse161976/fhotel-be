using FHotel.Service.Profiles;
using FHotel.Services.DTOs.Cities;
using FHotel.Services.DTOs.Users;
using FHotel.Services.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace FHotel.API.Controllers
{
    /// <summary>
    /// Controller for managing user.
    /// </summary>
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Get a list of all users.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<UserResponse>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<List<UserResponse>>> GetAll()
        {
            try
            {
                var rs = await _userService.GetAll();
                if (rs.IsNullOrEmpty()) // check list is null or empty
                {
                    return NotFound(new ApiResponse
                    {
                        Success = false,
                        Message = "No users found."
                    });
                }
                return Ok(rs);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Get user by user id.
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<UserResponse>> Get(Guid id)
        {
            try
            {
                var rs = await _userService.Get(id);
                return Ok(rs);
            }
            catch
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Create new user.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<UserResponse>> Create([FromBody] UserRequest request)
        {
            try
            {
                var result = await _userService.Create(request);
                return CreatedAtAction(nameof(Create), result);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Delete user by user id.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult<UserResponse>> Delete(Guid id)
        {
            var rs = await _userService.Delete(id);
            return Ok(rs);
        }

        /// <summary>
        /// Update user by user id.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<UserResponse>> Update(Guid id, [FromBody] UserRequest request)
        {
            try
            {
                var rs = await _userService.Update(id, request);
                return Ok(rs);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
