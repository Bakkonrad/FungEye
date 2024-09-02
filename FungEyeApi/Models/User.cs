using FungEyeApi.Data.Entities;
using FungEyeApi.Enums;

namespace FungEyeApi.Models
{
    public class User
    {
        public User() { }
        public User(UserEntity user)
        {
            Id = user.Id;
            Role = (RoleEnum)user.Role;
            Username = user.Username;
            Email = user.Email;
            FirstName = user.FirstName;
            LastName = user.LastName;
            ImageUrl = user.ImageUrl;
            CreatedAt = user.CreatedAt;
            ModifiedAt = user.ModifiedAt;
            DateOfBirth = user.DateOfBirth;
        }
        public int Id { get; set; }
        public RoleEnum Role { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? ImageUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public DateTime? DateOfBirth { get; set; }
    }
    
    public class LoginUser
    {
        public LoginUser() { }
        public string? Username { get; set; }
        public string? Email { get; set; }
        public required string Password { get; set; }
    }
}