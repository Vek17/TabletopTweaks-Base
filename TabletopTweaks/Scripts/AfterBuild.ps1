param (
    [Parameter(Mandatory)]$BuildDir
)
Copy-Item -Path $BuildDir -Destination "C:\Program Files (x86)\Steam\steamapps\common\Pathfinder Second Adventure\Mods\TabletopTweaks - Development" -Recurse
Copy-Item -Path $BuildDir -Destination "C:\Program Files\Unity Mod Manager\Pathfinder Second Adventure\TabletopTweaks -Development"-Recurse