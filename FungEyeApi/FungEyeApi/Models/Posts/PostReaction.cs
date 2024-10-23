using FungEyeApi.Data.Entities;
using FungEyeApi.Data.Entities.Posts;
using FungEyeApi.Enums;

namespace FungEyeApi.Models.Posts
{
    public class PostReaction
    {
        public PostReaction() { }
        public PostReaction(PostReactionEntity postReactionEntity) 
        {
            Id = postReactionEntity.Id;
            PostId = postReactionEntity.PostId;
            UserId = postReactionEntity.UserId;

        }
        public int Id { get; set; }
        public required int PostId { get; set; }
        public required int UserId { get; set; }
    }

}
