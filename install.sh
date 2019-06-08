#!/usr/bin/env bash

set -e

# This is a conviniency bash installer script, designed to be invoked remotely by curl.
# -------------------------------------------------------------------------------------

# Setup.
VERSION="0.0.4-alpha"
FILE_NAME="linux.tar.gz"

# Download the release.
wget -o $FILE_NAME https://github.com/IonLanguage/Ion.CLI/releases/download/v$VERSION/$FILE_NAME

# Unpack release.
tar -xvzf $FILE_NAME

# Remove release package.
rm $FILE_NAME

# Enter unpacked directory.
cd $FILE_NAME

# Invoke installer.
bash INSTALL.sh
