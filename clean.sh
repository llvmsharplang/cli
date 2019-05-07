#!/bin/bash
rm -rf .releases
rm -rf bin
rm -rf obj
dotnet restore
dotnet build
