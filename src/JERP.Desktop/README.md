# JERP Desktop

WPF Desktop Application for Just Employee Resource Planning (JERP) system.

## Features

- **Modern UI**: Built with ModernWPF and follows Material Design principles
- **MVVM Architecture**: Clean separation of concerns using CommunityToolkit.Mvvm
- **Dependency Injection**: Full DI container with Microsoft.Extensions.DependencyInjection
- **Secure Authentication**: Token-based authentication with credential management
- **Dashboard**: Real-time compliance metrics and violation tracking
- **Employee Management**: Full CRUD operations with pagination and search
- **Timesheet Management**: Time tracking with approval workflow
- **Payroll Processing**: Pay period management and pay stub generation
- **Compliance Monitoring**: Real-time violation tracking with filtering
- **AI Assistant**: Chat interface powered by Claude AI for accounting and compliance questions

## Prerequisites

- Windows 10/11
- .NET 8.0 SDK or later
- Visual Studio 2022 (recommended) or Rider

## Configuration

The application uses `appsettings.json` for configuration:

```json
{
  "Api": {
    "BaseUrl": "http://localhost:5000",
    "TimeoutSeconds": 30
  },
  "Theme": {
    "IsDark": true
  }
}
```

For production, use `appsettings.Production.json` to override settings.

## Building

```bash
dotnet build src/JERP.Desktop/JERP.Desktop.csproj
```

## Running

```bash
dotnet run --project src/JERP.Desktop/JERP.Desktop.csproj
```

Or open the solution in Visual Studio and press F5.

## Project Structure

```
JERP.Desktop/
├── Services/               # Business services
│   ├── IApiClient.cs      # HTTP client interface
│   ├── ApiClient.cs       # HTTP client implementation
│   ├── IAuthenticationService.cs
│   ├── AuthenticationService.cs
│   ├── IRegistryService.cs
│   └── RegistryService.cs # Windows Registry for settings
├── ViewModels/            # MVVM ViewModels
│   ├── ViewModelBase.cs
│   ├── LoginViewModel.cs
│   ├── MainViewModel.cs
│   ├── DashboardViewModel.cs
│   ├── EmployeesViewModel.cs
│   ├── TimesheetsViewModel.cs
│   ├── PayrollViewModel.cs
│   ├── ComplianceViewModel.cs
│   └── AIAssistantViewModel.cs
├── Views/                 # XAML Views
│   ├── LoginWindow.xaml
│   ├── MainWindow.xaml
│   ├── DashboardView.xaml
│   ├── EmployeesView.xaml
│   ├── TimesheetsView.xaml
│   ├── PayrollView.xaml
│   ├── ComplianceView.xaml
│   └── AIAssistantView.xaml
├── Converters/           # XAML Value Converters
│   ├── BooleanToVisibilityConverter.cs
│   ├── InverseBooleanConverter.cs
│   └── StringToVisibilityConverter.cs
├── App.xaml             # Application definition
├── App.xaml.cs          # Application startup & DI
├── app.manifest         # Windows manifest
└── appsettings.json     # Configuration

## Default Credentials

See the API documentation for default login credentials.

## Technologies

- **WPF** - Windows Presentation Foundation
- **ModernWPF 0.9.6** - Modern UI framework
- **CommunityToolkit.Mvvm 8.2.2** - MVVM toolkit
- **Microsoft.Extensions.DependencyInjection 8.0.0** - DI container
- **Microsoft.Extensions.Configuration.Json 8.0.0** - Configuration
- **Microsoft.Extensions.Http 8.0.0** - HTTP client factory
- **Newtonsoft.Json 13.0.3** - JSON serialization

## Features by View

### Dashboard
- Overall compliance score with color coding
- Violation counts by severity
- Active employee count
- Pending timesheet count
- Recent violations grid

### Employees
- Searchable employee list
- Pagination support
- Add/Edit/Delete operations
- Employee details panel

### Timesheets
- Date range filtering
- Employee filtering
- Status filtering
- Submit/Approve workflow
- Add/Edit/Delete operations

### Payroll
- Pay period selection by year
- Payroll processing
- Pay period approval
- Payroll record viewing
- Individual pay stub download (PDF)
- Bulk pay stub download (ZIP)

### Compliance
- Overall compliance score
- Violation list with severity color coding
- Filter by severity and status
- Resolve violations
- View violation details

### AI Assistant
- Chat interface with Claude AI
- Context-aware responses for accounting and compliance questions
- Message history with timestamps
- User messages styled in blue (right-aligned)
- Assistant messages styled in dark gray (left-aligned)
- Clear chat functionality
- Enter key to send messages
- "Thinking..." indicator while processing

## Registry Storage

Application settings are stored in Windows Registry at:
```
HKEY_CURRENT_USER\Software\JERP\Desktop
```

Stored values:
- `ApiUrl` - Custom API base URL
- `RememberedUsername` - Username for "Remember Me" feature

## License

Copyright © 2024 JERP. All rights reserved.
