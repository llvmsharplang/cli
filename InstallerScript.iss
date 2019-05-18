#define Name "Ion Language"
#define Version "0.0.3-alpha"
#define Publisher "Atlas and Contributors"
#define Website "https://github.com/IonLanguage"

[Setup]
AppId={{2969D4B6-04A2-4792-ADD2-627DE35B5FDC}
AppName={#Name}
AppVersion={#Version}
AppPublisher={#Publisher}
AppPublisherURL={#Website}
AppSupportURL={#Website}
AppUpdatesURL={#Website}
DefaultDirName={autopf}\IonLanguage
DefaultGroupName={#Name}
PrivilegesRequired=lowest
PrivilegesRequiredOverridesAllowed=commandline
OutputDir=.packages
OutputBaseFilename=ion-cli-win-x64-{#Version}
SetupIconFile=Ion.ico
Compression=lzma
SolidCompression=yes
WizardStyle=modern

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Files]
Source: "IonCLI\bin\Release\netcoreapp2.2\win10-x64\publish\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs
Source: ".tools\*"; DestDir: "{app}\tools"; Flags: ignoreversion recursesubdirs createallsubdirs
Source: ".installers\installer.ps1"; DestDir: "{app}"; Flags: ignoreversion
Source: "DefaultPackage.xml"; DestDir: "{app}"; Flags: ignoreversion

[Run]
Filename: "powershell"; Parameters: "-ExecutionPolicy remotesigned -File {app}\installer.ps1"; Description: "Run post-installation script (recommended)"; Flags: postinstall
