using InventoryManagementSystem.Constants;

namespace InventoryManagementSystem.Domain.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public UserRole Role { get; set; } = UserRole.Viewer;
        public User() { }
        public User(int id, string username, string password, string email, UserRole role)
        {
            Id = id;
            Username = username;
            Password = password;
            Email = email;
            Role = role;
        }
    }
}
