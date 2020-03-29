# Paths
$packFolder = (Get-Item -Path "./" -Verbose).FullName
$rootFolder = Join-Path $packFolder "../"

# List of projects
$projects = (

    "src/EasyAbp.Abp.RelatedDtoLoader",
    "src/EasyAbp.Abp.RelatedDtoLoader.Abstractions"
)