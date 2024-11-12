using FungEyeApi.Data.Entities;
using Newtonsoft.Json;

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

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("postId")]
        public int PostId { get; set; }

        [JsonProperty("userId")]
        public int UserId { get; set; }
    }

}
