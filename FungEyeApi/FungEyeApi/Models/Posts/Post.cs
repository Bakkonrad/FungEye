using FungEyeApi.Data.Entities;
using Newtonsoft.Json;

namespace FungEyeApi.Models.Posts
{
    public class Post
    {
        public Post() { }

        public Post(PostEntity postEntity)
        {
            Id = postEntity.Id;
            Content = postEntity.Content;
            CreatedAt = postEntity.CreatedAt;
            ModifiedAt = postEntity.ModifiedAt;
            UserId = postEntity.UserId;
            ImageUrl = postEntity.ImageUrl;
        }

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("content")]
        public string? Content { get; set; }

        [JsonProperty("imageUrl")]
        public string? ImageUrl { get; set; }

        [JsonProperty("createdAt")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("modifiedAt")]
        public DateTime? ModifiedAt { get; set; }

        [JsonProperty("userId")]
        public int UserId { get; set; }

        [JsonProperty("likeAmount")]
        public long LikeAmount { get; set; }

        [JsonProperty("commentsAmount")]
        public long CommentsAmount { get; set; }

        [JsonProperty("loggedUserReacted")]
        public bool LoggedUserReacted { get; set; }

        [JsonProperty("deletePhoto")]
        public bool? DeletePhoto { get; set; }

        [JsonProperty("comments")]
        public List<Comment>? Comments { get; set; }
    }
}
