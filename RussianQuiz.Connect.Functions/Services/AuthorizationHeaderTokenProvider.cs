using System.Collections.Generic;
using System.Linq;

using Microsoft.Azure.Functions.Worker.Http;

using RussianQuiz.Connect.Functions.Services.Abstractions;


namespace RussianQuiz.Connect.Functions.Services
{
    public class AuthorizationHeaderTokenProvider : IAuthorizationTokenProvider
    {
        public string GetToken(HttpRequestData request)
        {
            if (request.Headers.TryGetValues("Authorization", out IEnumerable<string> authHeaders))
            {
                var bearerTokenAuthHeader = authHeaders.FirstOrDefault(h => h.StartsWith("Bearer"));
                return bearerTokenAuthHeader?.Replace("Bearer", "").Trim();
            }

            return null;
        }
    }
}