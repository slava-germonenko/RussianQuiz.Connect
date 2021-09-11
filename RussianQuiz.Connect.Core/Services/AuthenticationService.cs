using System;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using RussianQuiz.Connect.Core.Exceptions;
using RussianQuiz.Connect.Core.Models;
using RussianQuiz.Connect.Core.Services.Abstractions;


namespace RussianQuiz.Connect.Core.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly AuthContext _authContext;

        private readonly IHashingTool _hashingTool;


        public AuthenticationService(AuthContext authContext, IHashingTool hashingTool)
        {
            _authContext = authContext;
            _hashingTool = hashingTool;
        }


        public async Task<User> AuthenticateAsync(string emailAddressOrUserName, string password)
        {
            User user = await _authContext.Users.FirstOrDefaultAsync(
                u => u.EmailAddress.Equals(emailAddressOrUserName, StringComparison.OrdinalIgnoreCase)
                    || u.Username.Equals(emailAddressOrUserName, StringComparison.OrdinalIgnoreCase)
            );

            if (user is null || !_hashingTool.ValidateHash(password, user.PasswordHash))
            {
                throw new NotFoundException($"User with such credentials was not found.");
            }

            return user;
        }
    }
}