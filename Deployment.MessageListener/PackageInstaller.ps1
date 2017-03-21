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
[string]$ServicePath = $(throw "No -ServicePath switch specified."),
[bool]$StartService = $True
)

# Constants.

Set-Variable ServiceName -option ReadOnly -value "REPLACE_ServiceName"
Set-Variable ServiceFileName -option ReadOnly -value "REPLACE_ServiceFileName"
Set-Variable TargetConfigurationFileName -option ReadOnly -value "REPLACE_TargetConfigurationFileName"
Set-Variable LogFilename -option ReadOnly -value "log-$(get-date -format 'MMddyyyy-hhmmss').txt"
Set-Variable ScriptPath -option ReadOnly -value (Split-Path (Get-Variable MyInvocation).Value.MyCommand.Path)

function Main()
{
	#=================================================================
	# Fetch the version of the component that we are installing
	#=================================================================

	$Version = [System.Diagnostics.FileVersionInfo]::GetVersionInfo("$ScriptPath\Content\$ServiceFileName").FileVersion

	#=================================================================
	# Write the summary information
	#=================================================================
	
	"-------------------------------------------------------"
	"Running PackageInstaller.ps1"
	"-------------------------------------------------------"
	"ServiceName:                 $ServiceName"
	"Version:                     $version"
	"Configuration:               $Configuration"
	"Working directory:           $ScriptPath"
	"Service path:                $ServicePath"
	"Service filename:            $ServiceFileName"
	"Configuration filename:      $TargetConfigurationFileName"
	"Start Service?:              $StartService"
	"-------------------------------------------------------"

	#=================================================================
	# Check that a valid configuration has been specified 
	# There should be a matching app.*.config file available
	#=================================================================
	
	$ConfigPath = (join-path -path $ScriptPath -childpath Configuration\app.$Configuration.config)
	if(-not (Test-Path $ConfigPath)) 
	{
		"Configuration '$Configuration' not found. Aborting."
		Abort
	}

	#=================================================================
	# Shut down any services or sites associated with this component
	#=================================================================
	
	$service1 = Get-WmiObject -Class Win32_Service -Filter "Name = '$ServiceName'"
	if ($service1 -ne $null) 
	{
		"Stopping $ServiceName..."
		$service1 | stop-service -Force
		"Waiting 10 seconds before continuing to ensure all dependencies are released..."
		Start-sleep -s 10
	}	
	
	#=================================================================
	# Delete services or sites associated with this component
	#=================================================================
	
	$service1 = Get-WmiObject -Class Win32_Service -Filter "Name = '$ServiceName'"
	if ($service1 -ne $null) 
	{
		"Deleting $ServiceName."
		$service1.Delete()
	}

	#=================================================================
	# Verify that the folder exists and remove any existing files
	#=================================================================

	if(Test-Path $ServicePath) 
	{
		# Remove existing files though retain any logs
		"Installation path $ServicePath found."
		"Removing existing files."
		Remove-Item $ServicePath\* -recurse -exclude *.log
	}
	else 
	{
		# Create the path
		"Installation path $ServicePath does not exist."
		"Creating installation folder at $ServicePath."
		New-Item -ItemType directory -Path $ServicePath
	}

	#=================================================================
	# Copy the files in the Content directory to the installation path
	# Do these one-by-one rather than using wildcards so we can log each individual file copy action
	#=================================================================

	$ContentPath = (Join-Path -path $ScriptPath -childpath Content)
	"Copying files from $ContentPath to $ServicePath"

	if(Test-Path $ServicePath) 
	{	
		Copy-Item $ContentPath\* $ServicePath -recurse
	}
	else 
	{
		Copy-Item $ContentPath $ServicePath -recurse
	}

	"Files copied."	
	
	#=================================================================
	# Copy and rename the correct version of App.*.Config for the requested configuration
	#=================================================================

	$CopyFrom = (join-path -path $ScriptPath -childpath Configuration\app.$Configuration.config)
	$CopyTo = (join-path -path $ServicePath -childpath $TargetConfigurationFileName)
	"Copying configuration."
	Copy-Item $CopyFrom $CopyTo

	#=================================================================
	# Install the service using InstallUtil.exe if the service is not found
	# NB: This will not work for remote machines.
	#=================================================================

	"-------------------------------------------------------"
	"Starting service installation using installutil.exe"
	"-------------------------------------------------------"
	
	if((get-service -name ($ServiceName + "*")) -eq $null) {
		"Installing service $ServiceName"
		$exePath = (join-path -path $ServicePath -childpath $ServiceFileName)
		
		# the path to powershell is not known. we'll try to use the latest powershell and fallback to earlier versions if not found.
		if(Test-Path "C:\Windows\Microsoft.NET\Framework\v4.0.30319\InstallUtil.exe") { C:\Windows\Microsoft.NET\Framework\v4.0.30319\InstallUtil.exe "$exePath" } 
		elseif(Test-Path "C:\Windows\Microsoft.NET\Framework\v2.0.50727\InstallUtil.exe") { C:\Windows\Microsoft.NET\Framework\v2.0.50727\InstallUtil.exe "$exePath" } 
		else { "InstallUtil.exe not found. Please install .net framework on this machine to install the service. " }
		
		"Waiting 5 seconds before continuing..."
		Start-Sleep -s 5
	}

	#=================================================================
	# Restart the service
	#=================================================================
	
	if($StartService -and ($ServiceName.length -gt 0) -and (get-service -name ($ServiceName + "*"))) {
		"Starting $ServiceName."
		Get-Service -Name $ServiceName -ComputerName $ComputerName | Set-Service -Status Running
	}

	"-------------------------------------------------------"
	"Service installation completed successfully."
	"-------------------------------------------------------"
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
    "PackageInstaller.ps1 script failed."
	$Error 
	exit 1
}