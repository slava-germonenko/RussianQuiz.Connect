using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Options;

using RussianQuiz.Connect.Core.Models;
using RussianQuiz.Connect.Core.Services.Abstractions;
using RussianQuiz.Connect.Functions.Attributes;
using RussianQuiz.Connect.Functions.Extensions;
using RussianQuiz.Connect.Functions.Models;
using RussianQuiz.Connect.Functions.Models.Responses;
using RussianQuiz.Connect.Functions.Options;
using RussianQuiz.Connect.Functions.Settings;


namespace RussianQuiz.Connect.Functions.Triggers
{
    public class Authentication : BaseTrigger
    {
        private readonly IAuthenticationService _authenticationService;

        private readonly AuthOptions _authOptions;


        public Authentication(
            IAuthenticationService authenticationService,
            IOptionsSnapshot<AuthOptions> authSettingsOptions
        )
        {
            _authenticationService = authenticationService;
            _authOptions = authSettingsOptions.Value;
        }


        [Function("Authentication")]
        [RequireTokenAuthorization]
        public async Task<HttpResponseData> AuthenticateAsync(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "auth/login")] [FromBody] HttpRequestData req
        )
        {
            var response = req.CreateResponse();

            if (!req.TryGetBody<UserAuthData>(out var authData))
            {
                response.StatusCode = HttpStatusCode.BadRequest;
                response.SetBody(new BaseErrorResponse("Unable to retrieve user credentials."));
                return response;
            }

            UserToken userToken = await _authenticationService.AuthenticateAsync(
                authData.Username,
                authData.Password
            );

            var authCookie = CreateAuthorizationCookie(response, userToken.TokenValue, userToken.ExpiresAt);
            response.Cookies.Append(authCookie);

            response.SetBody(userToken);
            return response;
        }

        [Function("Logout")]
        public HttpResponseData Logout(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "auth/logout")] HttpRequestData req
        )
        {
            var response = req.CreateResponse();
            response.StatusCode = HttpStatusCode.NoContent;

            var currentToken = req.Cookies
                .FirstOrDefault(c => c.Name == _authOptions.TokenCookieName)
                ?.Value;

            if (string.IsNullOrEmpty(currentToken))
            {
                return response;
            }

            var authCookie = CreateAuthorizationCookie(response, currentToken, DateTime.UtcNow.AddDays(-1));
            response.Cookies.Append(authCookie);

            return response;
        }

        private IHttpCookie CreateAuthorizationCookie(HttpResponseData responseData, string value, DateTime? expiresAt)
        {
            return new HttpCookie(_authOptions.TokenCookieName, value)
            {
                Expires = expiresAt,
                Domain = _authOptions.TokenCookieDomain,
                HttpOnly = _authOptions.HttpOnlyTokenCookie,
                Secure = _authOptions.SecureTokenCookie,
            };
        }
    }
}