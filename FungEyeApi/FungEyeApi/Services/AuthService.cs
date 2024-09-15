using FungEyeApi.Data;
using FungEyeApi.Data.Entities;
using FungEyeApi.Enums;
using FungEyeApi.Interfaces;
using FungEyeApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace FungEyeApi.Services
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;
        private readonly DataContext db;

        public AuthService(DataContext db, IConfiguration configuration)
        {
            this._configuration = configuration;
            this.db = db;
        }

        public async Task<bool> IsUsernameOrEmailUsed(string? username, string? email)
        {
            UserEntity? existingUser = null;

            if (username != null && email != null)
            {
                existingUser = await db.Users.FirstOrDefaultAsync(u => u.Username == username || u.Email == email);
            }
            else if (username != null && email == null)
            {
                existingUser = await db.Users.FirstOrDefaultAsync(u => u.Username == username);
            }
            else if (email != null && username == null)
            {
                existingUser = await db.Users.FirstOrDefaultAsync(u => u.Email == email);
            }
            else
            {
                throw new Exception("Username or email is required");
            }

            return existingUser != null ? true : false;
        }

        public async Task<bool> Register(User user)
        {
            try
            {
                var existingUser = await db.Users.FirstOrDefaultAsync(u => u.Username == user.Username || u.Email == user.Email);

                if (existingUser != null)
                {
                    throw new Exception("Username or email already in use");
                }

                string passwordHash = BCrypt.Net.BCrypt.HashPassword(user.Password);
                var userEntity = UserEntity.Create(user.Role, user.Username, user.Email, passwordHash, user.DateOfBirth, user.FirstName, user.LastName, user.ImageUrl);

                await db.Users.AddAsync(userEntity);
                await db.SaveChangesAsync();

                user.Id = userEntity.Id;
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("User registration failed. " + ex.Message);
            }
        }

        public async Task<bool> RegisterUser(User user)
        {
            user.Role = RoleEnum.User;
            return await Register(user);
        }

        public async Task<bool> RegisterAdmin(User user)
        {
            user.Role = RoleEnum.Admin;
            return await Register(user);
        }

        public async Task<string> Login(LoginUser requestUser)
        {
            try
            {
                var checkUser = await db.Users.FirstOrDefaultAsync(u => u.Username == requestUser.Username || u.Email == requestUser.Email);
                if (checkUser == null)
                {
                    throw new UnauthorizedAccessException("Username or password is incorrect");
                }

                if (!BCrypt.Net.BCrypt.Verify(requestUser.Password, checkUser.Password))
                {
                    throw new UnauthorizedAccessException("Username or password is incorrect");
                }

                if (checkUser.BanExpirationDate != null && checkUser.BanExpirationDate > DateTime.Now)
                {
                    throw new AccessViolationException(checkUser.BanExpirationDate.ToString());
                }

                string token = await CreateToken(new User(checkUser));
                return token;
            }
            catch(Exception ex)
            {
                throw;
            }
            
        }

        private Task<string> CreateToken(User user)
        {
            string userRole = user.Role == RoleEnum.Admin ? "Admin" : "User";

            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, userRole),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("Jwt:Token").Value!));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds
                );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return Task.FromResult(jwt);
        }
    }
}