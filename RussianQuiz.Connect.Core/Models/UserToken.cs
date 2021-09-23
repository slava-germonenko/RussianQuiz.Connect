using System;


namespace RussianQuiz.Connect.Core.Models
{
    public class UserToken
    {
        public string TokenValue { get; set; }

        public DateTime? ExpiresAt { get; set; }

        public UserToken(string tokenValue, DateTime? expiresAt)
        {
            TokenValue = tokenValue;
            ExpiresAt = expiresAt;
        }
    }
}