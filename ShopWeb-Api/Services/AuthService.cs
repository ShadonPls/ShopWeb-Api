using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ShopWeb_Api.Models;
using ShopWeb_Api.Models.DTO.User;
using ShopWeb_Api.Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BCrypt.Net;

namespace ShopWeb_Api.Services
{
    /// <summary>
    /// Сервис для аутентификации и регистрации пользователей
    /// </summary>
    public class AuthService : IAuthService
    {
        private readonly Data.AppContext _context;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;
        private readonly ILogger<AuthService> _logger;

        /// <summary>
        /// Конструктор сервиса категорий
        /// </summary>
        /// <param name="context">Контекст базы данных</param>
        /// <param name="mapper">Автомаппер для преобразования DTO</param>
        /// <param name="logger">Логгер для записи событий</param>
        /// <param name="config">Конфиг</param>
        public AuthService(Data.AppContext context, IConfiguration config,
            IMapper mapper, ILogger<AuthService> logger)
        {
            _context = context;
            _config = config;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Регистрация нового пользователя
        /// </summary>
        /// <param name="registerDto">Данные для регистрации</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns>Данные зарегистрированного пользователя или null при ошибке</returns>
        public async Task<UserResponseDTO> RegisterAsync(RegisterDTO registerDto, CancellationToken cancellationToken = default)
        {
            if (await _context.Users.AnyAsync(u => u.Login == registerDto.Login, cancellationToken))
            {
                _logger.LogWarning($"Пользователь с логином {registerDto.Login} уже существует");
                return null;
            }

            var user = new User
            {
                Login = registerDto.Login,
                Password = BCrypt.Net.BCrypt.HashPassword(registerDto.Password),
                Email = registerDto.Email,
                RegistrationDate = DateTime.UtcNow
            };

            var userRole = await _context.Roles.FirstOrDefaultAsync(r => r.Name == "User", cancellationToken);

            user.Roles = new List<RoleUser> { new RoleUser { RoleId = userRole.Id } };
            _context.Users.Add(user);

            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation($"Зарегистрирован новый пользователь: {user.Login} (ID: {user.Id})");
            return _mapper.Map<UserResponseDTO>(user);
        }

        /// <summary>
        /// Аутентификация пользователя
        /// </summary>
        /// <param name="loginDto">Данные для входа</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns>JWT токен или null при неудаче</returns>
        public async Task<string> LoginAsync(LoginDTO loginDto, CancellationToken cancellationToken = default)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Login == loginDto.Login, cancellationToken);

            if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.Password))
            {
                _logger.LogWarning($"Неудачная попытка входа для логина: {loginDto.Login}");
                return null;
            }
            var admin = await _context.RoleUsers.AnyAsync(x => x.Role.Name == "Admin" && x.User.Id == user.Id, cancellationToken);
            string role = "";
            if (admin)
                role = "Admin";
            else
                role = "User";
            var token = GenerateJwtToken(user, role);

            _logger.LogInformation($"Успешный вход пользователя: {user.Login}");
            return token;
        }

        /// <summary>
        /// Генерация JWT токена
        /// </summary>
        private string GenerateJwtToken(User user,string role)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Login),
                new Claim(ClaimTypes.Role, role)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(2),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
