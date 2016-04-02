<#
.SYNOPSIS
Tests whether the current process is being run under an elevated (i.e. admin) token.

.DESCRIPTION
On default Windows installs, users who are administrators run under a limited token by default; only when the process is explicitly launched "As Administrator" does it run with a token
that has administrative privileges.

.ROLE
Internal

.EXAMPLE 
Test-ProcessElevation
Returns $true if the process is elevated; $false otherwise.
#>
function Test-ProcessElevation 
{
    [CmdletBinding()]
    param()
    $identity  = [System.Security.Principal.WindowsIdentity]::GetCurrent()
    $principal = New-Object System.Security.Principal.WindowsPrincipal($identity)
    $principal.IsInRole([System.Security.Principal.WindowsBuiltInRole]::Administrator)
}

<#
.SYNOPSIS
Enables the system-wide Fusion log.

.DESCRIPTION
Enables Fusion logging at the given log path. Fusion logging is useful to diagnose problems loading .Net assemblies (.dlls), which manifest as FileNotFoundException or FileLoadException in the event log.

.PARAMETER LogAll 
If set, all Fusion bind attempts are logged, not just errors.

.PARAMETER LogPath
The path to store Fusion log files. Will be created if it doesn't exist.

.EXAMPLE 
Enable-FusionLog -LogAll -LogPath D:\logs
Enables Fusion logging for all binds and sets log path to D:\logs

.NOTES
Need to run elevated.
Fusion is the name of the module loading subsystem in Windows.

.ROLE
Diagnostics

.LINK 
Disable-FusionLog
#>
function Enable-FusionLog 
{
    [CmdletBinding()]
    param (
        [Parameter(HelpMessage='Include to log both successful and unsuccessful assembly loads.')]
        [switch]$LogAll, 
        [Parameter(HelpMessage='Path to log to. (Will be created if it does not exist.)')]
        [string]$LogPath = "$($env:HOMEDRIVE)\logs\"

    ) 

    if (-not (Test-ProcessElevation))
    {
        Write-Error 'Must run this command in an elevated shell.'
        return
    }

    [void](New-ItemProperty  HKLM:Software\Microsoft\Fusion -name EnableLog -propertyType dword -ErrorAction SilentlyContinue)  
    [void](New-ItemProperty  HKLM:Software\Microsoft\Fusion -name ForceLog -propertyType dword -ErrorAction SilentlyContinue)  
    [void](New-ItemProperty  HKLM:Software\Microsoft\Fusion -name LogFailures -propertyType dword -ErrorAction SilentlyContinue)  
    [void](New-ItemProperty  HKLM:Software\Microsoft\Fusion -name LogPath -propertyType string -ErrorAction SilentlyContinue)
    Set-ItemProperty  HKLM:Software\Microsoft\Fusion -name EnableLog -value 1 -ErrorAction Stop 
    
    if(-not (Test-Path $LogPath))
    {
        New-Item $LogPath -Type Directory | Out-Null
        Write-Output "Created $LogPath"
    }

    if ($LogAll) 
    {
        Set-ItemProperty  HKLM:Software\Microsoft\Fusion -name ForceLog -value 1 -ErrorAction Stop 
    }

    Set-ItemProperty  HKLM:Software\Microsoft\Fusion -name LogFailures -value 1 -ErrorAction Stop 
    Set-ItemProperty  HKLM:Software\Microsoft\Fusion -name LogPath -value $LogPath -ErrorAction Stop 
    Write-Output "Fusion log enabled. Logging to $LogPath. Use Disable-FusionLog to disable." 
}

<#
.SYNOPSIS
Disables the system-wide Fusion log.

.DESCRIPTION
Disables all Fusion logging, not even errors will be logged.

.EXAMPLE 
Disable-FusionLog
Disables Fusion logging for any binds.

.NOTES
Need to run elevated.
Fusion is the name of the module loading subsystem in Windows.

.ROLE
Diagnostics

.LINK 
Enable-FusionLog
#>
function Disable-FusionLog 
{ 
    [CmdletBinding()]
    param()
    
    if (-not (Test-ProcessElevation))
    {
        Write-Error 'Must run this command in an elevated shell.'
        return
    }

    Set-ItemProperty  HKLM:Software\Microsoft\Fusion -name EnableLog -value 0 -ErrorAction Stop 
    Set-ItemProperty  HKLM:Software\Microsoft\Fusion -name ForceLog -value 0 -ErrorAction Stop 
    Set-ItemProperty  HKLM:Software\Microsoft\Fusion -name LogFailures -value 0 -ErrorAction Stop 
    Write-Host "Fusion log disabled"  
}

function Enable-StartWithDebugger
{
    [CmdletBinding()]
    param (
        [Parameter(Position=0, Mandatory=$true, HelpMessage='Enter the name of the EXE to start with a debugger, like "calc.exe"')]
        [ValidatePattern({\.exe$})]
        [string]$ExeName,
        [Parameter(Position=1, Mandatory=$true, HelpMessage='Choose 32 or 64 bits for the EXE target machine')]
        [ValidateSet(32, 64)]
        [int]$Bitness,
        [Parameter(Position=2, Mandatory=$false, HelpMessage='Which debugger to use?')]
        [ValidateSet('windbg', 'cdb', 'vsjitdebugger')]
        [string]$Debugger,
        [Parameter(Mandatory=$false, HelpMessage='Which SDK version to use?')]
        [ValidateSet('8', '8.1', '10')]
        [string]$SdkVersion = '10',
        [Parameter(Mandatory=$false, HelpMessage='Use a full path to a specific debugger.')]
        [ValidateNotNullOrEmpty]
        [string]$FullPathDebugger,
        [Parameter(HelpMessage='Include to enable loader snaps in debugger output.')]
        [switch]$IncludeLoaderSnaps
    )    
    
    if (-not (Test-ProcessElevation))
    {
        Write-Error 'Must run this command in an elevated shell.'
        return
    }

    if ($FullPathDebugger)
    {
        if (-not (Test-Path $FullPathDebugger))
        {
            Write-Error "Cannot find $FullPathDebugger"
            return
        }
        
        $debuggerPath = $FullPathDebugger
    }
    elseif($Debugger)
    {
        $machineType = switch($Bitness)
        {
            32 { 'x86' }
            64 { 'x64' }
        }

        $debugToolsPath = join-path ${env:ProgramFiles(x86)} "Windows Kits\$SdkVersion\Debuggers\$machineType\";

        if (-not (Test-Path $debugToolsPath))
        {
            Write-Error "Windows SDK $debugToolsPath Debuggers not found"
            return
        }
                
        $dbgFullPath = switch($Debugger)
        {
            'windbg' { (Join-Path $debugToolsPath 'windbg.exe') }
            'cdb' { (Join-Path $debugToolsPath 'cdb.exe') }
            'vsjitdebugger' { 'vsjitdebugger.exe' }
        }

        $debuggerPath = (Get-Command $dbgFullPath -ErrorAction SilentlyContinue).Definition
        
        if (-not $debuggerPath)
        {
            Write-Error "Debugger $debuggerPath not found"
            return
        }
    }
    else
    {
        Write-Error 'No debugger specified'
        return
    }

    $imageOptionsKey = "HKLM:SOFTWARE\Microsoft\Windows NT\CurrentVersion\Image File Execution Options"
    $optionsKey = New-Item -Path $imageOptionsKey -Name $ExeName -ErrorAction SilentlyContinue

    if (-not $optionsKey)
    {
        $optionsKey = Get-Item -Path (join-path $imageOptionsKey $ExeName)

        if (-not $optionsKey)
        {
            Write-Error "Cannot create key for $ExeName in $imageOptionsKey"
            return
        }
    }

    [void](New-ItemProperty -Path $optionsKey.PSPath -Name 'Debugger' -Value $debuggerPath -Force)
        
    if ($IncludeLoaderSnaps)
    {
        [void](New-ItemProperty -Path $optionsKey.PSPath -Name 'GlobalFlag' -Value 2 -Force)
    }
    else
    {        
        [void](New-ItemProperty -Path $optionsKey.PSPath -Name 'GlobalFlag' -Value 0 -Force)
    }
}
 
function Disable-StartWithDebugger 
{
    [CmdletBinding()]
    param (
        [Parameter(Position=0, Mandatory=$true, HelpMessage='Enter the name of the EXE to start with a debugger, like "calc.exe"')]
        [ValidatePattern({\.exe$})]
        [string]$ExeName
    )
        
    if (-not (Test-ProcessElevation))
    {
        Write-Error 'Must run this command in an elevated shell.'
        return
    }

    $imageOptionsKey = "HKLM:SOFTWARE\Microsoft\Windows NT\CurrentVersion\Image File Execution Options\$ExeName"
    Remove-Item -Path $imageOptionsKey -ErrorAction SilentlyContinue
}

function Start-EtwDataLogging
{
    [CmdletBinding()]
    param (
        [Parameter(Position=0, Mandatory=$true, HelpMessage='Enter the name of the EXE to start with a debugger, like "calc.exe"')]
        [ValidatePattern({\.exe$})]
        [string]$ExeName
    )
}