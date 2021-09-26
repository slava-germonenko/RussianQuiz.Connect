using Microsoft.Azure.Functions.Worker.Http;


namespace RussianQuiz.Connect.Functions.Services.Abstractions
{
    public interface IAuthorizationTokenProvider
    {
        public string GetToken(HttpRequestData request);
    }
}