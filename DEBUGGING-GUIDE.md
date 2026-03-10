# Debugging Guide for RESTfulApiDemo

This guide addresses common debugging issues when working with the RESTfulApiDemo project in VS Code.

## Common Debugging Issues and Solutions

### Issue: "${vscodeDebugPath}" is not recognized as an internal or external command

**Problem**: This error occurs when using the C# Dev Kit extension with an older launch.json configuration that references the non-existent `vscodeDebugPath` variable.

**Solution**: The launch.json file has been updated to remove the problematic `pipeTransport` configuration that was causing this issue.

### Issue: Debugger not attaching properly

**Problem**: Sometimes the debugger fails to attach to the running process.

**Solution**:

1. Make sure you have the latest C# extension installed
2. Clean and rebuild the project: `Ctrl+Shift+B` then select "build"
3. Restart the debugging session

## Debugging Steps

1. **Ensure Extensions are Installed**:
   - C# (by Microsoft)
   - C# Extensions (by JosKreativ)
   - .NET Install Tool (by Microsoft)
   - .NET Core Tools (by Microsoft)

2. **Open the Project in VS Code**:
   - Open the root folder of the project in VS Code

3. **Build the Project**:
   - Press `Ctrl+Shift+B` and select "build"

4. **Start Debugging**:
   - Press `F5` or click the debug button
   - Select "Debug RESTful API" configuration

## Alternative Debugging Methods

### Method 1: Using the integrated terminal

1. Open the integrated terminal in VS Code (`Ctrl+`` `)
2. Navigate to the API project directory: `cd DVDStore.API`
3. Run: `dotnet run`

### Method 2: Using tasks

1. Press `Ctrl+Shift+P` to open the command palette
2. Type "Tasks: Run Task" and select it
3. Choose "run" to start the application

## Environment Variables

The project uses environment variables defined in:

- `.env` file in the DVDStore.API directory
- `appsettings.Development.json` file

Make sure these files are present and correctly configured.

## Troubleshooting

### If debugging still doesn't work

1. **Check .NET SDK**: Ensure .NET 8.0 SDK is installed
2. **Check Extensions**: Make sure all required C# extensions are installed and up to date
3. **Clear Cache**: Delete the `bin` and `obj` folders in all projects and rebuild
4. **Restart VS Code**: Sometimes a simple restart resolves extension issues

### Verify Installation

Run these commands in the integrated terminal to verify your setup:

```bash
dotnet --version
dotnet build
dotnet run
```

## Port Configuration

The API is configured to run on port `51000`:

- This matches the development settings in `appsettings.Development.json`
- Avoids conflicts with other common development ports
- Can be changed in `launch.json` if needed

## Logging

The application uses NLog for logging. You can view logs in:

- The integrated terminal when running the application
- The Output panel in VS Code (View → Output)
- Log files in the `logs` directory (if configured)
