using System;

using Azure.Extensions.AspNetCore.Configuration.Secrets;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;


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

        private static void ConfigureServices(IServiceCollection services)
        {
        }

        private static void Configure(IFunctionsWorkerApplicationBuilder app)
        {
        }
    }
}