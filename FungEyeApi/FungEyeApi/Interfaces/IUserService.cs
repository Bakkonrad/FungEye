using FungEyeApi.Enums;
using FungEyeApi.Models;

namespace FungEyeApi.Interfaces
{
    public interface IUserService
    {
        Task<bool> RemoveAccount(int userId);
        Task<bool> UpdateUser(User user);
        Task<User> GetUserProfile(int userId);
        Task<bool> UpdateUserImage(int userId, string imageUrl);
        Task<bool> IsAdmin(int userId);
        Task<bool> BanUser(int userId, BanOptionEnum banOption);
        Task<List<User>> GetUsers(int? page = null, string? search = null);
        Task<string> GetUserImage(int userId);
        Task<bool> AddFollow(int userId, int followId);
        Task<List<User>> GetFollows(int userId);
        Task<bool> DeleteFollow(int userId, int followId);
    }
}
