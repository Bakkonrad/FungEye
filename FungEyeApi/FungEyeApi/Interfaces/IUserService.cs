using FungEyeApi.Enums;
using FungEyeApi.Models;

namespace FungEyeApi.Interfaces
{
    public interface IUserService
    {
        Task<bool> RemoveAccount(int userId);
        Task<bool> UpdateUser(User user);
        Task<bool> RetrieveAccount(int userId);
        Task<User> GetUserProfile(int userId);
        Task<User> GetSmallUserProfile(int userId);
        Task<bool> UpdateUserImage(int userId, string imageUrl);
        Task<bool> IsAdmin(int userId);
        Task<DateTime?> BanUser(int userId, BanOptionEnum banOption);
        Task<List<User>> GetUsers(int? page = null, string? search = null);
        Task<string> GetUserImage(int userId);
        Task<bool> DeleteAvatar(int userId);
    }
}
