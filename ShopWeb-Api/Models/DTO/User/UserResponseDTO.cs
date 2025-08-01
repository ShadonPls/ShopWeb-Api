namespace ShopWeb_Api.Models.DTO.User
{
    public class UserResponseDTO
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public List<string> Roles { get; set; } = new();
    }
}
