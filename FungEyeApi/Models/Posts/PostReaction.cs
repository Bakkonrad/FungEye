using FungEyeApi.Data.Entities;
using FungEyeApi.Enums;

namespace FungEyeApi.Models.Posts
{
    public class PostReaction
    {
        public int Id { get; set; }
        public ReactionTypeEnum ReactionTypeId { get; set; }
        public int PostId { get; set; }
        public int UserId { get; set; }
        public PostReactionTypeEntity? ReactionType { get; set; }
        public Post? Post { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
    }
}
