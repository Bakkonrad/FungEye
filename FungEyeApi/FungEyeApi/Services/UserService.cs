using FungEyeApi.Data;
using FungEyeApi.Data.Entities;
using FungEyeApi.Enums;
using FungEyeApi.Interfaces;
using FungEyeApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace FungEyeApi.Services
{
    public class UserService : IUserService
    {
        private readonly DataContext db;

        public UserService(DataContext db)
        {
            this.db = db;
        }

        public async Task<User> GetUserProfile(int userId) // Zwraca dane u¿ytkownika do okna profilu
        {
            try
            {
                var user = await db.Users.FirstOrDefaultAsync(u => u.Id == userId);
                if (user == null)
                {
                    return null;
                }

                return new User(user);
            }
            catch (Exception ex)
            {
                throw new Exception("Error during removing account :" + ex.Message);
            }
        }

        public async Task<bool> RemoveAccount(int userId)
        {
            try
            {
                var user = await db.Users.FirstOrDefaultAsync(u => u.Id == userId);
                if (user == null)
                {
                    throw new Exception("User doesn't exist");
                }

                // Logika usuwania konta
                db.Users.Remove(user);
                await db.SaveChangesAsync();

                return true;
            }
            catch(Exception ex) 
            {
                throw new Exception("Error during removing account :" + ex.Message);
            }
        }

        public async Task<bool> UpdateUser(User user)
        {
            var userEntity = await db.Users.FirstOrDefaultAsync(u => u.Id == user.Id) ?? throw new Exception("User not found in the database");

            userEntity.Username = user.Username;
            userEntity.Email = user.Email;
            userEntity.FirstName = user.FirstName;
            userEntity.LastName = user.LastName;
            userEntity.ImageUrl = user.ImageUrl;
            userEntity.CreatedAt = user.CreatedAt;
            userEntity.ModifiedAt = DateTime.Now;
            userEntity.DateOfBirth = user.DateOfBirth;

            db.SaveChanges();
            return true;

        }

        public async Task<bool> UpdateUserImage(int userId, string imageUrl)
        {
            var userEntity = await db.Users.FirstOrDefaultAsync(u => u.Id == userId) ?? throw new Exception("User not found in the database");
            userEntity.ImageUrl = imageUrl;

            db.SaveChanges();
            return true;
        }

        public async Task<bool> IsAdmin(int userId)
        {
            var userEntity = await db.Users.FirstOrDefaultAsync(u => u.Id == userId) ?? throw new Exception("User not found in the database");
            if((RoleEnum)userEntity.Role != RoleEnum.Admin)
            {
                return false;
            }

            return true;
        }

        public async Task<List<User>> GetUsers(int? page = null, string? search = null)
        {
            try
            {
                var query = db.Users.AsQueryable();

                if (!String.IsNullOrWhiteSpace(search))
                {
                    query = query.Where(u => u.Username.Contains(search.ToString()) || u.Email.Contains(search.ToString()) || u.FirstName.Contains(search.ToString()));
                }

                int pageSize = 100;
                int pageNumber = page != null ? page.Value : 1;
                query = query.Skip((pageNumber - 1) * pageSize).Take(pageSize);

                var users = await query.ToListAsync();

                return users.Select(u => new User(u)).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Error during retrieving users: " + ex.Message);
            }
        }

        public async Task<bool> AddFollow(int userId, int friendId)
        {
            try
            {
                var friendship = new FriendshipEntity
                {
                    UserId = userId,
                    FriendId = friendId
                };

                db.Friendships.Add(friendship);
                await db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Error during adding friendship: " + ex.Message);
            }
        }

        public async Task<List<User>> GetFriends(int userId)
        {
            try
            {
                var friends = await db.Friendships
                            .Where(f => f.UserId == userId)
                            .Select(f => f.Friend)
                            .ToListAsync();

                return friends.Select(u => new User(u)).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Error during retrieving friendships: " + ex.Message);
            }

        }

        public async Task<bool> DeleteFriend(int userId, int friendId)
        {
            try
            {
                var friendship = await db.Friendships
                .FirstOrDefaultAsync(f => f.UserId == userId && f.FriendId == friendId);

                if (friendship != null)
                {
                    db.Friendships.Remove(friendship);
                    await db.SaveChangesAsync();
                    return true;
                }

                return false; // Jeœli relacja nie istnieje
            }
            catch (Exception ex)
            {
                throw new Exception("Error during deleting friendship: " + ex.Message);
            }

        }

        public async Task<string> GetUserImage(int userId)
        {
            var user = await db.Users.FirstOrDefaultAsync(u => u.Id == userId) ?? throw new Exception("User not found in the database");
            return user.ImageUrl;
        }

        public async Task<bool> IsUsernameOrEmailUsed(string? username, string? email)
        {
            UserEntity? existingUser = null;

            if (username != null && email != null)
            {
                existingUser = await db.Users.FirstOrDefaultAsync(u => u.Username == username || u.Email == email);
            }
            else if (username != null && email == null)
            {
                existingUser = await db.Users.FirstOrDefaultAsync(u => u.Username == username);
            }
            else if (email != null && username == null)
            {
                existingUser = await db.Users.FirstOrDefaultAsync(u => u.Email == email);
            }
            else
            {
                throw new Exception("Username or email is required");
            }

            return existingUser != null ? true : false;
        }
    }
}