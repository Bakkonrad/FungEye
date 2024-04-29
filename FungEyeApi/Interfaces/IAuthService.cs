using FungEyeApi.Models;

namespace FungEyeApi.Interfaces
{
    public interface IAuthService
    {
        Task<bool> Register(User user);
        Task<bool> IsUsernameOrEmailUsed(string? username, string? email);
        Task<string> Login(LoginUser user); //JWT token
    }
}
