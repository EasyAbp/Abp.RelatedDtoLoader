# Paths
$packFolder = (Get-Item -Path "./" -Verbose).FullName
$rootFolder = Join-Path $packFolder "../"

# List of projects
$projects = (

    "src/EasyAbp.Abp.RelatedDtoLoader.Application",
    "src/EasyAbp.Abp.RelatedDtoLoader.Application.Contracts"
)