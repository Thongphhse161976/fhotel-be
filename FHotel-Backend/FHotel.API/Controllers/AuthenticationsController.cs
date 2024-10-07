using FHotel.Repository.Models;
using FHotel.Service.Profiles;
using FHotel.Services.DTOs.Users;
using FHotel.Services.Services.Interfaces;
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
        public async Task<ActionResult<UserResponse>> LoginMember(LoginMem loginMem)
        {
            UserResponse userResponse = await _userService.Login(loginMem);
            if(userResponse == null)
            {
                return NotFound(
                        new ApiResponse
                        {
                            Success = false,
                            Message = "Invalid Username or Password"
                        }
                 );
            }
            else
            {
                // Check if the user is active
                if (userResponse.IsActive != true)
                {
                    // Handle the case where user is not active, e.g., log an error or return a meaningful response
                    return NotFound(
                        new ApiResponse
                        {
                            Success = false,
                            Message = "Your account is banned"
                        });
                }
                else
                {
                    return Ok(
                              new ApiResponse
                              {
                                  Success = true,
                                  Message = "Authenticate Successfully",
                                  Data = GenerateToken(userResponse)
                              }
                        );
                }
                
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
                    new Claim(ClaimTypes.Role, model.RoleId.ToString()),

                    //roles
                    new Claim("Token Id", Guid.NewGuid().ToString()),
                }),
                Expires = DateTime.MaxValue,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKeyBytes)
                        , SecurityAlgorithms.HmacSha256Signature)
            };

            var token = jwtTokenHandler.CreateToken(tokenDescription);

            return jwtTokenHandler.WriteToken(token);
        }

    }
}
