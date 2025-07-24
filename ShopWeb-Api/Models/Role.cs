namespace ShopWeb_Api.Models
{
    public class Role
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<RoleUser> Users { get; set; }
    }
}
