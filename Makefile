all: IonCLI
	cd Ion
	dotnet build -c Release
	cd ..
	dotnet build -c Release
clean:
	rm -rf bin
	rm -rf .packages
	rm -rf obj
test: 
	cd IonCLI.Tests
	dotnet test
install:
	cp -r IonCLI/bin/Release/netcoreapp2.2/linux-x64/publish /opt/ioncli
	chmod +x /opt/ioncli/IonCLI
	ln -s /opt/ioncli/IonCLI /usr/bin/ion
uninstall:
	rm -r /opt/ioncli
