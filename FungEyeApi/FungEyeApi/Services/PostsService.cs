using FungEyeApi.Data;
using FungEyeApi.Data.Entities;
using FungEyeApi.Enums;
using FungEyeApi.Interfaces;
using FungEyeApi.Models;
using FungEyeApi.Models.Posts;
using Microsoft.EntityFrameworkCore;

namespace FungEyeApi.Services
{
    public class PostsService : IPostsService
    {
        private readonly DataContext db;
        private readonly IBlobStorageService _blobStorageService;


        public PostsService(DataContext db, IBlobStorageService blobStorageService)
        {
            this.db = db;
            _blobStorageService = blobStorageService;
        }

        public async Task<bool> AddComment(Comment comment)
        {
            try
            {
                var commentEntity = CommentEntity.Create(comment.PostId, comment.User.Id, comment.Content);
                await db.Comments.AddAsync(commentEntity);
                await db.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Error during adding comment: " + ex.Message);
            }
        }

        public async Task<bool> AddPost(Post post)
        {
            try
            {
                if(post.Content == null)
                {
                    throw new Exception("Post content cannot be null");
                }
                var postEntity = PostEntity.Create(post.UserId, post.Content, post.ImageUrl);
                await db.Posts.AddAsync(postEntity);
                await db.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Error during adding post: " + ex.Message);
            }
        }

        public async Task<bool> DeleteComment(int commentId)
        {
            try
            {
                var comment = await db.Comments.FirstOrDefaultAsync(c => c.Id == commentId);

                if (comment == null)
                {
                    return false;
                }

                db.Comments.Remove(comment);
                await db.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Error during deleting comment: " + ex.Message);
            }
        }

        public async Task<bool> DeletePost(int postId)
        {
            try
            {
                var post = await db.Posts.FirstOrDefaultAsync(p => p.Id == postId);

                if (post == null)
                {
                    return false;
                }

                if(post.ImageUrl != null)
                {
                    await _blobStorageService.DeleteFile(post.ImageUrl, Enums.BlobContainerEnum.Posts);
                }

                db.Posts.Remove(post);
                await db.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Error during deleting post: " + ex.Message);
            }
        }

        public async Task<bool> EditComment(Comment comment)
        {
            try
            {
                var commentEntity = await db.Comments.FirstOrDefaultAsync(c => c.Id == comment.Id);

                if (commentEntity == null)
                {
                    return false;
                }

                commentEntity.Content = comment.Content;
                await db.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Error during editing comment: " + ex.Message);
            }
        }

        public async Task<bool> EditPost(Post post)
        {
            try
            {
                var postEntity = await db.Posts.FirstOrDefaultAsync(p => p.Id == post.Id);

                if (postEntity == null)
                {
                    return false;
                }
                else if (post.Content == null)
                {
                    throw new Exception("New content cannot be null");
                }

                postEntity.Content = post.Content;
                postEntity.ModifiedAt = DateTime.Now;
                postEntity.ImageUrl = post.ImageUrl;

                await db.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Error during editing post: " + ex.Message);
            }
        }

        public async Task<List<Comment>> GetComments(int postId)
        {
            try
            {
                var comments = await db.Comments.Where(c => c.PostId == postId).Include(c=>c.User).ToListAsync();
                var commentsToModel = comments.Select(c => new Comment(c)).ToList();

                return commentsToModel;
            }
            catch (Exception ex)
            {
                throw new Exception("Error during retrieving comments: " + ex.Message);
            }
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
                postsToModel = await GetPostsInfo(postsToModel, userId);

                return postsToModel;
            }
            catch (Exception ex)
            {
                throw new Exception("Error during retrieving users: " + ex.Message);
            }
        }

        public async Task<bool> AddPostReaction(int postId, int userId)
        {
            try
            {
                if(await db.Reactions.AnyAsync(r => r.PostId == postId && r.UserId == userId))
                {
                    return false;
                }

                var reactionEntity = PostReactionEntity.Create(postId, userId);

                await db.Reactions.AddAsync(reactionEntity);
                await db.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Adding post reaction failed. " + ex.Message);
            }
        }

        public async Task<bool> DeletePostReaction(int postId, int userId)
        {
            try
            {
                var existingReaction = await db.Reactions.FirstOrDefaultAsync(r => r.PostId == postId && r.UserId == userId);

                if(existingReaction == null)
                {
                    return false;
                }

                db.Reactions.Remove(existingReaction);
                await db.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Deleting post reaction failed. " + ex.Message);
            }
        }

        private async Task<List<Post>> GetPostsInfo(List<Post> posts, int userId)
        {
            var reactions = db.Reactions.AsQueryable();
            var comments = db.Comments.AsQueryable();

            foreach (var post in posts)
            {
                post.LikeAmount = await reactions.CountAsync(r => r.PostId == post.Id);
                post.CommentsAmount = await comments.CountAsync(r => r.PostId == post.Id);
                post.LoggedUserReacted = await reactions.AnyAsync(r => r.PostId == post.Id && r.UserId == userId);
            }

            return posts;
        }
    }
}