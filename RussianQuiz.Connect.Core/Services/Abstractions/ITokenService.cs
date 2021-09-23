using RussianQuiz.Connect.Core.Models;


namespace RussianQuiz.Connect.Core.Services.Abstractions
{
    public interface ITokenService
    {
        public UserToken CreateToken(User user);

        public bool ValidateToken(string token);
    }
}