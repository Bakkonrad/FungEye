using FungEyeApi.Models;

namespace FungEyeApi.Interfaces
{
    public interface IFollowService
    {
        Task<bool> AddFollow(int userId, int followId);
        Task<List<User>> GetFollows(int userId);
        Task<List<User>> GetFollowers(int userId);
        Task<bool> RemoveFollow(int userId, int followId);
    }
}
