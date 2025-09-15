;Inno Setup Compiler 6.4.1

#define AppName "FunctionToGraph"
#define AppVersion "1.0"
#define AppPublisher "Ivan Zinchanka"
#define AppURL "https://github.com/ivan-zinchanka-dev/FunctionToGraph"
#define AppExeName "FunctionToGraph.exe"

[Setup]
AppId={{48F767B3-B632-48C0-821F-15F991F7B35D}
AppName={#AppName}
AppVersion={#AppVersion}
AppVerName={#AppName} {#AppVersion}
AppPublisher={#AppPublisher}
AppPublisherURL={#AppURL}
DefaultDirName={autopf}\{#AppName}
UninstallDisplayIcon={app}\{#AppExeName}
ArchitecturesAllowed=x86 x64compatible
ArchitecturesInstallIn64BitMode=x64compatible
DisableProgramGroupPage=yes
PrivilegesRequiredOverridesAllowed=dialog
OutputBaseFilename=func-to-graph-setup
SolidCompression=yes
WizardStyle=modern
DisableDirPage=no

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"
Name: "russian"; MessagesFile: "compiler:Languages\Russian.isl"

[Files]
Source: "..\FunctionToGraph\bin\Release\net6.0-windows\*.*"; DestDir: "{app}"; Flags: recursesubdirs createallsubdirs