using System;

using Azure.Extensions.AspNetCore.Configuration.Secrets;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;


namespace RussianQuiz.Auth.Functions
{
    public static class Program
    {
        public static void Main()
        {
            var host = new HostBuilder()
                .ConfigureFunctionsWorkerDefaults()
                .ConfigureServices(services =>
                {
                    var configuration = BuildConfiguration();
                    services.AddSingleton(configuration);
                })
                .Build();

            host.Run();
        }

        private static IConfigurationRoot BuildConfiguration()
        {
            IConfigurationBuilder configurationBuilder = new ConfigurationBuilder();

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

            return configurationBuilder.Build();
        }
    }
}