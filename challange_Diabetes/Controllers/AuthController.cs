using challange_Diabetes.DTO;
using challange_Diabetes.Services;
using challenge_Diabetes.DTO;
using challenge_Diabetes.Migrations;
using challenge_Diabetes.Model;
using challenge_Diabetes.Services;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using NuGet.Protocol;
using System.Security.Claims;

namespace challenge_Diabetes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        private readonly ITokenBlacklistService _tokenBlacklistService;
        private readonly UserManager<ApplicationUser> _userManager;


        public AuthController(IAuthService authService, ITokenBlacklistService tokenBlacklistService, UserManager<ApplicationUser> userManager)
        {
            _authService = authService;
            _tokenBlacklistService = tokenBlacklistService;
            _userManager = userManager;
        }
        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync([FromForm] RegisterModelDTO model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.RegisterAsync(model);

            if (!result.IsAuthenticated)
                return BadRequest(result.Message);

            return Ok(result);
        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login(TokenRequestModelDTO model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.Login(model);

            if (!result.IsAuthenticated)
                return BadRequest(result.Message);

            return Ok(result);
        }

        [HttpPost("Test")]
        public async Task<IActionResult> Tset()
        {
            var userid = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;

            return Ok(userid);
        }



        [HttpPost("addrole")]
        public async Task<IActionResult> AddRoleAsync(AddRoleModelDTO model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.AddRoleAsync(model);

            if (!string.IsNullOrEmpty(result))
                return BadRequest(result);

            return Ok(model);
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");



            if (string.IsNullOrEmpty(token))
            {
                return BadRequest("Invalid token.");
            }
            var remove = HttpContext.Request.Headers["Authorization"] = "";
            if (string.IsNullOrEmpty(remove))
            {
                return Ok();
            }
            return Ok("Logged out successfully.");

            /* _tokenBlacklistService.RemoveToken(token);
             return Ok("Token removed successfully.");

             _tokenBlacklistService.BlacklistToken(token);
             return Ok("Logged out successfully.");
            */

        }
        [HttpGet("Get User Details")]
        public async Task<IActionResult> GetUserById()
        {
            var userid = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            var user = await _userManager.FindByIdAsync(userid);
            if (user == null)
            {
                return NotFound(new { Message = "User not found" });

            }
            var userData = new UserDTO
            {
                Username = user.UserName,
                Email = user.Email,
                Phone = user.PhoneNumber,
                PhotoUrl = user.Photo
            };
            return Ok(userData);

        }
        [HttpPut("UpdateUser")]
        public async Task<IActionResult> UpdateUser(UpdateUserDTO modelDTO)
        {
            var userid = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            var user = await _userManager.FindByIdAsync(userid);
            if (user == null)
            {
                return NotFound(new { Message = "User not found" });

            }
            if (!string.IsNullOrEmpty(modelDTO.Email))
            {
                user.Email = modelDTO.Email;
            }
            if (!string.IsNullOrEmpty(modelDTO.UserName))
            {
                user.UserName = modelDTO.UserName;
            }
            if (!string.IsNullOrEmpty(modelDTO.Phone))
            {
                user.PhoneNumber = modelDTO.Phone;
            }
            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                var errors = string.Empty;
                foreach (var error in result.Errors)
                {
                    errors += $"{error.Description}, ";
                }
                return BadRequest(new { Message = errors });
            }

            return Ok(new { Message = "User updated successfully!", Date = modelDTO });
        }

        [HttpGet("Get user by name")]
        public async Task<UserDTO> FindUserByUsernameAsync(string username)
        {
            var user = await _userManager.FindByNameAsync(username);

            if (user == null)
            {
                return null;
            }

            var roles = await _userManager.GetRolesAsync(user);

            return new UserDTO
            {
                Email = user.Email,
                Username = user.UserName,
                PhotoUrl = user.Photo,
                Phone = user.PhoneNumber,
                
            };

        }
    }






    }


 

