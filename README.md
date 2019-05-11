### Ion CLI

The CLI compiler for the Ion language.

### Development Environment Notes

Please note that to setup a working development environment, a path to an existing [core library](https://github.com/IonLanguage/Ion) build directory must be provided in the [cli.csproj](https://github.com/IonLanguage/Ion/cli/blob/5a577626af24a43f090903da00b05b7ca7b9876e/cli.csproj#L9) file.


### Development environment setup

Developer env. setup is a breeze! Follow the steps below for your OS.

#### Windows

First, make sure you have Inno Setup installed, as this is used to package the Windows installer.

> [Click here to download it](http://www.jrsoftware.org/download.php/is.exe)

Now, simply `cd` into your desired development folder, and run the following one-liner:
```cmd
git clone https://github.com/IonLanguage/Ion.CLI && cd Ion.CLI/.scripts/windows && setup-env.bat
```

You're all set!

#### Additional notes

If you're developing the application, it is recommended you run it with the following command:

```shell
$ dotnet run -- -vd -r .test -t .llvm-tools
```

### Installation

If you have downloaded this as a release, follow the instructions below to install the CLI utility locally on your machine:

#### Windows

Simply right-click on the `install.ps1` file, and select "Run with PowerShell".

#### Linux

Run the `install.sh` script:

```shell
$ bash install.sh
```

### Usage

Usage is simple. Once you've ran the installation script on your platform, you can simple run the following command on a Windows Command Prompt (if you're on Windows) or a shell otherwise:

```shell
$ ion
```

### Options

```
-v, --verbose         Set output to verbose messages.

-e, --exclude         Exclude certain directories from being processed.

-o, --output          (Default: ion.bin) The output directory which the program will be
                      emitted onto.

-r, --root            The root directory to start the scanning process from.

-b, --bitcode         Print out the LLVM Bitcode code instead of LLVM IR.

-s, --silent          Do not output any messages.

-i, --no-integrity    Skip integrity check.

-d, --debug           Use debugging mode.

-t, --tools-path      (Default: llvm-tools) Specify the tools directory path to use. Path is
                      relative to the CLI's execution directory.

--help                Display this help screen.

--version             Display version information.
```

Note: If any of the CLI's arguments are specified and defined within the package manifest file, the CLI's arguments will take precedence.
