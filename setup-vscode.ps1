# VS Code Setup Script for RESTfulApiDemo
# Run this script to prepare your environment for VS Code development

Write-Host "Setting up VS Code environment for RESTfulApiDemo..." -ForegroundColor Green

# Install required VS Code extensions
Write-Host "Installing required VS Code extensions..." -ForegroundColor Yellow
code --install-extension ms-dotnettools.csharp
code --install-extension joskreativ.csharp-extensions
code --install-extension ms-dotnettools.vscode-dotnet-runtime
code --install-extension ms-dotnettools.csharpextensions

Write-Host "VS Code setup complete!" -ForegroundColor Green
Write-Host "You can now open the project in VS Code and start developing." -ForegroundColor Green