namespace FungEyeApi.Data.Entities
{
    public class CommentEntity
    {
        public int Id { get; set; }
        public int PostId { get; set; }
        public int UserId { get; set; }
        public required string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public UserEntity User { get; set; }

        public static CommentEntity Create(int postId, int userId, string content)
        {
            var entity = new CommentEntity
            {
                PostId = postId,
                UserId = userId,
                Content = content,
                CreatedAt = DateTime.Now
            };

            return entity;
        }
    }
}
