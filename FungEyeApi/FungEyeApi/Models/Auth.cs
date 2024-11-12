using Newtonsoft.Json;

namespace FungEyeApi.Models
{
    public class SendResetPasswordEmail
    {
        [JsonProperty("email")]
        public string? Email { get; set; }
    }

    public class ResetPassword
    {
        [JsonProperty("password")]
        public string? Password { get; set; }
    }

    public enum CreateTokenEnum
    {
        Login = 1,
        ResetPassword = 2
    }
}