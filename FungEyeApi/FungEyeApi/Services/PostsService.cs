using FungEyeApi.Data;
using FungEyeApi.Data.Entities;
using FungEyeApi.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.JsonWebTokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace FungEyeApi.Services
{
    public class PostsService : IPostsService
    {
        private readonly DataContext db;

        public PostsService(DataContext db)
        {
            this.db = db;
        }

        //public async Task<bool> RemoveAccount(int userId, string token)
        //{
        //    try
        //    {
        //        // SprawdŸ, czy token JWT jest prawid³owy
        //        var isValidToken = ValidateJWTToken(token, userId);
        //        if (!isValidToken)
        //        {
        //            throw new Exception("Unauthorized");
        //        }

        //        var user = await db.Users.FirstOrDefaultAsync(u => u.Id == userId);
        //        if (user == null)
        //        {
        //            throw new Exception("User doesn't exist");
        //        }

        //        // Logika usuwania konta
        //        db.Users.Remove(user);
        //        await db.SaveChangesAsync();

        //        return true;
        //    }
        //    catch(Exception ex) 
        //    {
        //        throw new Exception("Error during removing account :" + ex.Message);
        //    }
        //}
    }
}