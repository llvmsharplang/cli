# Cleanup first.
"Cleaning up ..."
Start-Process -Wait -WindowStyle Hidden -FilePath cleanup.bat

# Setup environment.
"Setting up environment ..."
Start-Process -Wait -WindowStyle Hidden -FilePath setup-env.bat

# Navigate to root folder.
Set-Location ../../

# Build projects.
"Please note that builds will take longer the first time."

# Build Windows x86.
"Building project (Windows x86) ..."
dotnet publish -c Release -r win10-x86

# Build Windows x64.
"Building project (Windows x64) ..."
dotnet publish -c Release -r win10-x64

# Build Linux x64.
"Building project (Linux x64) ..."
dotnet publish -c Release -r linux-x64

# Package builds.
"Packaging builds ..."
New-Item -ItemType Directory -Force -Path .\.packages

# Package Windows x64.
"Packaging Windows x64 (through Inno Setup) ..."
Start-Process -Wait -WindowStyle Hidden -FilePath "$env:LOCALAPPDATA\Programs\Inno Setup 6\ISCC.exe" -ArgumentList InstallerScript.iss

# Package Linux x64.
"Packaging Linux x64 ..."
Copy-Item .\.installers\installer.sh .\bin\Release\netcoreapp2.2\linux-x64\publish\
Copy-Item .\.installers\*.txt .\bin\Release\netcoreapp2.2\linux-x64\publish\
Compress-Archive -CompressionLevel Optimal -Path .\bin\Release\netcoreapp2.2\linux-x64\publish\* -DestinationPath .\.packages\linux-x64.zip

# Finish up.
"Packaging completed."
Set-Location .\.scripts\windows
