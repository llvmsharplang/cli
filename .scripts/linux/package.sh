#!/bin/bash

# Abort script on failure.
set -e

# Platforms.
platforms=(linux-x64 linux-arm)

# Global directories.
ROOT=$PWD
PACKAGES_PATH=$ROOT/.packages
INSTALLERS_PATH=$ROOT/.installers
TOOLS_FOLDER_NAME=.tools
TOOLS_PATH=$ROOT/$TOOLS_FOLDER_NAME

# Verify that the zip command exists.
if [ -x "$(command -v zip)" ]; then
    # Append Windows-based and MacOS platforms.
    platforms=(win10-x64 win10-x86 osx-x64 ${platforms[@]})
else
    echo "Notice: 'zip' utility command is not installed. Windows and MacOS packages will be omitted."
fi

# Cleanup.
bash .scripts/linux/clean.sh

# Validate directories.
directories=($ROOT $PACKAGES_PATH $INSTALLERS_PATH $TOOLS_PATH)

for i in ${directories[@]}; do
    if [ ! -d ${i} ]; then
        echo "Error: Required directory does not exist: ${i}"
        exit 1
    fi
done

for i in ${platforms[@]}; do
    # Build platform.
    dotnet publish -c Release -r ${i}

    # Setup directories.
    PUBLISH_PATH=IonCLI/bin/Release/netcoreapp2.2/${i}/publish

    # Create the packages output directory.
    mkdir -p $PACKAGES_PATH

    # Copy corresponding installer scripts and text files to the publish directory.
    cp $INSTALLERS_PATH/INSTALL.sh $PUBLISH_PATH
    cp $INSTALLERS_PATH/*.txt $PUBLISH_PATH

    # Select tools depending on platform.
    TOOLS=""

    # Selection process.
    if [[ ${i} == linux-x64 ]]; then
        TOOLS=linux
    elif [[ ${i} == linux-arm ]]; then
        TOOLS=armv7a
    elif [[ ${i} == osx* ]]; then
        TOOLS=macOS
    elif [[ ${i} == win10-x86 ]]; then
        TOOLS=win32
    elif [[ ${i} == win10-x64 ]]; then
        TOOLS=win64
    else
        echo "Error: Unable to map tools from platform: ${i}"
        exit 1
    fi

    # Copy tools.
    cp -r $TOOLS_PATH/$TOOLS $PUBLISH_PATH/tools

    # Enter the publish path.
    cd $PUBLISH_PATH

    # Windows-based or MacOS platform.
    if [[ ${i} == win* ]] || [[ ${i} == osx* ]]; then
        # Compress the publish directories (zip).
        zip -r $PACKAGES_PATH/${i}.zip .
    # Linux-based platform.
    elif [[ ${i} == linux* ]]; then
        # Compress the publish directories (gzip).
        tar -czvf $PACKAGES_PATH/${i}.tar.gz .
    # Unsupported platform.
    else
        echo "Notice: Unrecognized platform: ${i}"
    fi

    # Enter root directory.
    cd $ROOT
done

# Finish.
exit 0
