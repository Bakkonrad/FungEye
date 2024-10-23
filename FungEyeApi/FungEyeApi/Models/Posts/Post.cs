using FungEyeApi.Data.Entities;
using FungEyeApi.Data.Entities.Posts;

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


        public int Id { get; set; }
        public string Content { get; set; }
        public string? ImageUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public int UserId { get; set; }
        public long LikeAmount { get; set; }
        public long CommentsAmount { get; set; }
        public bool LoggedUserReacted { get; set; }
    }
}
