using System;
using System.Text;

using Microsoft.Extensions.Options;

using RussianQuiz.Connect.Core.Settings;
using RussianQuiz.Connect.Functions.Options;


namespace RussianQuiz.Connect.Functions.Settings
{
    public class JwtTokenSettings : IJwtTokenSettings
    {
        private readonly AuthOptions _authOptions;

        public string Issuer => _authOptions.TokenIssuer;

        public Encoding SecretEncoding => Encoding.UTF8;

        public string Secret => _authOptions.TokenSecretKey;

        public TimeSpan? Ttl => TimeSpan.FromMinutes(_authOptions.TokenTtlMinutes);


        public JwtTokenSettings(IOptionsSnapshot<AuthOptions> authOptions)
        {
            _authOptions = authOptions.Value;
        }
    }
}