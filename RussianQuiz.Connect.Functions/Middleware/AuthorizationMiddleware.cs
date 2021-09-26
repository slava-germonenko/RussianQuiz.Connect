using System.Collections.Generic;
using System.Security.Authentication;
using System.Threading.Tasks;

using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.Functions.Worker.Middleware;

using RussianQuiz.Connect.Core.Services.Abstractions;
using RussianQuiz.Connect.Functions.Extensions;
using RussianQuiz.Connect.Functions.Services.Abstractions;


namespace RussianQuiz.Connect.Functions.Middleware
{
    public class AuthorizationMiddleware : IFunctionsWorkerMiddleware
    {
        private readonly IEnumerable<IAuthorizationTokenProvider> _tokenProviders;

        private readonly ITokenService _tokenService;


        public AuthorizationMiddleware(
            IEnumerable<IAuthorizationTokenProvider> tokenProviders,
            ITokenService tokenService
        )
        {
            _tokenProviders = tokenProviders;
            _tokenService = tokenService;
        }


        public Task Invoke(FunctionContext context, FunctionExecutionDelegate next)
        {
            var request = context.GetHttpRequestData();
            var token = GetToken(request);

            if (token is null)
            {
                throw new AuthenticationException("Authorization token is not provider.");
            }

            if (!_tokenService.ValidateToken(token))
            {
                throw new AuthenticationException("Authorization token is invalid.");
            }

            return Task.CompletedTask;
        }

        private string GetToken(HttpRequestData request)
        {
            foreach (var tokenProvider in _tokenProviders)
            {
                var token = tokenProvider.GetToken(request);
                if (token is not null)
                {
                    return token;
                }
            }

            return null;
        }
    }
}