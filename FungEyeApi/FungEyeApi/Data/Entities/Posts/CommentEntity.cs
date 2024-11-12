
namespace FungEyeApi.Data.Entities
{
    public class CommentEntity
    {
        public int Id { get; set; }
        public required string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }

        // Post to which the comment belongs
        public required int PostId { get; set; }
        public PostEntity? Post { get; set; }

        // User who created the comment
        public required int UserId { get; set; }
        public UserEntity? User { get; set; }

        public ICollection<ReportEntity> Reports { get; set; } = [];
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
