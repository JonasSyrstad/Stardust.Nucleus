
param($installPath, $toolsPath, $package, $project)
Write-Host "Updating AppBlueprint.partial.cs"
$project.ProjectItems | where { $_.Name -eq "App_Start" } |
ForEach { $_.ProjectItems } |  where { $_.Name -eq "AppBlueprint.partial.cs"} |
ForEach { 
	Write-Host "Deleting AppBlueprint.partial.cs"
	$_.Delete();
}

$project.ProjectItems | where { $_.Name -eq "App_Start" } |
ForEach { $_.ProjectItems } |  where { $_.Name -eq "AppBlueprint.cs"} |
ForEach { $_.ProjectItems } |  where { $_.Name -eq "AppBlueprint.partial.cs"} |
ForEach { 
	Write-Host "Deleting AppBlueprint.partial.cs nested"
	$_.Delete();
}
Write-Host "blueprint rewrite"
$project.ProjectItems | where { $_.Name -eq "App_Start" } |
ForEach { $_.ProjectItems } |  where { $_.Name -eq "AppBlueprint.partial.cs.temp"} |
ForEach { 
	$_.Name = "AppBlueprint.partial.cs" 
}



$hasBlueprint=$false
$project.ProjectItems | where { $_.Name -eq "App_Start" } |
ForEach { $_.ProjectItems } |  where { $_.Name -eq "AppBlueprint.cs"} |
ForEach { 
	
	$hasBlueprint=$true
}
if($hasBlueprint){Write-Host "blueprint exists, skipping it"}
else{
Write-Host "blueprint rewrite"
$project.ProjectItems | where { $_.Name -eq "App_Start" } |
ForEach { $_.ProjectItems } |  where { $_.Name -eq "AppBlueprint.cs.temp"} |
ForEach { 
	$_.Name = "AppBlueprint.cs" 
}
}
Write-Host "temp blueprint delete"
$project.ProjectItems | where { $_.Name -eq "App_Start" } |
ForEach { $_.ProjectItems } |  where { $_.Name -eq "AppBlueprint.cs.temp"} |
ForEach { $_.Delete() }

Write-Host "Nest blueprint Files"


#ForEach($t in $project.ProjectItems)
#{
#Write-Host $t.Name
#}
$appStart = $project.ProjectItems | Where-Object { $_.Name -eq "App_Start"  }
$service = $appStart.ProjectItems | Where-Object { $_.Name -eq "AppBlueprint.cs" -and  $_.ProjectItems.Count -eq 0 }

if($service -eq $null)
{
	Write-Host "unable to locate AppBlueprint.cs"
    return
}

$designer = $appStart.ProjectItems | Where-Object { $_.Name -eq "AppBlueprint.partial.cs"  }

if($designer -eq $null)
{
Write-Host "unable to locate AppBlueprint.partial.cs"
    return
}

Write-Host "Executing nesting of AppBlueprint files"
$nestResult =$service.ProjectItems.AddFromFile($designer.Properties.Item("FullPath").Value)
