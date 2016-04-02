
function Enable-FusionLog 
{
    [CmdletBinding()]
    param (
        [bool]$all = $false
    ) 
    [void](New-ItemProperty  HKLM:Software\Microsoft\Fusion -name EnableLog -propertyType dword -ErrorAction SilentlyContinue)  
    [void](New-ItemProperty  HKLM:Software\Microsoft\Fusion -name ForceLog -propertyType dword -ErrorAction SilentlyContinue)  
    [void](New-ItemProperty  HKLM:Software\Microsoft\Fusion -name LogFailures -propertyType dword -ErrorAction SilentlyContinue)  
    [void](New-ItemProperty  HKLM:Software\Microsoft\Fusion -name LogPath -propertyType string -ErrorAction SilentlyContinue)
    Set-ItemProperty  HKLM:Software\Microsoft\Fusion -name EnableLog -value 1 -ErrorAction Stop 
    if($all -eq $true) {
        Set-ItemProperty  HKLM:Software\Microsoft\Fusion -name ForceLog -value 1 -ErrorAction Stop 
    }
    Set-ItemProperty  HKLM:Software\Microsoft\Fusion -name LogFailures -value 1 -ErrorAction Stop 
    Set-ItemProperty  HKLM:Software\Microsoft\Fusion -name LogPath -value 'C:\logs\' -ErrorAction Stop 
    Write-Host "Fusion log enabled. Use Disable-FusionLog to disable." 
}
 
function Disable-FusionLog 
{ 
    [CmdletBinding()]
    param()
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
        [Parameter(Position=1, Mandatory=$false, HelpMessage='Which debugger to use?')]
        [ValidateSet('windbg', 'cdb', 'vsjitdebugger')]
        [string]$Debugger,
        [Parameter(Mandatory=$false, HelpMessage='Use a full path to a specific debugger.')]
        [ValidateNotNullOrEmpty]
        [string]$FullPathDebugger,
        [Parameter(HelpMessage='Include to enable loader snaps in debugger output.')]
        [switch]$IncludeLoaderSnaps
    )

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
        if (Test-Path (join-path $env:ProgramFiles "Debugging Tools for Windows (x64)\"))
        {
            $debugToolsPath = (join-path $env:ProgramFiles "Debugging Tools for Windows (x64)\")
        }
        elseif (Test-Path (join-path $env:ProgramFiles "Debugging Tools for Windows (x86)\"))
        {
            $debugToolsPath = (join-path $env:ProgramFiles "Debugging Tools for Windows (x86)\")
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