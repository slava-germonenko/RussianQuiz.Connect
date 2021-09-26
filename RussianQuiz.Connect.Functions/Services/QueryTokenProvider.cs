using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Options;

using RussianQuiz.Connect.Functions.Services.Abstractions;
using RussianQuiz.Connect.Functions.Settings;


namespace RussianQuiz.Connect.Functions.Services
{
    public class QueryTokenProvider : IAuthorizationTokenProvider
    {
        private readonly AuthSettings _authSettings;


        public QueryTokenProvider(IOptionsSnapshot<AuthSettings> authSettings)
        {
            _authSettings = authSettings.Value;
        }


        public string GetToken(HttpRequestData request)
        {
            var queryParams = QueryHelpers.ParseQuery(request.Url.Query);
            return queryParams.TryGetValue(_authSettings.TokenQueryParamName, out var token)
                ? token.ToString()
                : null;
        }
    }
}