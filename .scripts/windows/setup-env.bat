@echo off

echo Setting up development environment ...

REM Install tools.
Powershell.exe -ExecutionPolicy remotesigned -File .\install-tools.ps1
