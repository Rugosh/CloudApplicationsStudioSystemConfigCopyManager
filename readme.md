# Cloud Application System Config Helper

A very quick (and dirty) programm to move the system configuration to the new Studio version with every release.

Basicly it is nothing else than a quick programm to move the Config from the source path to the target path with X as the last release and X as the current release.

Source Path:
Computer\HKEY_CURRENT_USER\SOFTWARE\SAP\SAPCloudApplicationsStudio\X\DialogPage\SAP.Copernicus.CopernicusOptionPage


Target Path:
Computer\HKEY_CURRENT_USER\SOFTWARE\SAP\SAPCloudApplicationsStudio\YYY\DialogPage\SAP.Copernicus.CopernicusOptionPage

## Requirements 
* .net Core Framework 3.1 or higher (https://dotnet.microsoft.com/download/dotnet/3.1)

## Usage
~~~shell
dotnet CloudApplicationsStudioSystemConfigCopyCLI.dll [options]

Options
  -s|--source       Source Version of the SAP Cloud Application Studio
  -t|--target       Target Version of the SAP Cloud Application Studio
  -a|--auto         Automatic choose the versions Default: "False".
  -h|--help         Shows help text.
  --version         Shows version information.
~~~