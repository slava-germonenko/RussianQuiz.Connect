using System.Threading.Tasks;

using RussianQuiz.Connect.Core.Models;


namespace RussianQuiz.Connect.Core.Services.Abstractions
{
    public interface IAuthenticationService
    {
        public Task<User> AuthenticateAsync(string emailAddressOrUserName, string password);
    }
}