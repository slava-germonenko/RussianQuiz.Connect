using System.Linq;

using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Options;
using RussianQuiz.Connect.Functions.Options;
using RussianQuiz.Connect.Functions.Services.Abstractions;
using RussianQuiz.Connect.Functions.Settings;


namespace RussianQuiz.Connect.Functions.Services
{
    public class CookieTokenProvider : IAuthorizationTokenProvider
    {
        private readonly AuthOptions _authOptions;


        public CookieTokenProvider(IOptionsSnapshot<AuthOptions> authSettings)
        {
            _authOptions = authSettings.Value;
        }

        public string GetToken(HttpRequestData request)
        {
            return request.Cookies.FirstOrDefault(c => c.Name == _authOptions.TokenCookieName)?.Value;
        }
    }
}