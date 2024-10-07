using FungEyeApi.Data.Entities;
using FungEyeApi.Enums;
using Microsoft.AspNetCore.Http.HttpResults;
using Newtonsoft.Json;
using System.Data;

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
        register = 1,
        resetPassword = 2
    }
}