# JERP 3.0 - MAUI Native Application

Aplicación móvil y de escritorio multiplataforma construida con .NET MAUI.

## 🚀 Plataformas Soportadas

- ✅ **Windows** (10/11)
- ✅ **macOS** (Catalyst)
- ✅ **iOS** (11.0+)
- ✅ **Android** (API 21+)

## 📋 Requisitos Previos

### Desarrollo
- .NET 10.0 SDK o superior
- Visual Studio 2022 17.12+ o Visual Studio Code
- Cargas de trabajo de MAUI instaladas

### Windows
```powershell
# Instalar carga de trabajo de MAUI
dotnet workload install maui
```

### macOS
```bash
# Instalar carga de trabajo de MAUI
sudo dotnet workload install maui
sudo dotnet workload install maui-ios
sudo dotnet workload install maui-maccatalyst
```

## 🛠️ Configuración del Proyecto

### 1. Clonar/Ubicar el proyecto

El proyecto MAUI debe estar en:
```
JERP-3.0/
├── src/
│   ├── JERP.Api/           # Backend existente
│   ├── JERP.Desktop/       # WPF existente
│   ├── JERP.Maui/          # ← Nueva app MAUI
│   └── JERP.Shared/        # ← DTOs compartidos
└── landing-page/           # Next.js existente
```

### 2. Restaurar dependencias

```powershell
cd C:\Users\ichbi\OneDrive\Documents\GitHub\JERP-3.0\src\JERP.Maui
dotnet restore
```

### 3. Configurar appsettings.json

Edita `appsettings.json` con la URL de tu backend:

```json
{
  "Api": {
    "BaseUrl": "http://localhost:7001"  // Tu puerto del backend
  }
}
```

### 4. Para desarrollo en Android (opcional)

Si estás en Windows y quieres probar Android:

```powershell
# Verificar que Android SDK esté instalado
dotnet build -t:Run -f net10.0-android
```

### 5. Para desarrollo en iOS (solo macOS)

```bash
# Desde macOS
dotnet build -t:Run -f net10.0-ios
```

## 🏃 Ejecutar la Aplicación

### Windows (recomendado para empezar)

```powershell
cd src\JERP.Maui
dotnet build -f net10.0-windows10.0.19041.0
dotnet run --framework net10.0-windows10.0.19041.0
```

O desde Visual Studio:
1. Abrir la solución `JERP.slnx`
2. Establecer `JERP.Maui` como proyecto de inicio
3. Seleccionar framework: `net10.0-windows`
4. Presionar F5

### Android

```powershell
dotnet build -f net10.0-android
dotnet run --framework net10.0-android
```

### iOS (macOS solamente)

```bash
dotnet build -f net10.0-ios
dotnet run --framework net10.0-ios
```

## 📱 Estructura del Proyecto

```
JERP.Maui/
├── Views/              # Páginas XAML
│   ├── LoginPage.xaml
│   ├── MainPage.xaml
│   └── DashboardPage.xaml
├── ViewModels/         # ViewModels con MVVM
│   ├── LoginViewModel.cs
│   └── MainViewModel.cs
├── Services/           # Servicios de negocio
│   ├── IAuthenticationService.cs
│   ├── AuthenticationService.cs
│   └── ApiInterfaces.cs
├── Resources/          # Recursos de la app
│   ├── Fonts/
│   ├── Images/
│   └── Styles/
├── Platforms/          # Código específico por plataforma
│   ├── Android/
│   ├── iOS/
│   ├── MacCatalyst/
│   └── Windows/
├── MauiProgram.cs      # Configuración DI
└── App.xaml            # App principal
```

## 🎨 Tecnologías Utilizadas

- **MAUI Native** - Framework multiplataforma
- **CommunityToolkit.Maui** - Componentes adicionales
- **CommunityToolkit.Mvvm** - MVVM Toolkit
- **Refit** - Cliente HTTP type-safe
- **System.IdentityModel.Tokens.Jwt** - Manejo de JWT

## 🔐 Autenticación

La app usa JWT tokens para autenticación:

1. El usuario ingresa credenciales
2. Se obtiene access token + refresh token
3. Tokens se guardan de forma segura en `SecureStorage`
4. `AuthHeaderHandler` inyecta el token automáticamente en todas las peticiones HTTP
5. Si el token expira, se renueva automáticamente

### Credenciales de Prueba

```
Usuario: admin
Contraseña: Admin@123
```

## 🌐 Configuración de Red

### Android - Permitir HTTP en desarrollo

Edita `Platforms/Android/Resources/xml/network_security_config.xml`:

```xml
<?xml version="1.0" encoding="utf-8"?>
<network-security-config>
    <base-config cleartextTrafficPermitted="true" />
</network-security-config>
```

### iOS - Configurar ATS

Edita `Platforms/iOS/Info.plist` si usas HTTP:

```xml
<key>NSAppTransportSecurity</key>
<dict>
    <key>NSAllowsArbitraryLoads</key>
    <true/>
</dict>
```

## 🐛 Troubleshooting

### Error: "No se puede conectar al backend"

1. Verifica que el backend esté corriendo
2. En Android Emulator usa `10.0.2.2` en lugar de `localhost`
3. En iOS Simulator puedes usar `localhost`

```json
// appsettings.json para Android
{
  "Api": {
    "BaseUrl": "http://10.0.2.2:7001"  // Android emulator
  }
}
```

### Error: "Workload not found"

```powershell
dotnet workload restore
dotnet workload install maui
```

### Error de compilación en Android

```powershell
# Limpiar y reconstruir
dotnet clean
dotnet build -f net10.0-android
```

## 📦 Publicar la Aplicación

### Windows (MSIX)

```powershell
dotnet publish -f net10.0-windows10.0.19041.0 -c Release
```

### Android (APK)

```powershell
dotnet publish -f net10.0-android -c Release
```

El APK estará en: `bin/Release/net10.0-android/publish/`

### iOS (IPA - requiere certificado de Apple)

```bash
dotnet publish -f net10.0-ios -c Release
```

## 🔄 Migración desde WPF

Si ya tienes código en WPF (`JERP.Desktop`), mucho puede reutilizarse:

1. **ViewModels** - La mayoría son compatibles (usa CommunityToolkit.Mvvm)
2. **Servicios** - 100% reutilizables si no dependen de WPF
3. **XAML** - Similar pero con diferencias (ContentPage vs Window)

## 🚢 Despliegue

### App Stores

- **Google Play**: Necesitas cuenta de desarrollador ($25 único)
- **Apple App Store**: Necesitas cuenta de desarrollador ($99/año)
- **Microsoft Store**: Incluido con cuenta de desarrollador

### Distribución interna (Enterprise)

Puedes distribuir directamente sin app stores:
- Android: Instalar APK directamente
- iOS: Usar TestFlight o MDM
- Windows: Instalar MSIX

## 📞 Soporte

Para problemas o preguntas:
- Email: ichbincesartobar@yahoo.com
- Documentación MAUI: https://learn.microsoft.com/dotnet/maui

## 📄 Licencia

Copyright (c) 2026 Julio Cesar Mendez Tobar. All Rights Reserved.
