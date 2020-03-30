[CmdletBinding(DefaultParameterSetName = 'None')]
param
(

    [String] [Parameter(Mandatory = $true)]
    $Source,
    
    [String] [Parameter(Mandatory = $true)]
    $Destination,
    
    [String] [Parameter(Mandatory = $false)]
    $DestinationComputer,
    
    [String] [Parameter(Mandatory = $false)]
    $AuthType,
    
    [String] [Parameter(Mandatory = $false)]
    $Username,
    
    [String] [Parameter(Mandatory = $false)]
    $Password,

    [String] [Parameter(Mandatory = $false)]
    $AdditionalArguments
)

# adding System.Web explicitly, since we use http utility
Add-Type -AssemblyName System.Web

Write-Host "Source= $Source"
Write-Host "Destination= $Destination"
Write-Host "DestinationComputer= $DestinationComputer"
Write-Host "Username= $Username"
Write-Host "AdditionalArguments= $AdditionalArguments"

# Find msdeploy on agent

$MSDeployKey = 'HKLM:\SOFTWARE\Microsoft\IIS Extensions\MSDeploy\3' 
 if(!(Test-Path $MSDeployKey)) { 
 throw "Could not find MSDeploy. Use Web Platform Installer to install the 'Web Deployment Tool' and re-run this command" 
 } 

$InstallPath = (Get-ItemProperty $MSDeployKey).InstallPath
 if(!$InstallPath -or !(Test-Path $InstallPath)) { 
 throw "Could not find MSDeploy. Use Web Platform Installer to install the 'Web Deployment Tool' and re-run this command" 
 } 

$msdeploy = Join-Path $InstallPath "msdeploy.exe" 
 if(!(Test-Path $MSDeploy)) { 
 throw "Could not find MSDeploy. Use Web Platform Installer to install the 'Web Deployment Tool' and re-run this command" 
 } 


Write-Host "Deploying..."

$remoteArguments = "computerName='$DestinationComputer',userName='$UserName',password='$Password',authType='$AuthType',"

if (-not $DestinationComputer -or -not $AuthType) {
    Write-Host "No destination or authType defined, performing local operation"
    $remoteArguments = ""
}

[string[]] $arguments = 
 "-verb:sync",
 "-source:contentPath='$Source'",
 "-dest:contentPath='$Destination',$($remoteArguments)includeAcls='False'",
 "-allowUntrusted"

$fullCommand = """$msdeploy"" $arguments $AdditionalArguments"
Write-Host $fullCommand


$result = cmd.exe /c "$fullCommand"

Write-Host $result


Write-Verbose "Done."