using FungEyeApi.Enums;
using FungEyeApi.Models;
using FungEyeApi.Models.Posts;

namespace FungEyeApi.Interfaces
{
    public interface IPostsService
    {
        Task<bool> AddPost(Post post);
        Task<bool> EditPost(Post post);
        Task<bool> DeletePost(int postId);
        Task<bool> LikePost(int postId, int userId);
        Task<bool> UnlikePost(int postId, int userId);
        Task<bool> AddComment(Comment comment);
        Task<bool> DeleteComment(int commentId);
        Task<bool> EditComment(Comment comment);
        Task<List<Comment>> GetComments(int postId);
        Task<List<Post>> GetPosts(PostsFilter filter, int userId, int? page = null);
    }
}
