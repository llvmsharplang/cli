# Cleanup first.
"Cleaning up ..."
Start-Process -Wait -WindowStyle Hidden -FilePath "cleanup.bat"

# Setup environment.
"Setting up environment ..."
Start-Process -Wait -WindowStyle Hidden -FilePath "setup-env.bat"

# Define globals.
$UnixLikePlatforms = "linux", "macOS", "armv7a"
$UnixLikePlatformPublishMap = "linux-x64", "osx-x64", "linux-arm"
$PublishMapCounter = 0

# Navigate to main project's folder.
Set-Location "../../IonCLI"

# Build projects.
"Please note that builds will take longer the first time."

# Build Windows x86.
"Building project (Windows x86) ..."
dotnet publish -c Release -r win10-x86

# Build Windows x64.
"Building project (Windows x64) ..."
dotnet publish -c Release -r win10-x64

# Navigate to root folder.
Set-Location "../"

# Package builds.
"Packaging builds ..."
New-Item -ItemType Directory -Force -Path ".packages"

# Package Windows x64.
"Packaging Windows x64 (through Inno Setup) ..."
Start-Process -Wait -WindowStyle Hidden -FilePath "$env:LOCALAPPDATA\Programs\Inno Setup 6\ISCC.exe" -ArgumentList "InstallerScript.iss"

# TODO: Windows x86?

# Process Unix-like platforms.
foreach ($platformId in $UnixLikePlatforms) {
    # Compute publish id.
    $publishId = $UnixLikePlatformPublishMap[$PublishMapCounter]

    # Build.
    dotnet publish -c Release -r $publishId

    # Package.
    "Packaging $platformId ..."
    
    Copy-Item ".installers\INSTALL.sh" ".\IonCLI\bin\Release\netcoreapp2.2\$publishId\publish\"
    Copy-Item "DefaultPackage.xml" ".\IonCLI\bin\Release\netcoreapp2.2\$publishId\publish\"
    Copy-Item ".installers\*.txt" ".\IonCLI\bin\Release\netcoreapp2.2\$publishId\publish\"
    Compress-Archive -CompressionLevel Optimal -Path ".\IonCLI\bin\Release\netcoreapp2.2\$publishId\publish\*" -DestinationPath ".packages\$platformId.zip"
    $PublishMapCounter++
}

# Finish up.
"Packaging completed."
Set-Location ".scripts\windows"
Pause
