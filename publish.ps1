# Purpose: Publish the application for Windows

# Define target frameworks and runtime identifiers
$artifactsDir = ".artifacts"
$publishDir = "publish"
$fullPublishDir = "$artifactsDir/$publishDir"
$scriptDir = Split-Path -Path $MyInvocation.MyCommand.Definition -Parent
$projectFile = "Frank.ServiceBusExplorer.Cli/Frank.ServiceBusExplorer.Cli.csproj"

# Change to the script directory
Set-Location $scriptDir

# Clean publish directory if it exists
if (Test-Path $fullPublishDir)
{
    Write-Host "Cleaning publish directory $fullPublishDir"
    Remove-Item -Recurse -Force $fullPublishDir
}

# Create publish directory
New-Item -ItemType Directory -Force -Path $fullPublishDir

# Publish cross-platform
dotnet publish $projectFile -c Release -o $fullPublishDir

if ($LASTEXITCODE -ne 0)
{
    Write-Host "dotnet publish failed with exit code $LASTEXITCODE"
    exit $LASTEXITCODE
}

Write-Host "Build completed successfully"