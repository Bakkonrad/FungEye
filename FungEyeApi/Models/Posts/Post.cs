using FungEyeApi.Data.Entities;

namespace FungEyeApi.Models.Posts
{
    public class Post
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }

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
