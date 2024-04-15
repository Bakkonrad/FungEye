namespace FungEyeApi.Data.Entities
{
    public class UserEntity
    {
        public int Id { get; set; }
        public required string Username { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? ImageUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime DateOfBirth { get; set; }

        public static UserEntity Create(string username, string email, string password)
        {
            return new UserEntity
            {
                Username = username,
                Email = email,
                Password = password
            };
        }
    }
}
