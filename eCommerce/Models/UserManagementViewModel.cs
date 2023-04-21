namespace eCommerce.Models
{
    public class UserManagementViewModel
    {
        public List<User> Users { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public bool IsAdmin { get; set; }
    }
}
