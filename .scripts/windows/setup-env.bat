@echo off

echo Setting up development environment ...

REM Setup environment variables used by other scripts.
SET VERSION="0.0.3-alpha"

REM Install tools.
Powershell.exe -ExecutionPolicy remotesigned -File .\install-tools.ps1
