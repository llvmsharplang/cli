@echo off

echo Setting up development environment ...

REM Install tools.
Powershell.exe -executionpolicy remotesigned -File .\install-tools.ps1
