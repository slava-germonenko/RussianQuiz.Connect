using Microsoft.Extensions.Options;

using RussianQuiz.Connect.Core.Settings;
using RussianQuiz.Connect.Functions.Options;


namespace RussianQuiz.Connect.Functions.Settings
{
    public class AuthContextSettings : IAuthContextSettings
    {
        private readonly InfrastructureOptions _infrastructureOptions;

        public string ConnectionString => _infrastructureOptions.CoreAzureSqlConnectionString;


        public AuthContextSettings(IOptionsSnapshot<InfrastructureOptions> infrastructureOptions)
        {
            _infrastructureOptions = infrastructureOptions.Value;
        }
    }
}