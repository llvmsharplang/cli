all: IonCLI
	# Build Ion's core library project.
	cd Ion/Ion && dotnet build -c Release

	# Build IonCLI project.
	cd IonCLI && dotnet build -c Release
clean:
	bash .scripts/linux/clean.sh
test:
	cd IonCLI.Tests && dotnet test
publish:
	cd IonCLI && dotnet publish -c Release -o bin/publish -r linux-x64
install:
	export ION_ROOT=$(shell pwd)/IonCLI/bin/publish && bash .installers/INSTALL.sh
uninstall:
	rm -rf /opt/ioncli
	rm -f /usr/bin/ion
