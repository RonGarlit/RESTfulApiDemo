# VS Code Configuration for RESTfulApiDemo

This document explains the VS Code configuration changes made to ensure seamless development across the team.

## Configuration Files

### 1. `.vscode/launch.json`

- Configures debugging for the API project
- Sets the application URL to `http://localhost:51000` for consistency with development settings
- Sets environment to `Development`
- Uses the correct .NET runtime path

### 2. `.vscode/tasks.json`

- Defines build tasks for both Debug and Release configurations
- Includes a run task for quick execution
- Configures problem matchers for error detection

### 3. `.vscode/settings.json`

- Configures OmniSharp for better C# support
- Excludes build artifacts from file explorer
- Sets up proper IntelliSense for C#

### 4. `DVDStore.API/.env`

- Contains environment variables for development
- Sets ASPNETCORE_URLS to `http://localhost:51000`

## Key Changes Made

1. **Port Consistency**: Changed the application URL from `http://localhost:5000` to `http://localhost:51000` to match the development settings in `appsettings.Development.json`

2. **Launch URL**: Changed from `weatherforecast` to `swagger` for easier API exploration

3. **Environment Variables**: Ensured consistent environment setup between Visual Studio and VS Code

## Team Usage Instructions

1. Install the required VS Code extensions:
   - C# (by Microsoft)
   - C# Extensions (by JosKreativ)
   - .NET Install Tool (by Microsoft)
   - .NET Core Tools (by Microsoft)

2. Run the setup script:
   - Windows: Execute `setup-vscode.bat`
   - Or run `setup-vscode.ps1` directly

3. Open the project in VS Code and use:
   - `Ctrl+F5` to run without debugging
   - `F5` to debug
   - `Ctrl+Shift+B` to build

## Port Configuration

The API is configured to run on port `51000` which:

- Matches the development settings in `appsettings.Development.json`
- Avoids conflicts with other common development ports
- Is consistent across both Visual Studio and VS Code environments
