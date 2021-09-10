# Russian Quiz Authorization Service

## Setting up local environment
Please note, this app uses .NET 5, so you'll need to install it first.

### Installing Azure CLI
To make azure functions work you need to install azure CLI first.

To install it macOS, you can use [homebrew](https://brew.sh) using the following `command brew update && brew install azure-cli`

To install in on Windows, you can use binary installer. You can find it on [Microsoft Azure CLI documentation](https://docs.microsoft.com/en-us/cli/azure/install-azure-cli-windows?tabs=azure-cli).

### Installing Azure functions core tools

You can install it quickly with npm NPM using the following command `npm i azure-functions-core-tools -g`

## Setting up configuration
Application is built around azure functions using .NET core 3.1, it uses dotnet-isolated runtime

Before running application, you may want to specify configuration. Application can pull configuration from 3 different source:
1. Local app settings file (e.g. local.settings.json, this file is added to .gitignore). To enable this option, you need to specify environment variable `LocalSettingsFile` which stores path (relative or absolute) to settings file.
2. Azure app configuration. To enable this option, you need to specify environment variable `AppConfigurationConnectionString` with connection string to azure app configuration. More information on that you can find on Microsoft official [documentation](https://docs.microsoft.com/en-us/azure/azure-app-configuration/overview)
3. Azure key vault. To enable this option you need to provide key vault URL via environment variable `KeyVaultUrl`. Also, if you're going to run app locally, you need to log in into your azure account using azure CLI. You can do that by running `az login` command

You can use settings.template.json file as reference to configuration required for app to run.


## Running app
Once you're done with setting up environment variable you can run `func start` command from command line.
Also you can do that using Rider or Visual Studio.
