using FungEyeApi.Enums;

namespace FungEyeApi.Models
{
    public class User
    {
        public int Id { get; set; }
        public RoleEnum Role { get; set; }
        public required string Username { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? ImageUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? DateOfBirth { get; set; }
    }
    
    public class LoginUser
    {
        public string? Username { get; set; }
        public string? Email { get; set; }
        public required string Password { get; set; }
    }
}