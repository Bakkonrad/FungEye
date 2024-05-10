using FungEyeApi.Data.Entities.Posts;
using FungEyeApi.Enums;

namespace FungEyeApi.Data.Entities
{
    public class PostReactionEntity
    {
        public int Id { get; set; }
        public int ReactionTypeId { get; set; }
        public int PostId { get; set; }
        public int UserId { get; set; }
        public PostReactionTypeEntity ReactionType { get; set; }
        public PostEntity Post { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }

        public static PostReactionEntity Create(int postId, int userId, int reactionTypeId)
        {
            var entity = new PostReactionEntity
            {
                PostId = postId,
                UserId = userId,
                ReactionTypeId = reactionTypeId,
                CreatedAt = DateTime.Now,
            };

            return entity;
        }

        public static PostReactionEntity Modify(int postId, int userId, int reactionTypeId)
        {
            //var entity = new PostReactionEntity
            //{
            //    PostId = postId,
            //    UserId = userId,
            //    ReactionTypeId = reactionTypeId,
            //    CreatedAt = DateTime.Now,
            //};

            //return entity;
            throw new NotImplementedException();
        }
    }
}
