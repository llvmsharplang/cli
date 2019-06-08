#!/bin/bash

base_directories=(IonCLI IonCLI.Tests)

# Remove output directories.
for i in ${base_directories[@]}; do
    rm -rf ${i}/bin ${i}/obj
done

rm -rf .packages
mkdir -p .packages
