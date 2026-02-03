@echo off
echo ========================================
echo JERP 2.0 Build Script
echo ========================================
echo.

REM Run PowerShell build script
powershell -ExecutionPolicy Bypass -File "%~dp0build-installer.ps1"

if %ERRORLEVEL% NEQ 0 (
    echo.
    echo Build failed!
    pause
    exit /b %ERRORLEVEL%
)

echo.
echo Build completed successfully!
pause
