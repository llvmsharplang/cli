#!/bin/bash

# Define alias target.
ALIAS_TARGET="$PWD/IonCLI"

# Create the alias in the current shell.
alias ion="$ALIAS_TARGET"

# Append the alias to the .bashrc file.
echo "alias ion=\"$ALIAS_TARGET\"" >> ~/.bashrc

# Inform the user that the process was completed.
echo "IonCLI utility was installed as an alias and saved on the ~/.bashrc file.
You may now use \"$ ion\" to compile Ion source code files.
Restart or open a new shell in order for the changes to take effect."
