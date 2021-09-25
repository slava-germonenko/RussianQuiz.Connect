using System;
using System.Text;


namespace RussianQuiz.Connect.Core.Settings
{
    public interface IJwtTokenSettings
    {
        public string Issuer { get; }

        public Encoding SecretEncoding { get; }

        public string Secret { get; }

        public TimeSpan? Ttl { get; }
    }
}