#!/usr/bin/env pwsh
<#
.SYNOPSIS
    Build JERP 2.0 installer
.DESCRIPTION
    Builds the complete JERP solution and creates Windows installer
#>

param(
    [switch]$SkipBuild = $false,
    [switch]$SkipTests = $false
)

$ErrorActionPreference = "Stop"
$startTime = Get-Date

Write-Host "========================================" -ForegroundColor Cyan
Write-Host "JERP 2.0 Build & Installer Script" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

# Ensure we're in the right directory
$scriptDir = $PSScriptRoot
Set-Location $scriptDir

# Create output directories
$publishDir = Join-Path $scriptDir "publish"
$installerDir = Join-Path $scriptDir "installer"
$outputDir = Join-Path $installerDir "output"

Write-Host "Creating output directories..." -ForegroundColor Yellow
New-Item -ItemType Directory -Force -Path $publishDir | Out-Null
New-Item -ItemType Directory -Force -Path $installerDir | Out-Null
New-Item -ItemType Directory -Force -Path $outputDir | Out-Null

# Create LICENSE.txt and README.txt if missing
if (-not (Test-Path "LICENSE.txt")) {
    Write-Host "LICENSE.txt not found - creating placeholder..." -ForegroundColor Yellow
    "JERP 2.0 - Enterprise Resource Planning System`nCopyright (c) 2024" | Out-File "LICENSE.txt"
}

if (-not (Test-Path "README.txt")) {
    Write-Host "README.txt not found - creating placeholder..." -ForegroundColor Yellow
    "JERP 2.0 Installation Guide`nPlease see documentation for details." | Out-File "README.txt"
}

if (-not $SkipBuild) {
    # Clean solution
    Write-Host "Cleaning solution..." -ForegroundColor Yellow
    dotnet clean --configuration Release --verbosity quiet
    if ($LASTEXITCODE -ne 0) { throw "Clean failed" }

    # Restore packages
    Write-Host "Restoring NuGet packages..." -ForegroundColor Yellow
    dotnet restore
    if ($LASTEXITCODE -ne 0) { throw "Restore failed" }

    # Build solution
    Write-Host "Building solution in Release mode..." -ForegroundColor Yellow
    dotnet build --configuration Release --no-restore
    if ($LASTEXITCODE -ne 0) { throw "Build failed" }

    # Publish Desktop app
    Write-Host "Publishing Desktop application..." -ForegroundColor Yellow
    $desktopPublish = Join-Path $publishDir "Desktop"
    dotnet publish src/JERP.Desktop/JERP.Desktop.csproj `
        --configuration Release `
        --output $desktopPublish `
        --runtime win-x64 `
        --self-contained true `
        -p:PublishSingleFile=true `
        -p:IncludeNativeLibrariesForSelfExtract=true
    if ($LASTEXITCODE -ne 0) { throw "Desktop publish failed" }

    # Publish API app
    Write-Host "Publishing API application..." -ForegroundColor Yellow
    $apiPublish = Join-Path $publishDir "API"
    dotnet publish src/JERP.Api/JERP.Api.csproj `
        --configuration Release `
        --output $apiPublish `
        --runtime win-x64 `
        --self-contained true
    if ($LASTEXITCODE -ne 0) { throw "API publish failed" }

    Write-Host "Build completed successfully!" -ForegroundColor Green
}

# Verify published files
Write-Host "Verifying published files..." -ForegroundColor Yellow
$desktopExe = Join-Path $publishDir "Desktop/JERP.Desktop.exe"
$apiDll = Join-Path $publishDir "API/JERP.Api.dll"

if (-not (Test-Path $desktopExe)) {
    throw "Desktop executable not found: $desktopExe"
}
if (-not (Test-Path $apiDll)) {
    throw "API DLL not found: $apiDll"
}

Write-Host "✓ Desktop executable found" -ForegroundColor Green
Write-Host "✓ API application found" -ForegroundColor Green

# Check for Inno Setup
$innoSetup = "${env:ProgramFiles(x86)}\Inno Setup 6\ISCC.exe"
if (-not (Test-Path $innoSetup)) {
    Write-Host "WARNING: Inno Setup not found at $innoSetup" -ForegroundColor Yellow
    Write-Host "Skipping installer creation. Install Inno Setup 6 to create installer." -ForegroundColor Yellow
} else {
    Write-Host "Compiling Inno Setup installer..." -ForegroundColor Yellow
    $issFile = Join-Path $installerDir "JERP-Setup.iss"
    
    if (Test-Path $issFile) {
        & $innoSetup $issFile
        if ($LASTEXITCODE -eq 0) {
            Write-Host "✓ Installer created successfully!" -ForegroundColor Green
        } else {
            Write-Host "WARNING: Installer compilation failed" -ForegroundColor Yellow
        }
    } else {
        Write-Host "WARNING: Setup script not found: $issFile" -ForegroundColor Yellow
    }
}

# Summary
$endTime = Get-Date
$duration = $endTime - $startTime

Write-Host ""
Write-Host "========================================" -ForegroundColor Cyan
Write-Host "Build Summary" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host "Duration: $($duration.ToString('mm\:ss'))" -ForegroundColor White

if (Test-Path $desktopExe) {
    $desktopSize = (Get-Item $desktopExe).Length / 1MB
    Write-Host "Desktop: $desktopExe ($($desktopSize.ToString('F2')) MB)" -ForegroundColor White
}

if (Test-Path $apiDll) {
    $apiSize = (Get-ChildItem (Join-Path $publishDir "API") -Recurse | Measure-Object -Property Length -Sum).Sum / 1MB
    Write-Host "API: $apiPublish ($($apiSize.ToString('F2')) MB)" -ForegroundColor White
}

$installerExe = Join-Path $outputDir "JERP-Setup.exe"
if (Test-Path $installerExe) {
    $installerSize = (Get-Item $installerExe).Length / 1MB
    Write-Host "Installer: $installerExe ($($installerSize.ToString('F2')) MB)" -ForegroundColor Green
}

Write-Host ""
Write-Host "Build completed successfully!" -ForegroundColor Green
Write-Host "========================================" -ForegroundColor Cyan
