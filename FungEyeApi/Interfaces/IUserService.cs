using FungEyeApi.Models;

namespace FungEyeApi.Interfaces
{
    public interface IUserService
    {
        Task RemoveAccount(string userId, string token);
    }
}
