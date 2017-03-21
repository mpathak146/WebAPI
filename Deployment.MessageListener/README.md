# CommandListener Deployment

This project builds an installer for the Fourth Dataload Message Listener which can be deployed to any environment.

The script **DeployService.ps1** is used to initiate a deployment. It does the following:

+ Copies the application files to a remote server
+ Runs the **PackageInstaller.ps1** script on the remote server

The **PackageInstaller.ps1** script runs on the target server and does the following:

+ Stops and de-installs the service if it already exists
+ Removes the existing service files
+ Copies the new service files to the install location
+ Copies the correct version of the configuration file to the install location
+ Installs the service
+ Restarts the service

## Settings

The package is controlled via **CreatePackage.targets**. This is an MSBuild targets file that sets variables 
such as the name of the service, the path to install it on and the files to include in the package.

Most of the settings are fixed, 
though there three settings in the first PropertyGroup element that are specific to this project:

| Setting | Description |
| ---------------------------- | ----------- |
| ServiceName | The name of the service used in installing, stopping and starting the service, 
e.g. FourthAccountCommandListener. |
| ServiceNamespace | The root namespaace of the service executable, e.g. Fourth.Dataload.MessageListener. |
| ServicePath | The path where the service will be installed (expressed WITHOUT a drive letter, i.e. \this_path\). |
| PackagePath | The path where the installation package will be copied to on the target server (expressed WITHOUT a drive letter, i.e. \this_path\). |

Note that the installer assumes that 
the service executable and configuration files are named after the ServiceNamespace property..

The files to install with the service are listed in *ContentFiles* elements in an *ItemGroup* element as shown below:
```
<!-- Place all the application files here - they will be copied to <Root>\Package\Content -->
<ItemGroup>
   <ContentFiles Include="$(SourceDirectory)\Autofac.dll"/>
   <ContentFiles Include="$(SourceDirectory)\Autofac.Configuration.dll"/>
   <ContentFiles Include="$(SourceDirectory)\Fourth.Orchestration.dll"/>
   ...
</ItemGroup>
```

## Configuration

.Net configuration transforms are used to generate configuration files at compile time.
A configuration file for each environment is compiled into the package 
and the correct file is selected during installation.

The transform files are held in the project's **Configuration** folder.

Full details of config transformation syntax can be found here: https://msdn.microsoft.com/en-us/library/dd465326(v=vs.110).aspx

## Deployment package

The project compiles into the following structure in the project's BIN\RELEASE directory:

* **DeployService.ps1** - The main deployment script
* **\Package** - The directory holding the packaged files
    * **\Configuration** - The transformed configuration files
    * **\Content** - The service application files
    * **PackageInstaller.ps1** - The installation script run on the remote server
    * **Intallutil.exe** - Service installation utility
    * **PackageInstaller.ps1** - Configuration for the service installation utility

## Usage:

Packages are normally deployed via Jenkins but they do not have to be. 
The following command line to deploy the package

`.\DeployService.ps1 -Computername <IP ADDRESS> -Configuration prelive -username <USER> -password <PASSWORD>`

| Parameter | Description |
| --------- | ----------- |
| -Computername | The name of the machine to deploy to - this can also be an IP address. |
| -Username | The username for credentials with sufficient rights to deploy to the specified machine. Should include the domain. |
| -Password | The password. |

## Jenkins

Jenkins is used to build the solution and deploy the packages to prelive and live environments.

The jobs are currently located at the following:

* Build: http://jenkins:8080/job/PeopleSystemDataImport-Build/
* Deploy to QA: http://jenkins:8080/job/PeopleSystemDataImport-Deploy-QA/
