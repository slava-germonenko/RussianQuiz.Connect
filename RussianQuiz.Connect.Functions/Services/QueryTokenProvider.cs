using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Options;
using RussianQuiz.Connect.Functions.Options;
using RussianQuiz.Connect.Functions.Services.Abstractions;
using RussianQuiz.Connect.Functions.Settings;


namespace RussianQuiz.Connect.Functions.Services
{
    public class QueryTokenProvider : IAuthorizationTokenProvider
    {
        private readonly AuthOptions _authOptions;


        public QueryTokenProvider(IOptionsSnapshot<AuthOptions> authSettings)
        {
            _authOptions = authSettings.Value;
        }


        public string GetToken(HttpRequestData request)
        {
            var queryParams = QueryHelpers.ParseQuery(request.Url.Query);
            return queryParams.TryGetValue(_authOptions.TokenQueryParamName, out var token)
                ? token.ToString()
                : null;
        }
    }
}