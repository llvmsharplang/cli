#!/bin/bash

# Abort script on failure.
set -e

# Prepare root directory.
ROOT=$PWD

# Use environment root path if applicable.
if [ ! -z "$ION_ROOT" ]; then
    ROOT=$ION_ROOT
    echo "Using environment root path: $ROOT"
else
    echo "Using root path: $ROOT"
fi

# Verify root path.
if [ ! -d "$ROOT" ]; then
    echo "Error: Root path does not exist"
    exit 1
fi

# Define paths.
BASE_PATH=/opt/ioncli
EXE_PATH=$BASE_PATH/IonCLI
BIN_PATH=/usr/bin/ion

# Ensure base path exists, otherwise create it.
if [ ! -d "$BASE_PATH" ]; then
    echo "Creating base directory."
    mkdir -p $BASE_PATH
fi

# Copy program.
cp -r $ROOT/* $BASE_PATH

# Remove installation script.
rm -f $BASE_PATH/INSTALL.sh

# Mark as executable.
chmod +x $EXE_PATH

# Create symbolic link.
ln -s $EXE_PATH $BIN_PATH
