namespace FungEyeApi.Data.Entities.Posts
{
    public class PostEntity
    {
        public int Id { get; set; }
        public required string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public string? ImageUrl { get; set; }

        // Użytkownik, który stworzył post
        public required int UserId { get; set; }
        public UserEntity User { get; set; }

        public ICollection<PostReactionEntity>? Reactions { get; set; }

        public ICollection<CommentEntity>? Comments { get; set; }
    }
}
