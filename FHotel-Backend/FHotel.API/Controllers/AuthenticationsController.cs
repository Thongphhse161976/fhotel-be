using FHotel.Repository.Models;
using FHotel.Service.DTOs.Users;
using FHotel.Service.Profiles;
using FHotel.Services.DTOs.Users;
using FHotel.Services.Services.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Mime;
using System.Security.Claims;
using System.Text;

namespace FHotel.API.Controllers
{
    /// <summary>
    /// Controller for managing authentication.
    /// </summary>
    [Route("api/authentications")]
    [ApiController]
    public class AuthenticationsController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;
        private readonly AppSettings _appSettings;

        public AuthenticationsController(IUserService userService, IConfiguration configuration, IOptions<AppSettings> appSettings)
        {
            _userService = userService;
            _configuration = configuration;
            _appSettings = appSettings.Value;
        }

        /// <summary>
        /// Login an account by email and password.
        /// </summary>
        [HttpPost("login")]
        [Consumes(MediaTypeNames.Application.Json)]
        public async Task<ActionResult<UserResponse>> LoginMember(UserLoginRequest loginMem)
        {
            try
            {
                // Call the login service to authenticate the user
                var userResponse = await _userService.Login(loginMem);

                if (userResponse == null)
                {
                    return NotFound(new ApiResponse
                    {
                        Success = false,
                        Message = "Invalid Username or Password"
                    });
                }

                // Check if the user is active
                if (userResponse.IsActive != true)
                {
                    return NotFound(new ApiResponse
                    {
                        Success = false,
                        Message = "Your account is not allowed to log in!"
                    });
                }

                // Authentication successful, generate token and return response
                return Ok(new ApiResponse
                {
                    Success = true,
                    Message = "Authenticated Successfully",
                    Data = GenerateToken(userResponse)
                });
            }
            catch (ValidationException ex)
            {
                // Handle validation exception and return validation errors
                return BadRequest(new
                {
                    message = "Validation failed",
                    errors = ex.Errors.Select(e => e.ErrorMessage)
                });
            }
            catch (Exception ex)
            {
                // Handle generic exception and return a 500 status code with error message
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    message = ex.Message
                });
            }
        }




        //Generate token
        private string GenerateToken(UserResponse model)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();

            var secretKeyBytes = Encoding.UTF8.GetBytes(_appSettings.SecretKey);

            var tokenDescription = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] {
                    new Claim("Id", model.UserId.ToString()),
                    new Claim(ClaimTypes.Role, model.Role.RoleName.ToString()),
                }),
                Expires = DateTime.MaxValue,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKeyBytes)
                        , SecurityAlgorithms.HmacSha256Signature)
            };

            var token = jwtTokenHandler.CreateToken(tokenDescription);

            return jwtTokenHandler.WriteToken(token);
        }

        /// <summary>
        /// Customer register.
        /// </summary>
        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<UserResponse>> Register([FromBody] UserCreateRequest request)
        {
            try
            {
                var result = await _userService.Register(request);
                return CreatedAtAction(nameof(Register), result);
            }
            catch (ValidationException ex)
            {
                // Access validation errors from ex.Errors
                return BadRequest(new
                {
                    message = "Validation failed",
                    errors = ex.Errors.Select(e => e.ErrorMessage).ToList()
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }

        /// <summary>
        /// Activate user.
        /// </summary>
        [HttpGet("activate")]
        public async Task<IActionResult> ActivateUser(string email)
        {
            try
            {
                // Call the service method to activate the user
                await _userService.ActivateUser(email);
                return Redirect("https://fhotel-web.web.app/success-register");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }


    }
}
