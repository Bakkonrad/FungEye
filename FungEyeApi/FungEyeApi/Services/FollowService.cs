﻿using FungEyeApi.Data;
using FungEyeApi.Data.Entities;
using FungEyeApi.Interfaces;
using FungEyeApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Data;


namespace FungEyeApi.Services
{
    public class FollowService : IFollowService
    {
        private readonly DataContext db;

        public FollowService(DataContext db)
        {
            this.db = db;
        }

        public async Task<bool> AddFollow(int userId, int followId)
        {
            try
            {
                //check if follow relation between users already exists
                var existingFollow = await db.Follows.FirstOrDefaultAsync(f => f.UserId == userId && f.FollowedUserId == followId);

                if (existingFollow != null)
                {
                    throw new InvalidOperationException("Error during adding follow.");
                }

                var follow = new FollowEntity
                {
                    UserId = userId,
                    FollowedUserId = followId
                };

                db.Follows.Add(follow);
                await db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                if (ex is InvalidOperationException)
                {
                    throw;
                }

                throw new Exception("Error during adding follow: " + ex.Message);
            }
        }

        public async Task<List<FollowUser>> GetFollowers(int userId)
        {
            try
            {
                var followers = await db.Follows
                            .Where(f => f.FollowedUserId == userId)
                            .Select(f => f.User)
                            .ToListAsync();

                return followers != null && followers.Count > 0 ? followers.Select(f => new FollowUser(f!)).ToList() : [];
            }
            catch (Exception ex)
            {
                throw new Exception("Error during retrieving followers: " + ex.Message);
            }
        }

        public async Task<List<FollowUser>> GetFollows(int userId)
        {
            try
            {
                var follows = await db.Follows
                            .Where(f => f.UserId == userId)
                            .Select(f => f.FollowedUser)
                            .ToListAsync();

                return follows != null && follows.Count > 0 ? follows.Select(f => new FollowUser(f!)).ToList() : [];
            }
            catch (Exception ex)
            {
                throw new Exception("Error during retrieving follows: " + ex.Message);
            }

        }

        public async Task<bool> RemoveFollow(int userId, int followId)
        {
            try
            {
                var follow = await db.Follows
                .FirstOrDefaultAsync(f => f.UserId == userId && f.FollowedUserId == followId) ?? throw new Exception("Error during deleting follow: Follow doesn't exist.");

                db.Follows.Remove(follow);
                await db.SaveChangesAsync();
                return true;

            }
            catch (Exception ex)
            {
                throw new Exception("Error during deleting follow: " + ex.Message);
            }

        }

        public async Task<bool> IsFollowing(int userId, int followId)
        {
            try
            {
                //check if follow already exists between users
                var existingFollow = await db.Follows.FirstOrDefaultAsync(f => f.UserId == userId && f.FollowedUserId == followId);

                return existingFollow != null;
            }
            catch (Exception ex)
            {
                if (ex is InvalidOperationException)
                {
                    throw;
                }

                throw new Exception("Error during adding follow: " + ex.Message);
            }
        }
    }
}
