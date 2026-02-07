# Building JERP Desktop Application

## Automated Builds

Every push to `main` or `develop` automatically triggers a build workflow that:

1. Builds the JERP.Desktop application
2. Creates a self-contained Windows executable
3. Packages everything into a ZIP file
4. Uploads as a downloadable artifact

## Downloading Artifacts

1. Go to the [Actions tab](../../actions)
2. Click on the latest workflow run
3. Scroll down to "Artifacts"
4. Download `JERP-Desktop-win-x64-Release.zip`
5. Extract and run `JERP.Desktop.exe`

## Manual Build

```bash
# Restore dependencies
dotnet restore src/JERP.Desktop/JERP.Desktop.csproj

# Build
dotnet build src/JERP.Desktop/JERP.Desktop.csproj --configuration Release

# Publish self-contained
dotnet publish src/JERP.Desktop/JERP.Desktop.csproj \
  --configuration Release \
  --runtime win-x64 \
  --self-contained true \
  --output ./publish/win-x64
```

## Requirements

- No .NET runtime required on target machine (self-contained)
- Windows 10/11 x64
- ~200MB disk space

## Configuration

Edit `appsettings.json` in the published folder to configure:
- API endpoint
- Connection strings
- Logging levels
