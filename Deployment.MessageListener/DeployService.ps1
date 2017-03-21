#=================================================================
# Service installation script
#=================================================================

#=================================================================
# Parameter declarations
#=================================================================
param 
(
[ValidateNotNullOrEmpty()]
[string]$ComputerName = $(throw "No -ComputerName switch specified for the name of the target machine."),
[ValidateNotNullOrEmpty()]
[string]$Configuration = $(throw "No -Configuration switch specified - e.g. Production, Staging or Test."),
[ValidateNotNullOrEmpty()]
[string]$Username = $(throw "No -Username switch specified."),
[ValidateNotNullOrEmpty()]
[string]$Password = $(throw "No -Password switch specified.")
)

# Constants.

Set-Variable ServiceName -option ReadOnly -value "REPLACE_ServiceName"
Set-Variable ScriptPath -option ReadOnly -value (Split-Path (Get-Variable MyInvocation).Value.MyCommand.Path)
Set-Variable ServicePath -option ReadOnly -value "REPLACE_ServicePath"
Set-Variable PackagePath -option ReadOnly -value "REPLACE_PackagePath"

function Main()
{

	"-------------------------------------------------------"
	"Starting DeployService.ps1."
	"-------------------------------------------------------"

	$ContentPath = "$ScriptPath\Package"
	$UploadPath = join-path "\\$ComputerName\c$\" $PackagePath
	$InstallerFilePath = join-path(join-path "C:\" $PackagePath) "\PackageInstaller.ps1"
	$ServiceInstallPath = join-path "C:\" $ServicePath


	"ServiceName:                 $ServiceName"
	"Configuration:               $Configuration"
	"Working directory:           $ScriptPath"
	"Upload path:                 $UploadPath"
	"Installer file path:         $InstallerFilePath"
	"Service install path:        $ServiceInstallPath"
	"-------------------------------------------------------"

	# Copy the installation files to the remote server

	"Dropping any existing shares to $ComputerName" 

	net use "\\$ComputerName\c$" /delete

	"Establishing remote connection to $ComputerName using account $Username"

	net use "\\$ComputerName\c$" "$Password" /USER:"$Username" /persistent:no

	"Copying installation files to: $UploadPath"

	if(Test-Path $UploadPath) 
	{
		"Removing old installation files..."
		Remove-Item $UploadPath\* -recurse
		Copy-Item $ContentPath\* $UploadPath -recurse
	}
	else 
	{
		Copy-Item $ContentPath $UploadPath -recurse
	}

	"Cancelling remote connection to $ComputerName"

	net use "\\$ComputerName\c$" /USER:"$Username" /delete

	"Installation files copied."

	# Now invoke the deployment script remotely to install the service

	"Attempting to run deployment script remotely at $InstallerFilePath"

	# Create some credentials 

	$pw = convertto-securestring -AsPlainText -Force -String "$Password"
	$cred = new-object -typename System.Management.Automation.PSCredential -argumentlist "$Username",$pw

	# Remotely invoke the command

	$Parameters = $ComputerName, $Configuration, $InstallerFilePath, $ServiceInstallPath
	invoke-command -ComputerName $ComputerName -Credential $cred -scriptblock { param($ip, $config, $installer, $path) &($installer) -ComputerName $ip -Configuration $config -ServicePath $path } -ArgumentList $Parameters

}

function Abort() 
{
	"Installation aborted."
	exit 1
}

# Kick off the main installation function

try
{
	try {
		Main
	} finally {
	}
}
catch [System.Exception]
{
    "DeployService.ps1 script failed."
	$Error 
	exit 1
}