### Ion CLI

The CLI compiler for the Ion language.

### Development Environment Notes

Please note that to setup a working development environment, a path to an existing [core library](https://github.com/IonLanguage/Ion) build directory must be provided in the [cli.csproj](https://github.com/IonLanguage/Ion/cli/blob/5a577626af24a43f090903da00b05b7ca7b9876e/cli.csproj#L9) file.

### Installation

If you have downloaded this as a release, use the following scripts to install the CLI utility locally on your machine:

#### Windows

Simple execute the `install.bat` script by double-clicking it.

#### Linux

Run the `install.sh` script:

```shell
$ bash install.sh
```

### Usage

Usage is simple. Once you've ran the installation script on your platform, you can simple run the following command on a Windows Command Prompt (if you're on Windows) or a shell (if you're on Linux):

```shell
$ ion
```

### Options

```
-v, --verbose    Set output to verbose messages.

-e, --exclude    Exclude certain directories from being processed.

-o, --output     (Default: l.bin) The output directory which the program will be 
                 emitted onto.

-r, --root       The root directory to start the scanning process from.

-i, --ir         Print out the emitted IR code instead of the compiled result.

-a, --asm        Prints assembly code for target machine to file.

--help           Display this help screen.

--version        Display version information.
```
