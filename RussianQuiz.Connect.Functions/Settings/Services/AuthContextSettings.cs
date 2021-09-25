using Microsoft.Extensions.Options;

using RussianQuiz.Connect.Core.Settings;


namespace RussianQuiz.Connect.Functions.Settings.Services
{
    public class AuthContextSettings : IAuthContextSettings
    {
        private readonly InfrastructureSettings _infrastructureSettings;

        public string ConnectionString => _infrastructureSettings.CoreAzureSqlConnectionString;


        public AuthContextSettings(IOptionsSnapshot<InfrastructureSettings> infrastructureSettings)
        {
            _infrastructureSettings = infrastructureSettings.Value;
        }
    }
}