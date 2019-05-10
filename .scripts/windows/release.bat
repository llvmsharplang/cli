@echo off
cd ..
echo Running tools installer script ...
Powershell.exe -executionpolicy remotesigned -File .\windows\install-tools.ps1
echo Make sure you have Inno Setup installed!
echo Invoking Inno Setup script compilation ...
echo Inno script invocation is not yet implemented! You'll have to do this manually.
cd windows
