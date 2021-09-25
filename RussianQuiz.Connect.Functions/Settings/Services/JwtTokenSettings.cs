using System;
using System.Text;
using Microsoft.Extensions.Options;
using RussianQuiz.Connect.Core.Settings;


namespace RussianQuiz.Connect.Functions.Settings.Services
{
    public class JwtTokenSettings : IJwtTokenSettings
    {
        private readonly AuthSettings _authSettings;

        public string Issuer => _authSettings.TokenIssuer;

        public Encoding SecretEncoding => Encoding.UTF8;

        public string Secret => _authSettings.TokenSecretKey;

        public TimeSpan? Ttl => TimeSpan.FromMinutes(_authSettings.TokenTtlMinutes);


        public JwtTokenSettings(IOptionsSnapshot<AuthSettings> authSettings)
        {
            _authSettings = authSettings.Value;
        }
    }
}