@echo off
cd ../../
rmdir /s /q .packages
rmdir /s /q bin
rmdir /s /q obj
dotnet restore
dotnet build
cd .scripts/windows
