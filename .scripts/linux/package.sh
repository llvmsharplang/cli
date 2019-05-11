#!/bin/bash

# Cleanup.
bash .scripts/linux/clean.sh

# Build Linux x64.
dotnet publish -c Release -r linux-x64

# Setup directories.
ROOT=$PWD
PUBLISH_PATH=bin/Release/netcoreapp2.2/linux-x64/publish
PACKAGES_PATH=.packages
INSTALLERS_PATH=.installers

# Create the packages output directory.
mkdir -p $PACKAGES_PATH

# Copy corresponding installer scripts and text files to the publish directory.
cp $INSTALLERS_PATH/installer.sh $PUBLISH_PATH
cp $INSTALLERS_PATH/*.txt $PUBLISH_PATH

# Enter the publish path.
cd $PUBLISH_PATH

# Zip publish directory.
tar -czf $ROOT/$PACKAGES_PATH/ion-cli-linux-x64-v0.0.0.tar.gz .

# Inform operation completed.
echo "Operation completed"
