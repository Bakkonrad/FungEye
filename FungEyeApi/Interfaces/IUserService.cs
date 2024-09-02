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
        Task<List<User>> GetUsers(int? page = null, string? search = null);
    }
}
