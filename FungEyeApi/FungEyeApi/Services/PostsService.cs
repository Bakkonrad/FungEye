using FungEyeApi.Data;
using FungEyeApi.Data.Entities;
using FungEyeApi.Enums;
using FungEyeApi.Interfaces;
using FungEyeApi.Models;
using FungEyeApi.Models.Posts;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.JsonWebTokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace FungEyeApi.Services
{
    public class PostsService : IPostsService
    {
        private readonly DataContext db;

        public PostsService(DataContext db)
        {
            this.db = db;
        }

        public async Task<bool> AddComment(Comment comment)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> AddPost(Post post)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> DeleteComment(int commentId)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> DeletePost(int postId)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> EditComment(Comment comment)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> EditPost(Post post)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Comment>> GetComments(int postId)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Post>> GetPosts(PostsFilter filter, int userId, int? page = null)
        {
            try
            {
                var query = db.Posts.AsQueryable();

                switch(filter)
                {
                    case PostsFilter.All:
                        query = query.OrderByDescending(p => p.CreatedAt);
                        break;

                    case PostsFilter.Follows:
                        var followedUserIds = await db.Follows.Where(u => u.UserId == userId).Select(f => f.FollowedUserId).ToListAsync();
                        query = query
                            .Where(p => followedUserIds.Contains(p.UserId))
                            .OrderByDescending(p => p.CreatedAt);
                        break;

                    default:
                        query = query.OrderByDescending(p => p.CreatedAt);
                        break;
                }

                int pageSize = 5;
                int pageNumber = page ?? 1;
                query = query.Skip((pageNumber - 1) * pageSize).Take(pageSize);

                var posts = await query.ToListAsync();

                var postsToModel = posts.Select(p => new Post(p)).ToList();
                postsToModel = await GetPostsInfo(postsToModel);

                return postsToModel;
            }
            catch (Exception ex)
            {
                throw new Exception("Error during retrieving users: " + ex.Message);
            }
        }

        public async Task<bool> LikePost(int postId, int userId)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UnlikePost(int postId, int userId)
        {
            throw new NotImplementedException();
        }

        private async Task<List<Post>> GetPostsInfo(List<Post> posts)
        {
            var reactions = db.PostReactions.AsQueryable();
            var comments = db.Comments.AsQueryable();

            foreach (var post in posts)
            {
                post.LikeAmount = await reactions.CountAsync(r => r.PostId == post.Id);
                post.CommentsAmount = await comments.CountAsync(r => r.PostId == post.Id);
                post.LoggedUserReacted = await reactions.AnyAsync(r => r.PostId == post.Id && r.UserId == post.UserId);
            }

            return posts;
        }
    }
}