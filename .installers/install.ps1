# Constant declaration.
$ToolsUrl = "https://github.com/IonLanguage/Ion.CLI/releases/download/llvm-tools-1/tools.zip"
$ToolsZipFile = "llvm-tools.zip"
$ToolsFolder = "llvm-tools"
$FinishedMessage = "You're all set!"

# Utility functions.
function PathAppend {
    param([string] $path)

    # Ensure path is resolved.
    $path = Resolve-Path $path

    # Set the environment variable.
    [Environment]::SetEnvironmentVariable("Path", $env:Path + ";$path", [System.EnvironmentVariableTarget]::User)
}

# LLVM tools.
if (Test-Path $ToolsFolder) {
    "LLVM tools directory already exists in the current directory."
}
else {
    "Downloading LLVM tools ..."

    # Download the LLVM tools package.
    Invoke-WebRequest $ToolsUrl -OutFile $ToolsZipFile

    "Extracting LLVM tools ..."

    # Extract the LLVM tools package.
    expand-archive -path $ToolsZipFile -destinationpath $ToolsFolder

    "Appending LLVM tools to path ..."

    # Append tools to path.
    PathAppend $ToolsFolder
}

# Cleanup.
"Cleaning up ..."

# Remove LLVM tools package file.
Remove-Item $ToolsZipFile -ErrorAction Ignore

# Inform the user the process completed.
$FinishedMessage
