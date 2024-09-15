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
                var userEntity = await db.Users.FirstOrDefaultAsync(u => u.Id == userId);
                if (userEntity == null)
                {
                    return null;
                }

                return new User(userEntity);
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
                var userEntity = await db.Users.FirstOrDefaultAsync(u => u.Id == userId);
                if (userEntity == null)
                {
                    throw new Exception("User doesn't exist");
                }

                // Logika usuwania konta
                db.Users.Remove(userEntity);
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

            await db.SaveChangesAsync();
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

        public async Task<bool> AddFollow(int userId, int followId)
        {
            try
            {
                //sprawdzenie czy follow miêdzy u¿ytkownikami ju¿ istnieje
                var existingFollow = await db.Follows.FirstOrDefaultAsync(u => u.UserId == userId) ?? throw new Exception("User not found in the database");

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
                throw new Exception("Error during adding follow: " + ex.Message);
            }
        }

        public async Task<List<User>> GetFollows(int userId)
        {
            try
            {
                var friends = await db.Follows
                            .Where(f => f.UserId == userId)
                            .Select(f => f.FollowedUser)
                            .ToListAsync();

                return friends.Select(u => new User(u)).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Error during retrieving follows: " + ex.Message);
            }

        }

        public async Task<bool> DeleteFollow(int userId, int followId)
        {
            try
            {
                var follow = await db.Follows
                .FirstOrDefaultAsync(f => f.UserId == userId && f.FollowedUserId == followId);

                if (follow != null)
                {
                    db.Follows.Remove(follow);
                    await db.SaveChangesAsync();
                    return true;
                }

                return false; // Jeœli relacja nie istnieje
            }
            catch (Exception ex)
            {
                throw new Exception("Error during deleting follow: " + ex.Message);
            }

        }

        public async Task<string> GetUserImage(int userId)
        {
            var user = await db.Users.FirstOrDefaultAsync(u => u.Id == userId) ?? throw new Exception("User not found in the database");
            return user.ImageUrl;
        }

        public async Task<bool> BanUser(int userId, BanOptionEnum banOption)
        {
            try
            {
                var userEntity = await db.Users.FirstOrDefaultAsync(u => u.Id == userId);
                if (userEntity == null)
                {
                    throw new Exception("User doesn't exist");
                }

                switch(banOption)
                {
                    case BanOptionEnum.Week:
                        userEntity.BanExpirationDate = DateTime.Now.AddDays(7);
                        break;
                    case BanOptionEnum.Month:
                        userEntity.BanExpirationDate = DateTime.Now.AddMonths(1);
                        break;
                    case BanOptionEnum.Year:
                        userEntity.BanExpirationDate = DateTime.Now.AddYears(1);
                        break;
                    case BanOptionEnum.Permanent:
                        userEntity.BanExpirationDate = DateTime.Now.AddYears(100);
                        break;
                }

                await db.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Error during banning user:" + ex.Message);
            }
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