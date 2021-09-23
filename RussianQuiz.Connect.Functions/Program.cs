using System;

using Azure.Extensions.AspNetCore.Configuration.Secrets;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using RussianQuiz.Connect.Core;
using RussianQuiz.Connect.Core.Services;
using RussianQuiz.Connect.Core.Services.Abstractions;
using RussianQuiz.Connect.Functions.Middleware;
using RussianQuiz.Connect.Functions.Settings;


namespace RussianQuiz.Connect.Functions
{
    public static class Program
    {
        public static void Main()
        {
            var host = new HostBuilder()
                .ConfigureAppConfiguration(ApplyConfiguration)
                .ConfigureServices(ConfigureServices)
                .ConfigureFunctionsWorkerDefaults(Configure)
                .Build();
            host.Run();
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            IConfigurationRoot configuration = CreateConfiguration();
            services.Configure<AppSettings>(configuration);
            services.Configure<AuthSettings>(configuration.GetSection("Auth"));
            services.Configure<InfrastructureSettings>(configuration.GetSection("Infrastructure"));

            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<ITokenService, JwtTokenService>();

            services.AddDbContext<AuthContext>();
        }

        private static void Configure(IFunctionsWorkerApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionsMiddleware>();
        }

        private static void ApplyConfiguration(IConfigurationBuilder configurationBuilder)
        {
            string localSettingsFileName = Environment.GetEnvironmentVariable("LocalSettingsFile");
            if (!string.IsNullOrEmpty(localSettingsFileName))
            {
                configurationBuilder.AddJsonFile(localSettingsFileName);
            }

            string appConfigurationConnectionString = Environment.GetEnvironmentVariable("AppConfigurationConnectionString");
            if (!string.IsNullOrEmpty(appConfigurationConnectionString))
            {
                configurationBuilder.AddAzureAppConfiguration(appConfigurationConnectionString);
            }

            string keyVaultUrl = Environment.GetEnvironmentVariable("KeyVaultUrl");
            if (!string.IsNullOrEmpty(keyVaultUrl))
            {
                var secretClient = new SecretClient(
                    new Uri(keyVaultUrl),
                    new DefaultAzureCredential()
                );

                configurationBuilder.AddAzureKeyVault(secretClient, new KeyVaultSecretManager());
            }

            configurationBuilder.AddEnvironmentVariables();
        }

        private static IConfigurationRoot CreateConfiguration()
        {
            var configurationBuilder = new ConfigurationBuilder();
            ApplyConfiguration(configurationBuilder);
            return configurationBuilder.Build();
        }
    }
}