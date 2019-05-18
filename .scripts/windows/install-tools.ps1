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
$ToolsUrl = "https://github.com/IonLanguage/Ion.CLI/releases/download/tools/tools.zip"
$ToolsZipFile = "tools.zip"
$ToolsFolder = Resolve "../../.tools"
$FinishedMessage = "Tools installation completed."

# Tools.
if (Test-Path $ToolsFolder) {
    "Tools directory already exists."
}
else {
    "Downloading tools ..."

    # Enforce TSL12 security protocol.
    [Net.ServicePointManager]::SecurityProtocol = [Net.SecurityProtocolType]::Tls12

    # Download the tools package.
    Invoke-WebRequest $ToolsUrl -OutFile $ToolsZipFile

    "Extracting tools ..."

    # Extract the tools package.
    Expand-Archive -path $ToolsZipFile -destinationpath $ToolsFolder

    "Appending tools to path ..."

    # Append tools to path.
    PathAppend $ToolsFolder
}

# Cleanup.
"Cleaning up ..."

# Remove tools package file.
Remove-Item $ToolsZipFile -ErrorAction Ignore

# Inform the user the process completed.
$FinishedMessage
