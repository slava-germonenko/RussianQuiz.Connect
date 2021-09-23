using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

using Microsoft.IdentityModel.Tokens;

using RussianQuiz.Connect.Core.Models;
using RussianQuiz.Connect.Core.Services.Abstractions;
using RussianQuiz.Connect.Core.Settings;


namespace RussianQuiz.Connect.Core.Services
{
    public class JwtTokenService : ITokenService
    {
        private readonly IJwtTokenSettings _tokenSettings;

        private SecurityKey SecurityKey => new SymmetricSecurityKey(
            _tokenSettings.SecretEncoding.GetBytes(_tokenSettings.Secret)
        );


        public JwtTokenService(IJwtTokenSettings tokenSettings)
        {
            _tokenSettings = tokenSettings;
        }


        public UserToken CreateToken(User user)
        {
            DateTime? expireDate = _tokenSettings.Ttl is null
                ? null
                : DateTime.UtcNow.Add(_tokenSettings.Ttl.Value);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = CreateIdentity(user),
                Expires = expireDate,
                Issuer = _tokenSettings.Issuer,
                SigningCredentials = new SigningCredentials(SecurityKey, SecurityAlgorithms.HmacSha256Signature),
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return new UserToken(tokenString, expireDate);
        }

        public bool ValidateToken(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    IssuerSigningKey = SecurityKey,
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                }, out _);

                return true;
            }
            catch
            {
                return false;
            }
        }

        private ClaimsIdentity CreateIdentity(User user) => new (
            new[]
            {
                new Claim("email", user.EmailAddress)
            }
        );
    }
}