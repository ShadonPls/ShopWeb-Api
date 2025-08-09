namespace ShopWeb_Api.Models
{
    /// <summary>
    /// Представляет роль пользователя в системе
    /// </summary>
    public class Role
    {
        /// <summary>
        /// Уникальный идентификатор роли
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Название роли (например: "Admin", "User")
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Коллекция пользователей, имеющих данную роль
        /// </summary>
        public ICollection<RoleUser> Users { get; set; }
    }
}
