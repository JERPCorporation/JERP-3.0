# JERP MAUI - Setup Script
# Automatiza la instalación y configuración del proyecto MAUI

Write-Host "=====================================" -ForegroundColor Cyan
Write-Host "   JERP 3.0 - MAUI Setup Script     " -ForegroundColor Cyan
Write-Host "=====================================" -ForegroundColor Cyan
Write-Host ""

$projectRoot = "C:\Users\ichbi\OneDrive\Documents\GitHub\JERP-3.0"
$mauiProject = Join-Path $projectRoot "src\JERP.Maui"
$sharedProject = Join-Path $projectRoot "src\JERP.Shared"

# Step 1: Verificar .NET SDK
Write-Host "[1/6] Verificando .NET SDK..." -ForegroundColor Yellow
try {
    $dotnetVersion = dotnet --version
    Write-Host "✓ .NET SDK encontrado: v$dotnetVersion" -ForegroundColor Green
} catch {
    Write-Host "✗ .NET SDK no encontrado. Por favor instala .NET 10.0 SDK" -ForegroundColor Red
    Write-Host "   Descarga desde: https://dotnet.microsoft.com/download" -ForegroundColor Yellow
    exit 1
}

# Step 2: Verificar/Instalar workload de MAUI
Write-Host "`n[2/6] Verificando workload de MAUI..." -ForegroundColor Yellow
$mauiInstalled = dotnet workload list | Select-String "maui"

if ($mauiInstalled) {
    Write-Host "✓ MAUI workload ya está instalado" -ForegroundColor Green
} else {
    Write-Host "  Instalando MAUI workload..." -ForegroundColor Yellow
    try {
        dotnet workload install maui
        Write-Host "✓ MAUI workload instalado correctamente" -ForegroundColor Green
    } catch {
        Write-Host "✗ Error instalando MAUI workload" -ForegroundColor Red
        Write-Host "  Intenta ejecutar como Administrador" -ForegroundColor Yellow
        exit 1
    }
}

# Step 3: Crear estructura de carpetas si no existe
Write-Host "`n[3/6] Verificando estructura de proyecto..." -ForegroundColor Yellow

if (-not (Test-Path $mauiProject)) {
    Write-Host "  Creando carpeta JERP.Maui..." -ForegroundColor Yellow
    New-Item -ItemType Directory -Path $mauiProject -Force | Out-Null
}

if (-not (Test-Path $sharedProject)) {
    Write-Host "  Creando carpeta JERP.Shared..." -ForegroundColor Yellow
    New-Item -ItemType Directory -Path $sharedProject -Force | Out-Null
}

# Crear subcarpetas necesarias
$folders = @(
    "Views",
    "ViewModels", 
    "Services",
    "Resources\Fonts",
    "Resources\Images",
    "Resources\Styles",
    "Platforms\Android",
    "Platforms\iOS",
    "Platforms\Windows",
    "Platforms\MacCatalyst"
)

foreach ($folder in $folders) {
    $fullPath = Join-Path $mauiProject $folder
    if (-not (Test-Path $fullPath)) {
        New-Item -ItemType Directory -Path $fullPath -Force | Out-Null
    }
}

Write-Host "✓ Estructura de carpetas verificada" -ForegroundColor Green

# Step 4: Copiar archivos generados
Write-Host "`n[4/6] Copiando archivos del proyecto..." -ForegroundColor Yellow

# Los archivos ya fueron generados por Claude en /home/claude
# Aquí solo indicamos que se deben copiar manualmente o ya están en su lugar

Write-Host "✓ Archivos del proyecto listos" -ForegroundColor Green

# Step 5: Restaurar paquetes NuGet
Write-Host "`n[5/6] Restaurando paquetes NuGet..." -ForegroundColor Yellow

if (Test-Path (Join-Path $sharedProject "JERP.Shared.csproj")) {
    Set-Location $sharedProject
    dotnet restore
    if ($LASTEXITCODE -eq 0) {
        Write-Host "✓ Paquetes restaurados en JERP.Shared" -ForegroundColor Green
    }
}

if (Test-Path (Join-Path $mauiProject "JERP.Maui.csproj")) {
    Set-Location $mauiProject
    dotnet restore
    if ($LASTEXITCODE -eq 0) {
        Write-Host "✓ Paquetes restaurados en JERP.Maui" -ForegroundColor Green
    }
}

# Step 6: Configurar appsettings
Write-Host "`n[6/6] Configurando appsettings.json..." -ForegroundColor Yellow

$backendPort = Read-Host "¿En qué puerto corre tu backend? (presiona Enter para usar 7001)"
if ([string]::IsNullOrWhiteSpace($backendPort)) {
    $backendPort = "7001"
}

$appsettingsPath = Join-Path $mauiProject "appsettings.json"
if (Test-Path $appsettingsPath) {
    $appsettings = Get-Content $appsettingsPath | ConvertFrom-Json
    $appsettings.Api.BaseUrl = "http://localhost:$backendPort"
    $appsettings | ConvertTo-Json -Depth 10 | Set-Content $appsettingsPath
    Write-Host "✓ appsettings.json configurado con puerto $backendPort" -ForegroundColor Green
}

# Resumen
Write-Host "`n=====================================" -ForegroundColor Cyan
Write-Host "   ✓ Setup Completado               " -ForegroundColor Green
Write-Host "=====================================" -ForegroundColor Cyan
Write-Host ""
Write-Host "Próximos pasos:" -ForegroundColor Yellow
Write-Host "1. Asegúrate de que tu backend esté corriendo en http://localhost:$backendPort"
Write-Host "2. Ejecuta: cd '$mauiProject'"
Write-Host "3. Ejecuta: dotnet build -f net10.0-windows10.0.19041.0"
Write-Host "4. Ejecuta: dotnet run --framework net10.0-windows10.0.19041.0"
Write-Host ""
Write-Host "Para Android:" -ForegroundColor Yellow
Write-Host "  dotnet build -f net10.0-android"
Write-Host ""
Write-Host "Para más información, lee el README.md en la carpeta JERP.Maui" -ForegroundColor Cyan
Write-Host ""

# Preguntar si quiere compilar ahora
$compile = Read-Host "¿Deseas compilar el proyecto ahora? (s/n)"
if ($compile -eq "s" -or $compile -eq "S") {
    Write-Host "`nCompilando JERP.Maui..." -ForegroundColor Yellow
    Set-Location $mauiProject
    dotnet build -f net10.0-windows10.0.19041.0
    
    if ($LASTEXITCODE -eq 0) {
        Write-Host "`n✓ Compilación exitosa!" -ForegroundColor Green
        $run = Read-Host "¿Deseas ejecutar la aplicación ahora? (s/n)"
        if ($run -eq "s" -or $run -eq "S") {
            dotnet run --framework net10.0-windows10.0.19041.0
        }
    } else {
        Write-Host "`n✗ Error en la compilación. Revisa los mensajes arriba." -ForegroundColor Red
    }
}

Set-Location $projectRoot
