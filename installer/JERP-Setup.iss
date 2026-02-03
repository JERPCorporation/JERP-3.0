; JERP 2.0 Installer Script for Inno Setup 6

#define MyAppName "JERP 2.0"
#define MyAppVersion "2.0.0"
#define MyAppPublisher "JERP Corporation"
#define MyAppURL "https://www.jerp.com"
#define MyAppExeName "JERP.Desktop.exe"

[Setup]
AppId={{8A7B6C5D-4E3F-2A1B-9C8D-7E6F5A4B3C2D}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
AppPublisher={#MyAppPublisher}
AppPublisherURL={#MyAppURL}
AppSupportURL={#MyAppURL}
AppUpdatesURL={#MyAppURL}
DefaultDirName={autopf}\JERP
DefaultGroupName=JERP 2.0
AllowNoIcons=yes
LicenseFile=..\LICENSE.txt
InfoBeforeFile=..\README.txt
OutputDir=output
OutputBaseFilename=JERP-Setup
Compression=lzma
SolidCompression=yes
WizardStyle=modern
PrivilegesRequired=admin
ArchitecturesAllowed=x64
ArchitecturesInstallIn64BitMode=x64

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Components]
Name: "desktop"; Description: "Desktop Application"; Types: full; Flags: fixed
Name: "api"; Description: "API Server"; Types: full

[Files]
Source: "..\publish\Desktop\*"; DestDir: "{app}\Desktop"; Flags: ignoreversion recursesubdirs; Components: desktop
Source: "..\publish\API\*"; DestDir: "{app}\API"; Flags: ignoreversion recursesubdirs; Components: api
Source: "..\LICENSE.txt"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\README.txt"; DestDir: "{app}"; Flags: ignoreversion

[Icons]
Name: "{group}\JERP Desktop"; Filename: "{app}\Desktop\{#MyAppExeName}"; Components: desktop
Name: "{autodesktop}\JERP Desktop"; Filename: "{app}\Desktop\{#MyAppExeName}"; Components: desktop

[Registry]
Root: HKCU; Subkey: "Software\JERP\Desktop"; ValueType: string; ValueName: "InstallPath"; ValueData: "{app}"; Flags: uninsdeletevalue
Root: HKCU; Subkey: "Software\JERP\Desktop"; ValueType: string; ValueName: "Version"; ValueData: "{#MyAppVersion}"; Flags: uninsdeletevalue

[Run]
Filename: "{app}\Desktop\{#MyAppExeName}"; Description: "Launch JERP Desktop"; Flags: nowait postinstall skipifsilent; Components: desktop
