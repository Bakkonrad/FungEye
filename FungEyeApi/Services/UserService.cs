using FungEyeApi.Interfaces;
using System.Net;
using System.Text;

namespace FungEyeApi.Models
{
    public class UserService : IUserService
    {
        public async Task RemoveAccount(string userId, string token)
        {
            //// Sprawd�, czy token JWT jest prawid�owy
            //var isValidToken = ValidateJWTToken(token);
            //if (!isValidToken)
            //{
            //    return new HttpResponseMessage(HttpStatusCode.Unauthorized)
            //    {
            //        Content = new StringContent("{ \"message\": \"Nieautoryzowany dost�p.\" }",
            //            Encoding.UTF8, "application/json")
            //    };
            //}

            //// Sprawd�, czy identyfikator u�ytkownika w ��daniu zgadza si� z identyfikatorem u�ytkownika w tokenie JWT
            //var tokenUserId = ExtractUserIdFromToken(token);
            //if (tokenUserId != userId)
            //{
            //    return new HttpResponseMessage(HttpStatusCode.Forbidden)
            //    {
            //        Content = new StringContent("{ \"message\": \"Brak uprawnie� do usuni�cia konta.\" }",
            //            Encoding.UTF8, "application/json")
            //    };
            //}

            //// Logika usuwania konta
            //await _userService.RemoveUser(userId);

            //return new HttpResponseMessage(HttpStatusCode.OK)
            //{
            //    Content = new StringContent("{ \"message\": \"Konto zosta�o usuni�te.\" }",
            //        Encoding.UTF8, "application/json")
            //};

            throw new NotImplementedException();
        }
    }
}