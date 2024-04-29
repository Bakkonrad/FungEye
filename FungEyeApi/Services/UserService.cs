using FungEyeApi.Interfaces;
using System.Net;
using System.Text;

namespace FungEyeApi.Models
{
    public class UserService : IUserService
    {
        public async Task RemoveAccount(string userId, string token)
        {
            //// SprawdŸ, czy token JWT jest prawid³owy
            //var isValidToken = ValidateJWTToken(token);
            //if (!isValidToken)
            //{
            //    return new HttpResponseMessage(HttpStatusCode.Unauthorized)
            //    {
            //        Content = new StringContent("{ \"message\": \"Nieautoryzowany dostêp.\" }",
            //            Encoding.UTF8, "application/json")
            //    };
            //}

            //// SprawdŸ, czy identyfikator u¿ytkownika w ¿¹daniu zgadza siê z identyfikatorem u¿ytkownika w tokenie JWT
            //var tokenUserId = ExtractUserIdFromToken(token);
            //if (tokenUserId != userId)
            //{
            //    return new HttpResponseMessage(HttpStatusCode.Forbidden)
            //    {
            //        Content = new StringContent("{ \"message\": \"Brak uprawnieñ do usuniêcia konta.\" }",
            //            Encoding.UTF8, "application/json")
            //    };
            //}

            //// Logika usuwania konta
            //await _userService.RemoveUser(userId);

            //return new HttpResponseMessage(HttpStatusCode.OK)
            //{
            //    Content = new StringContent("{ \"message\": \"Konto zosta³o usuniête.\" }",
            //        Encoding.UTF8, "application/json")
            //};

            throw new NotImplementedException();
        }
    }
}