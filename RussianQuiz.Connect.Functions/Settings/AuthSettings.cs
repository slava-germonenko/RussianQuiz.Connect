namespace RussianQuiz.Connect.Functions.Settings
{
    public class AuthSettings
    {
        public bool HttpOnlyTokenCookie { get; set; }

        public bool SecureTokenCookie { get; set; }

        public int TokenTtlMinutes { get; set; }

        public string TokenCookieSameSitePolicy { get; set; }

        public string TokenCookieName { get; set; }

        public string TokenCookieDomain { get; set; }

        public string TokenIssuer { get; set; }

        public string TokenSecretKey { get; set; }
    }
}