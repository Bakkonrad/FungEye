using FungEyeApi.Data.Entities;
using FungEyeApi.Enums;
using Newtonsoft.Json;

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

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("role")]
        public RoleEnum Role { get; set; }

        [JsonProperty("username")]
        public string? Username { get; set; }

        [JsonProperty("email")]
        public string? Email { get; set; }

        [JsonProperty("password")]
        public string? Password { get; set; }

        [JsonProperty("firstName")]
        public string? FirstName { get; set; }

        [JsonProperty("lastName")]
        public string? LastName { get; set; }

        [JsonProperty("imageUrl")]
        public string? ImageUrl { get; set; }

        [JsonProperty("createdAt")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("modifiedAt")]
        public DateTime? ModifiedAt { get; set; }

        [JsonProperty("dateOfBirth")]
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