using FungEyeApi.Models;

namespace FungEyeApi.Interfaces
{
    public interface IUserService
    {
        Task<bool> RemoveAccount(int userId);
    }
}
