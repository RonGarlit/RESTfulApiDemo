@echo off
echo Setting up VS Code environment for RESTfulApiDemo...
echo.

powershell -ExecutionPolicy Bypass -File .\setup-vscode.ps1

echo.
echo Setup complete! You can now open the project in VS Code.
pause