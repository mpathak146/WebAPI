#=================================================================
# WebApi project deployment script
# Example usage:
# DeployApiEndPoint.ps1 -Computername 10.10.20.85 -Configuration QA -username Fourth\devadmin -password ########
#=================================================================

#=================================================================
# Parameter declarations
#=================================================================
param(
[ValidateNotNullOrEmpty()]
[string]$ComputerName = $(throw "No -ComputerName switch specified for the name of the target machine."),
[ValidateNotNullOrEmpty()]
[string]$Configuration = $(throw "No -Configuration switch specified - e.g. Production, Staging or Test."),
[ValidateNotNullOrEmpty()]
[string]$Username = $(throw "No -Username switch specified."),
[ValidateNotNullOrEmpty()]
[string]$Password = $(throw "No -Password switch specified.")
)

# Constants
#Set-Variable ProjectName -option ReadOnly -value "REPLACE_ProjectName"
Set-Variable ScriptPath -option ReadOnly -value (Split-Path (Get-Variable MyInvocation).Value.MyCommand.Path)

function Main()
{
	
	$ParametersFile = Join-Path $ScriptPath "Configuration\$Configuration.SetParameters.xml"
	$ParametersFound = Test-Path $ParametersFile
	
	#=================================================================
	# Write the summary information
	#=================================================================
	
	"-------------------------------------------------------"
	"Running DeployApiEndpoint.ps1"
	"-------------------------------------------------------"
	"Website:                 $ProjectName"
	"Computer:                $ComputerName"
	"Configuration:           $Configuration"
	"Working directory:       $ScriptPath"
	"Parameters found?:       $ParametersFound"
	"Parameters path:         $ParametersFile"
	"-------------------------------------------------------"
	#=================================================================
	# Copy the selected configuration file over SetParameters.xml
	#=================================================================

	if(!$ParametersFound) 
	{
		throw [System.IO.FileNotFoundException] "Parameters file not found for config $Configuration."
	}

	Copy-Item $ParametersFile $ScriptPath\WebPackage\WebPackage.SetParameters.xml -force

	"Copied parameters file from $ParametersFile."

	#=================================================================
	# Execute WebDeploy to deploy the actual package
	#=================================================================

	"Executing WebDeploy to install the site."

	# This is required to bypass our certificates
	$env:_MsDeployAdditionalFlags = "-allowUntrusted"

	$server = "/M:http://" + $ComputerName + ":9805/MSDeploy.axd" 
	$user = "/U:" + $Username
	$pwd = "/P:" + $Password
	$Parameters = "/Y", $server, $user, $pwd, "/A:Basic"

	.\WebPackage\WebPackage.deploy.cmd $Parameters

	"-------------------------------------------------------"
	"Website installation completed successfully."
	"-------------------------------------------------------"
}

# Kick off the main installation function
try
{
    Main
}
catch [System.Exception]
{
    "Deployment script failed."
    $Error 
    exit 1
}
