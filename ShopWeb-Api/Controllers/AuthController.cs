using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ShopWeb_Api.Models;
using ShopWeb_Api.Models.DTO.User;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ShopWeb_Api.Services.Interfaces;

namespace ShopWeb_Api.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO registerDto)
        {
            var user = await _authService.RegisterAsync(registerDto);
            if(user == null)
                return Conflict("Пользователь с таким логином уже существует");
            return Ok(new { user.Id, user.Login });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDto)
        {
            var token = await _authService.LoginAsync(loginDto);
            if(token == null)
                return Conflict("Неправильно введен логин или пароль");
            return Ok(new { Token = token });
        }
    }
}
