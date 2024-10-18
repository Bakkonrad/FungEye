using FungEyeApi.Data.Entities.Posts;
using FungEyeApi.Enums;

namespace FungEyeApi.Data.Entities
{
    public class PostReactionEntity
    {
        public int Id { get; set; }

        // Post, do któego należy reakcja
        public required int PostId { get; set; }
        public PostEntity Post { get; set; }

        // Użytkownik, który dodał reakcję
        public required int UserId { get; set; }
        public UserEntity User { get; set; }

        public static PostReactionEntity Create(int postId, int userId, int reactionTypeId)
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
