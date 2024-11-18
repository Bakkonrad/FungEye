using FungEyeApi.Data;
using FungEyeApi.Data.Entities;
using FungEyeApi.Enums;
using FungEyeApi.Interfaces;
using FungEyeApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace FungEyeApi.Services
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;
        private readonly DataContext db;

        public AuthService(DataContext db, IConfiguration configuration, IEmailService emailService)
        {
            this._configuration = configuration;
            this._emailService = emailService;
            this.db = db;
        }

        public async Task<bool> IsUsernameOrEmailUsed(string? username, string? email, int? userId = null)
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

            if (userId != null && existingUser != null) // if user is updating his profile and not changing username or email
            {
                return existingUser.Id != userId ? true : false;
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

                if (checkUser.DateDeleted != null)
                {
                    throw new Exception("Account is deleted");
                }

                string token = await CreateToken(new User(checkUser), CreateTokenEnum.Login);
                return token;
            }
            catch (Exception)
            {
                throw;
            }

        }

        public async Task<bool> ChangePassword(int userId, string newPassword)
        {
            try
            {
                var userEntity = await db.Users.FirstOrDefaultAsync(u => u.Id == userId);

                if (userEntity == null)
                {
                    throw new ArgumentException("User not found in the database");
                }

                userEntity.Password = BCrypt.Net.BCrypt.HashPassword(newPassword);
                await db.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> SendResetPasswordEmail(string userEmail)
        {
            var user = GetUser(userEmail);

            var token = user.Result != null ? CreateToken(user.Result, CreateTokenEnum.ResetPassword) : throw new Exception("User not found");
            var resetLink = $"http://localhost:5173/resetPassword?token={token.Result}";

            var result = await _emailService.SendEmailAsync(userEmail, SendEmailOptionsEnum.ResetPassword, resetLink);

            if (result)
            {
                return true;
            }
            else
            {
                throw new Exception("Email sending failed");
            }
        }

        public async Task<bool> SendSetAdminPasswordEmail(string userEmail)
        {
            if (userEmail == null)
            {
                throw new Exception("Email is required");
            }
            var user = GetUser(userEmail);

            var token = user.Result != null ? CreateToken(user.Result, CreateTokenEnum.ResetPassword) : throw new Exception("User not found");
            var setLink = $"http://localhost:5173/resetPassword?token={token.Result}";

            var result = await _emailService.SendEmailAsync(userEmail, SendEmailOptionsEnum.SetAdminPassword, setLink);

            if (result)
            {
                return true;
            }
            else
            {
                throw new Exception("Email sending failed");
            }
        }


        private Task<string> CreateToken(User user, CreateTokenEnum createTokenOption)
        {
            string userRole = user.Role == RoleEnum.Admin ? "Admin" : "User";

            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, userRole),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("Jwt:Token").Value!));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            switch (createTokenOption)
            {
                case CreateTokenEnum.ResetPassword:
                    var resetPasswordToken = new JwtSecurityToken(
                                claims: claims,
                                expires: DateTime.Now.AddHours(2),
                                signingCredentials: creds
                                );
                    var resetPasswordJwt = new JwtSecurityTokenHandler().WriteToken(resetPasswordToken);
                    return Task.FromResult(resetPasswordJwt);

                case CreateTokenEnum.Login:
                    var loginToken = new JwtSecurityToken(
                        claims: claims,
                        expires: DateTime.Now.AddDays(1),
                        signingCredentials: creds
                        );

                    var jwt = new JwtSecurityTokenHandler().WriteToken(loginToken);
                    return Task.FromResult(jwt);
            }
            throw new Exception("Invalid token creation option");
        }

        public async Task<bool> DoesUserExist(string? email)
        {
            UserEntity? existingUser = null;

            if (email != null)
            {
                existingUser = await db.Users.FirstOrDefaultAsync(u => u.Email == email);
            }
            else
            {
                throw new Exception("Email is required");
            }

            return existingUser != null ? true : false;
        }

        public async Task<User> GetUser(string? email)
        {
            UserEntity? existingUser = null;

            if (email != null)
            {
                existingUser = await db.Users.FirstOrDefaultAsync(u => u.Email == email);
            }
            else
            {
                throw new Exception("Email is required");
            }

            return new User(existingUser) ?? null;
        }

        public Task<bool> ValidateToken(string token)
        {
            throw new NotImplementedException();
        }
    }
}