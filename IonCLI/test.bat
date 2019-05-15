@echo off
dotnet run -- build -dv -t ../.llvm-tools -r .test -o .test/bin
