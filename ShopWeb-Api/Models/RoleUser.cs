namespace ShopWeb_Api.Models
{
    /// <summary>
    /// Представляет связь между пользователем и ролью
    /// </summary>
    public class RoleUser
    {
        /// <summary>
        /// Идентификатор пользователя, связанного с ролью
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Идентификатор роли, связанной с пользователем
        /// </summary>
        public int RoleId { get; set; }

        /// <summary>
        /// Навигационное свойство для связанного пользователя
        /// </summary>
        public User User { get; set; }

        /// <summary>
        /// Навигационное свойство для связанной роли
        /// </summary>
        public Role Role { get; set; }
    }
}
