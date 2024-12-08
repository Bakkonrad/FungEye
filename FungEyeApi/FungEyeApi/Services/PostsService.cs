using FungEyeApi.Data;
using FungEyeApi.Data.Entities;
using FungEyeApi.Enums;
using FungEyeApi.Interfaces;
using FungEyeApi.Models;
using FungEyeApi.Models.Posts;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.Design;

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
                if (post.Content == null)
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
                var commentReports = db.Reports.Where(c => c.CommentId == commentId).ToListAsync().Result;

                if (comment == null)
                {
                    throw new Exception("Comment not found");
                }

                if(commentReports != null && commentReports.Count > 0)
                {
                    db.Reports.RemoveRange(commentReports);
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
                var postReports = db.Reports.Where(c => c.PostId == postId).ToListAsync().Result;

                if (post == null)
                {
                    throw new Exception("Post not found");
                }

                if (post.ImageUrl != null)
                {
                    await _blobStorageService.DeleteFile(post.ImageUrl, Enums.BlobContainerEnum.Posts);
                }

                if (postReports != null && postReports.Count > 0)
                {
                    db.Reports.RemoveRange(postReports);
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
                    throw new Exception("Comment not found");
                }
                else if (String.IsNullOrWhiteSpace(comment.Content))
                {
                    throw new Exception("Content cannot be null");
                }

                commentEntity.Content = comment.Content;
                commentEntity.ModifiedAt = DateTime.Now;
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
                    throw new Exception("Post not found");
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
                var comments = await db.Comments.Where(c => c.PostId == postId).Include(c => c.User).ToListAsync();
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

                switch (filter)
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
                throw new Exception("Error during retrieving posts: " + ex.Message);
            }
        }

        public async Task<Post> GetPost(int postId, int userId)
        {
            try
            {
                var post = await db.Posts.FirstOrDefaultAsync(r => r.Id == postId);

                if (post == null)
                {
                    throw new KeyNotFoundException("Post not found");
                }

                var postToModel = new Post(post);
                postToModel = await GetPostInfo(postToModel, userId);

                return postToModel;
            }
            catch (KeyNotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception("Error during retrieving post: " + ex.Message);
            }
        }

        public async Task<bool> AddPostReaction(int postId, int userId)
        {
            try
            {
                if (await db.Reactions.AnyAsync(r => r.PostId == postId && r.UserId == userId))
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

        public async Task<bool> DeletePostReaction(int userId, int postId)
        {
            try
            {
                var existingReaction = await db.Reactions.FirstOrDefaultAsync(r => r.PostId == postId && r.UserId == userId);

                if (existingReaction == null)
                {
                    throw new Exception("Reaction not found");
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

        public async Task<bool> Report(int reporterId, int? postId = null, int? commentId = null)
        {
            try
            {
                if (postId == null && commentId == null)
                {
                    throw new Exception("There is no data provided to create a report");
                }

                var reportEntity = ReportEntity.Create(reporterId, postId, commentId);
                await db.Reports.AddAsync(reportEntity);
                await db.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Error during adding report: " + ex.Message);
            }
        }

        public async Task<List<Report>> GetReports()
        {
            try
            {
                var reports = await db.Reports.Include(r => r.ReportedBy).OrderBy(r => r.Completed).ToListAsync();
                var reportsToModel = reports.Select(r => new Report(r)).ToList();

                return reportsToModel;
            }
            catch (Exception ex)
            {
                throw new Exception("Error during retrieving reports: " + ex.Message);
            }
        }

        public async Task<bool> MarkReportAsCompleted(int reportId)
        {
            try
            {
                var reportEntity = await db.Reports.FirstOrDefaultAsync(r => r.Id == reportId);

                if (reportEntity == null)
                {
                    throw new Exception("Report not found");
                }

                reportEntity.Completed = true;
                reportEntity.ModifiedAt = DateTime.Now;

                await db.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Error during marking report as completed: " + ex.Message);
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

        private async Task<Post> GetPostInfo(Post post, int userId)
        {
            var reactions = db.Reactions.AsQueryable();
            var comments = db.Comments.AsQueryable();

            post.LikeAmount = await reactions.CountAsync(r => r.PostId == post.Id);
            post.CommentsAmount = await comments.CountAsync(r => r.PostId == post.Id);
            post.LoggedUserReacted = await reactions.AnyAsync(r => r.PostId == post.Id && r.UserId == userId);
            post.Comments = await comments.Where(c => c.PostId == post.Id).Include(c => c.User).Select(c => new Comment(c)).ToListAsync();

            return post;
        }
    }
}