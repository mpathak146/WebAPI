# ApiEndPoint Deployment

This project builds an installer for the Fourth Dataload Service API which can be deployed to any environment.

Most of the legwork is carried out by the WebDeploy package created at compile time. The script **DeployApiEndpoint.ps1** is used to initiate a deployment. It does the following:

+ Copies the correct configuration file for the selected environment
+ Executes the WebDeploy package generated during compilation

## Settings

The package is controlled via **CreatePackage.targets**. This is an MSBuild targets file that sets variables such as the name of the site, the path to install it on and the files to include in the package.

Most of the settings are fixed, though there three settings in the first PropertyGroup element that are specific to this project:

| Setting | Description |
| ---------- | ----------- |
| WebProject | The root namespace of the project to deploy - e.g. Fourth.Dataload.ApiEndpoint. |
| DeployProject | The root namespace of the deployment project, i.e. Deployment.ApiEndpoint. |
| DeployScriptFilename | The name of the main deployment script that is run by Jenkins. |

## Configuration

WebDeploy parameterisation us used to set configuration at deploy time. 

A **parameters.xml** file is created in the web project that defines the parameters that can be changed by a deployment.

A separate configuration file for each environment is created for each environment and selected during deployment. The environment configuration files are held in the project's **Configuration** folder.

Full details of parameterisation can be found here: https://msdn.microsoft.com/en-us/library/ff398068(v=vs.110).aspx

## Deployment package

The project compiles into the following structure in the project's BIN\RELEASE directory:

* **DeployApiEndpoint.ps1** - The main deployment script
* **\WebPackage** - The directory holding the WebDeploy package files
* **\Configuration** - The environment configuration files

## Usage:

Packages are normally deployed via Jenkins but they do not have to be. The following command line to deploy the package

`.\DeployApiEndPoint.ps1 -Computername <IP ADDRESS> -Configuration prelive -username <USER> -password <PASSWORD>`

| Parameter | Description |
| ---------- | ----------- |
| -Computername | The name of the machine to deploy to - this can also be an IP address. |
| -Username | The username for credentials with sufficient rights to deploy to the specified machine. Should include the domain. |
| -Password | The password. |

## Jenkins

Jenkins is used to build the solution and deploy the packages to prelive and live environments.

The jobs are currently located at the following:

* Build: http://jenkins:8080/job/PeopleSystemDataImport-Build/
* Deploy to QA: http://jenkins:8080/job/PeopleSystemDataImport-Deploy-QA/

