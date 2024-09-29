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
        private readonly IBlobStorageService _blobStorageService;

        public UserService(DataContext db, IBlobStorageService blobStorageService)
        {
            this.db = db;
            _blobStorageService = blobStorageService;
        }

        public async Task<User> GetUserProfile(int userId) // Zwraca dane u¿ytkownika do okna profilu
        {
            try
            {
                var userEntity = await db.Users.FirstOrDefaultAsync(u => u.Id == userId);
                if (userEntity == null)
                {
                    throw new Exception("User not found");
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

                userEntity.DateDeleted = DateTime.Now;
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

        

        public async Task<string> GetUserImage(int userId)
        {
            var user = await db.Users.FirstOrDefaultAsync(u => u.Id == userId) ?? throw new Exception("User not found in the database");
            return user.ImageUrl;
        }

        public async Task<DateTime?> BanUser(int userId, BanOptionEnum banOption)
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

                return userEntity.BanExpirationDate;
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

        public async Task<bool> RetrieveAccount(int userId)
        {
            try
            {
                var userEntity = await db.Users.FirstOrDefaultAsync(u => u.Id == userId);
                if (userEntity == null)
                {
                    throw new Exception("User not found");
                }

                userEntity.DateDeleted = null;
                await db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> DeleteAvatar(int userId)
        {
            try
            {
                var userEntity = await db.Users.FirstOrDefaultAsync(u => u.Id == userId);
                if (userEntity == null)
                {
                    throw new Exception("User not found");
                }

                var result = await _blobStorageService.DeleteFile(userEntity.ImageUrl);

                if(result)
                {
                    userEntity.ImageUrl = null;
                    await db.SaveChangesAsync();
                }
                else
                {
                    throw new Exception("Error during deleting avatar");
                }

                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}