using ShopWeb_Api.Models.DTO.User;

namespace ShopWeb_Api.Services.Interfaces
{
    public interface IAuthService
    {
        Task<UserResponseDTO> RegisterAsync(RegisterDTO registerDto);
        Task<string> LoginAsync(LoginDTO loginDto);
    }
}
