using FungEyeApi.Data;
using FungEyeApi.Data.Entities;
using FungEyeApi.Interfaces;
using FungEyeApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.JsonWebTokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace FungEyeApi.Services
{
    public class UserService : IUserService
    {
        private readonly DataContext db;

        public UserService(DataContext db)
        {
            this.db = db;
        }

        public async Task<User> GetUserProfile(int userId) // Zwraca dane u¿ytkownika do okna profilu
        {
            try
            {
                var user = await db.Users.FirstOrDefaultAsync(u => u.Id == userId);
                if (user == null)
                {
                    throw new Exception("User doesn't exist");
                }

                return new User(user);
            }
            catch (Exception ex)
            {
                throw new Exception("Error during removing account :" + ex.Message);
            }
        }

        public async Task<bool> RemoveAccount(int userId)
        {
            try
            {
                var user = await db.Users.FirstOrDefaultAsync(u => u.Id == userId);
                if (user == null)
                {
                    throw new Exception("User doesn't exist");
                }

                // Logika usuwania konta
                db.Users.Remove(user);
                await db.SaveChangesAsync();

                return true;
            }
            catch(Exception ex) 
            {
                throw new Exception("Error during removing account :" + ex.Message);
            }
        }


        // Walidacja tokenu zosta³a przeniesiona do kontrolera!!!

        //private int ExtractUserIdFromJWTToken(string token)
        //{
        //    int userId = 0;

        //    if (!string.IsNullOrEmpty(token))
        //    {
        //        try
        //        {
        //            var tokenHandler = new JwtSecurityTokenHandler();
        //            var jwtToken = tokenHandler.ReadToken(token) as JwtSecurityToken;

        //            if (jwtToken != null)
        //            {
        //                var claim = jwtToken.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);

        //                if (claim != null && int.TryParse(claim.Value, out userId))
        //                {
        //                    return userId;
        //                }
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            throw new Exception($"Error decoding JWT token: {ex.Message}");
        //        }
        //    }

        //    return userId;
        //}

        //private bool ValidateJWTToken(string token, int PassedUserId)
        //{
        //    var userIdFromToken = ExtractUserIdFromJWTToken(token);
        //    if(userIdFromToken != PassedUserId)
        //    {
        //        throw new Exception("User id passed in the request and one from JWT token doesn't match");
        //    }
        //    return true;
        //}
    }
}