using System.Linq;

using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Options;

using RussianQuiz.Connect.Functions.Services.Abstractions;
using RussianQuiz.Connect.Functions.Settings;


namespace RussianQuiz.Connect.Functions.Services
{
    public class CookieTokenProvider : IAuthorizationTokenProvider
    {
        private readonly AuthSettings _authSettings;


        public CookieTokenProvider(IOptionsSnapshot<AuthSettings> authSettings)
        {
            _authSettings = authSettings.Value;
        }

        public string GetToken(HttpRequestData request)
        {
            return request.Cookies.FirstOrDefault(c => c.Name == _authSettings.TokenCookieName)?.Value;
        }
    }
}