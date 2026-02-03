#!/usr/bin/env pwsh
<#
.SYNOPSIS
    Verify build environment for JERP 2.0
.DESCRIPTION
    Checks all prerequisites before building
#>

$ErrorActionPreference = "Continue"
$allChecks = $true

Write-Host "========================================" -ForegroundColor Cyan
Write-Host "JERP 2.0 Build Verification" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

# Check .NET SDK
Write-Host "Checking .NET SDK..." -ForegroundColor Yellow
$dotnetVersion = dotnet --version 2>$null
if ($LASTEXITCODE -eq 0) {
    Write-Host "✓ .NET SDK $dotnetVersion installed" -ForegroundColor Green
} else {
    Write-Host "✗ .NET SDK not found" -ForegroundColor Red
    $allChecks = $false
}

# Check projects exist
Write-Host ""
Write-Host "Checking project files..." -ForegroundColor Yellow
$projects = @(
    "src/JERP.Core/JERP.Core.csproj",
    "src/JERP.Infrastructure/JERP.Infrastructure.csproj",
    "src/JERP.Compliance/JERP.Compliance.csproj",
    "src/JERP.Application/JERP.Application.csproj",
    "src/JERP.Api/JERP.Api.csproj",
    "src/JERP.Desktop/JERP.Desktop.csproj"
)

foreach ($proj in $projects) {
    if (Test-Path $proj) {
        Write-Host "✓ $proj" -ForegroundColor Green
    } else {
        Write-Host "✗ $proj not found" -ForegroundColor Red
        $allChecks = $false
    }
}

# Test NuGet restore
Write-Host ""
Write-Host "Testing NuGet restore..." -ForegroundColor Yellow
dotnet restore --verbosity quiet 2>$null
if ($LASTEXITCODE -eq 0) {
    Write-Host "✓ NuGet restore successful" -ForegroundColor Green
} else {
    Write-Host "✗ NuGet restore failed" -ForegroundColor Red
    $allChecks = $false
}

# Test Debug build
Write-Host ""
Write-Host "Testing Debug build..." -ForegroundColor Yellow
dotnet build --configuration Debug --no-restore --verbosity quiet 2>$null
if ($LASTEXITCODE -eq 0) {
    Write-Host "✓ Debug build successful" -ForegroundColor Green
} else {
    Write-Host "✗ Debug build failed" -ForegroundColor Red
    $allChecks = $false
}

# Test Release build
Write-Host ""
Write-Host "Testing Release build..." -ForegroundColor Yellow
dotnet build --configuration Release --no-restore --verbosity quiet 2>$null
if ($LASTEXITCODE -eq 0) {
    Write-Host "✓ Release build successful" -ForegroundColor Green
} else {
    Write-Host "✗ Release build failed" -ForegroundColor Red
    $allChecks = $false
}

# Check Inno Setup (optional)
Write-Host ""
Write-Host "Checking Inno Setup (optional)..." -ForegroundColor Yellow
$innoSetup = "${env:ProgramFiles(x86)}\Inno Setup 6\ISCC.exe"
if (Test-Path $innoSetup) {
    Write-Host "✓ Inno Setup 6 found" -ForegroundColor Green
} else {
    Write-Host "⚠ Inno Setup 6 not found (installer creation will be skipped)" -ForegroundColor Yellow
}

# Summary
Write-Host ""
Write-Host "========================================" -ForegroundColor Cyan
if ($allChecks) {
    Write-Host "✓ All checks passed - Ready to build!" -ForegroundColor Green
    exit 0
} else {
    Write-Host "✗ Some checks failed - Fix issues before building" -ForegroundColor Red
    exit 1
}
Write-Host "========================================" -ForegroundColor Cyan
