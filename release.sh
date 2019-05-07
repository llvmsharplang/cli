#!/bin/bash

# Define utility functions.
timestamp() {
  date +"%T"
}

# Invoke .NET Core.
dotnet publish -c Release

# Setup directories.
ROOT=$PWD
PUBLISH_PATH=bin/Release/netcoreapp2.2/publish
RELEASES_PATH=.releases
INSTALLERS_PATH=.installers

# Create the release output directory.
mkdir -p $RELEASES_PATH

# Copy installers and README to the publish directory.
cp $INSTALLERS_PATH/* $PUBLISH_PATH
cp README.md $PUBLISH_PATH

# Enter the publish path.
cd $PUBLISH_PATH

# Zip publish directory.
zip -r -9 $ROOT/$RELEASES_PATH/ion-cli-v0.0.0.$(timestamp).zip .

# Inform operation completed.
echo "Operation completed"
