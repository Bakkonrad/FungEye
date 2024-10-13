namespace FungEyeApi.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public int PostId { get; set; }
        public int UserId { get; set; }
        public string? Username { get; set; }
        public required string Content { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
