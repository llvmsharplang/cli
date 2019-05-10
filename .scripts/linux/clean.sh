#!/bin/bash
rm -rf .packages
rm -rf bin
rm -rf obj
dotnet restore
dotnet build
