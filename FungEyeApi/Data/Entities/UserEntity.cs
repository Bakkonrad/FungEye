using FungEyeApi.Enums;
using FungEyeApi.Models;

namespace FungEyeApi.Data.Entities
{
    public class UserEntity
    {
        //public UserEntity(string username, string email, string? firstName, string? lastName, string? imageUrl, DateTime createdAt, DateTime modifiedAt, DateTime? dateOfBirth)
        //{
        //    Username = username;
        //    Email = email;
        //    FirstName = firstName;
        //    LastName = lastName;
        //    ImageUrl = imageUrl;
        //    ModifiedAt = modifiedAt;
        //    DateOfBirth = dateOfBirth;
        //}

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

        public ICollection<FriendshipEntity> Friendships { get; set; }
        public ICollection<FriendshipEntity> FriendsWith { get; set; }

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
