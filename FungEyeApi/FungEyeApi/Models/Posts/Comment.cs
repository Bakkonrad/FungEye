using FungEyeApi.Data.Entities;
using Newtonsoft.Json;

namespace FungEyeApi.Models
{
    public class Comment
    {
        public Comment() { }
        public Comment(CommentEntity commentEntity)
        {
            Id = commentEntity.Id;
            Content = commentEntity.Content;
            CreatedAt = commentEntity.CreatedAt;
            PostId = commentEntity.PostId;
            User = new FollowUser(commentEntity.User!);
        }

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("content")]
        public string? Content { get; set; }

        [JsonProperty("createdAt")]
        public DateTime? CreatedAt { get; set; }

        [JsonProperty("postId")]
        public int PostId { get; set; }

        [JsonProperty("user")]
        public FollowUser? User { get; set; }
    }
}
