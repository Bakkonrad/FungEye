using FungEyeApi.Enums;
using FungEyeApi.Models;

namespace FungEyeApi.Data.Entities
{
    public class UserEntity
    {
        public int Id { get; set; }
        public required int Role { get; set; }
        public required string Username { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? ImageUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public DateTime? BanExpirationDate { get; set; }
        public ICollection<FollowEntity> Follows { get; set; }

        public static UserEntity Create(RoleEnum role, string username, string email, string password, DateTime? dateOfBirth, string? firstname, string? lastname, string? imageUrl)
        {
            return new UserEntity
            {
                Role = (int)role,
                Username = username,
                Email = email,
                Password = password,
                FirstName = firstname,
                LastName = lastname,
                ImageUrl = imageUrl,
                DateOfBirth = dateOfBirth,
                CreatedAt = DateTime.Now
            };
        }
    }
}
