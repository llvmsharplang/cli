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
    #[Environment]::SetEnvironmentVariable("Path", $env:Path + ";$path", [System.EnvironmentVariableTarget]::User)
}

function Prompt {
    param([string] $question)

    # Append options to the question.
    $question = "$question ? Yes or (N)o"

    # Invoke, and retrieve the answer.
    $answer = Read-Host $question

    # Return whether the answer contained a positive keyword.
    return "y", "Y", "yes", "Yes", "YES" -Contains $answer
}

function GetToolsUrl {
    param([string] $platformId)

    return "https://github.com/IonLanguage/Ion.CLI/releases/download/tools/$platformId.zip"
}

function GetToolsTempFilename {
    param([string] $platformId)

    return "tools.$platformId.zip"
}

function DownloadTools {
    param([string] $platformId)

    "($ToolCounter/$PlatformIdsLength) Downloading $platformId tools ..."

    # Resolve the tools download URL.
    $DownloadUrl = GetToolsUrl $platformId

    # Resolve the temporary zip filename.
    $TempFileName = GetToolsTempFilename $platformId

    # Download the tools package.
    $WebClient.DownloadFile($DownloadUrl, $TempFileName)

    "Extracting $platformId tools ..."

    # Extract the tools package.
    Expand-Archive -Force -Path $TempFileName -DestinationPath "$ToolsFolder\$platformId"

    # Remove tools package file.
    Remove-Item $TempFileName -ErrorAction Ignore
}

# Constant declaration.
$WebClient = New-Object System.Net.WebClient
$ToolsFolder = Resolve "../../.tools"
$FinishedMessage = "Tools installation completed."
$PlatformIds = "win64", "win32", "ubuntu16.04", "ubuntu14.04", "macOS", "debian8", "armv7a"
$PlatformIdsLength = $PlatformIds.Count
$ToolCounter = 1

# Do not continue if tools directory exists.
if (Test-Path $ToolsFolder) {
    Tools directory already exists.
    exit
}

"Tools will now be downloaded. This may take some time depending on your Internet speed. This process should only occur once."

# Download tools for all platforms.
foreach ($platformId in $PlatformIds) {
    DownloadTools $platformId
    $ToolCounter++
}

# Inform the user the process completed.
$FinishedMessage
Pause
