using ShopWeb_Api.Models.DTO.User;

namespace ShopWeb_Api.Services.Interfaces
{
    public interface IAuthService
    {
        Task<UserResponseDTO> RegisterAsync(RegisterDTO registerDto, CancellationToken cancellationToken);
        Task<string> LoginAsync(LoginDTO loginDto, CancellationToken cancellationToken);
    }
}
