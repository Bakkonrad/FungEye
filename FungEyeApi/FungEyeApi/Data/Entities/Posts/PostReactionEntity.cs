namespace FungEyeApi.Data.Entities
{
    public class PostReactionEntity
    {
        public int Id { get; set; }

        // Post to which the reaction belongs
        public required int PostId { get; set; }
        public PostEntity? Post { get; set; }

        // User who added the reaction
        public required int UserId { get; set; }
        public UserEntity? User { get; set; }

        public static PostReactionEntity Create(int postId, int userId)
        {
            var entity = new PostReactionEntity
            {
                PostId = postId,
                UserId = userId,
            };

            return entity;
        }
    }
}
