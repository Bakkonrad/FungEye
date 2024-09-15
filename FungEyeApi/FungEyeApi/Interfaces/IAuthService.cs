using FungEyeApi.Models;

namespace FungEyeApi.Interfaces
{
    public interface IAuthService
    {
        Task<bool> RegisterUser(User user);
        Task<bool> RegisterAdmin(User user);
        Task<bool> ChangePassword(int userId, string newPassword);
        Task<bool> IsUsernameOrEmailUsed(string? username, string? email);
        Task<string> Login(LoginUser user); //JWT token
    }
}
