using FungEyeApi.Data.Entities.Posts;
using FungEyeApi.Data.Entities;

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
            UserId = commentEntity.UserId;
        }


        public int Id { get; set; }
        public required string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public required int PostId { get; set; }
        public required int UserId { get; set; }
    }
}
