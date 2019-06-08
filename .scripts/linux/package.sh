#!/bin/bash

# Setup.
platforms=(linux-x64 linux-x86 linux-arm win10-x64 win10-x86)

# Cleanup.
bash .scripts/linux/clean.sh

# Install zip command if applicable.
if [ -x "$(command -v zip)" ]; then
    echo "Installing required zip command"
    sudo apt-get install zip
fi

for i in ${platforms[@]}; do
    # Build platform.
    dotnet publish -c Release -r ${i}

    # Setup directories.
    ROOT=$PWD
    PUBLISH_PATH=IonCLI/bin/Release/netcoreapp2.2/${i}/publish
    PACKAGES_PATH=.packages
    INSTALLERS_PATH=.installers

    # Create the packages output directory.
    mkdir -p $PACKAGES_PATH

    # Copy corresponding installer scripts and text files to the publish directory.
    cp $INSTALLERS_PATH/INSTALL.sh $PUBLISH_PATH
    cp $INSTALLERS_PATH/*.txt $PUBLISH_PATH

    # Enter the publish path.
    cd $PUBLISH_PATH

    # Windows-based platform.
    if [[ ${i} == win* ]]; then
        zip -r $ROOT/$PACKAGES_PATH/${i}.zip .
    # Linux-based platform.
    elif [[ ${i} == linux* ]]; then
        # Zip the publish directories.
        tar -czvf $ROOT/$PACKAGES_PATH/${i}.tar.gz .
    # Unsupported platform.
    else
        echo "Unrecognized platform: ${i}"
        exit 1
    fi

    # Enter root directory.
    cd $ROOT
done

# Finish.
exit 0
