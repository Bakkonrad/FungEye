using FungEyeApi.Models;

namespace FungEyeApi.Interfaces
{
    public interface IFollowService
    {
        Task<bool> AddFollow(int userId, int followId);
        Task<bool> IsFollowing(int userId, int followId);
        Task<List<FollowUser>> GetFollows(int userId);
        Task<List<FollowUser>> GetFollowers(int userId);
        Task<bool> RemoveFollow(int userId, int followId);
    }
}
