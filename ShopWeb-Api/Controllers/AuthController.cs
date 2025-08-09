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
    /// <summary>
    /// Контроллер для аутентификации и регистрации пользователей
    /// </summary>
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        /// <summary>
        /// Регистрация нового пользователя
        /// </summary>
        /// <param name="registerDto">Данные для регистрации</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns>Данные зарегистрированного пользователя</returns>
        /// <response code="200">Пользователь успешно зарегистрирован</response>
        /// <response code="400">Некорректные данные</response>
        /// <response code="409">Пользователь с таким логином уже существует</response>
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO registerDto, CancellationToken cancellationToken)
        {
            var user = await _authService.RegisterAsync(registerDto, cancellationToken);
            if(user == null)
                return Conflict("Пользователь с таким логином уже существует");
            return Ok(new { user.Id, user.Login });
        }


        /// <summary>
        /// Аутентификация пользователя
        /// </summary>
        /// <param name="loginDto">Данные для входа</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns>JWT токен и данные пользователя</returns>
        /// <response code="200">Успешная аутентификация</response>
        /// <response code="400">Некорректные данные</response>
        /// <response code="401">Неверные учетные данные</response>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDto, CancellationToken cancellationToken)
        {
            var token = await _authService.LoginAsync(loginDto, cancellationToken);
            if(token == null)
                return Conflict("Неправильно введен логин или пароль");
            return Ok(new { Token = token });
        }
    }
}
