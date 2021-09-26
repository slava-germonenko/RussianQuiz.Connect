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

        private readonly ITokenService _tokenService;


        public AuthenticationService(
            AuthContext authContext,
            IHashingTool hashingTool,
            ITokenService tokenService
        )
        {
            _authContext = authContext;
            _hashingTool = hashingTool;
            _tokenService = tokenService;
        }


        public async Task<UserToken> AuthenticateAsync(string emailAddressOrUserName, string password)
        {
            User user = await _authContext.Users.FirstOrDefaultAsync(
                u => u.EmailAddress == emailAddressOrUserName
                    || u.Username == emailAddressOrUserName
            );

            if (user is null || !_hashingTool.ValidateHash(password, user.PasswordHash))
            {
                throw new NotFoundException("User with such credentials was not found.");
            }

            return _tokenService.CreateToken(user);
        }
    }
}