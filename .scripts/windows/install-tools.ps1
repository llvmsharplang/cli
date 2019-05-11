# Utility functions.
function Resolve {
    param([string] $path)

    # Return the resolved path.
    return $ExecutionContext.SessionState.Path.GetUnresolvedProviderPathFromPSPath($path)
}

function PathAppend {
    param([string] $path)

    # Ensure path is resolved.
    $path = Resolve-Path $path

    # Set the environment variable.
    [Environment]::SetEnvironmentVariable("Path", $env:Path + ";$path", [System.EnvironmentVariableTarget]::User)
}

# Constant declaration.
$ToolsUrl = "https://github.com/IonLanguage/Ion.CLI/releases/download/llvm-tools/tools.zip"
$ToolsZipFile = "llvm-tools.zip"
$ToolsFolder = Resolve "../../.llvm-tools"
$FinishedMessage = "Tools installation completed."

# LLVM tools.
if (Test-Path $ToolsFolder) {
    "LLVM tools directory already exists."
}
else {
    "Downloading LLVM tools ..."

    # Enforces TSL12 security protocol.
    [Net.ServicePointManager]::SecurityProtocol = [Net.SecurityProtocolType]::Tls12

    # Download the LLVM tools package.
    Invoke-WebRequest $ToolsUrl -OutFile $ToolsZipFile

    "Extracting LLVM tools ..."

    # Extract the LLVM tools package.
    Expand-Archive -path $ToolsZipFile -destinationpath $ToolsFolder

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
