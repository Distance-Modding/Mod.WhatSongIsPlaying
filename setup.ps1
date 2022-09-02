Set-Variable Source -Option Constant -Value $MyInvocation.MyCommand.Definition;
Set-Variable ValidationRegex -Option Constant -Value "^([a-zA-Z]+([ ]?[a-zA-Z0-9])*)\r?$";

$Extensions = ".txt", ".json", ".env", ".sln", ".proj", ".csproj", ".vbproj", ".shproj", ".projitems", ".cs", ".vb", ".targets", ".props";

Class ProjectInfo {
	[string]$Namespace;
	[string]$Title;

	ProjectInfo([string]$namespace, [string]$title) {
		$this.Namespace = $namespace;
		$this.Title = $title;
	}
}

Function Read-ProjectInfo {
	[OutputType([ProjectInfo])]
	Param ()

	Write-Host "Enter a new project name to setup your mod (Example: 'My First Mod').";
	Write-Host "In order to be valid, a name must respect the following rules:";
	Write-Host "- The project name can't begin with a number";
	Write-Host "- The project name can't start or end with one or more spaces";
	Write-Host "- The name can't contain a series of more than 1 consecutive space character";
	Write-Host "- Words must be composed only of (latin) letters and numbers (a-zA-Z0-9)";
	Write-Host "- Any special characters are not allowed (@#-.;:/!§,?%=+* ...)";
	Write-Host "";
	Write-Host "The program will prompt you to type a name until it is a valid one.";
	Write-Host "To exit it, press CTRL + C";
	Write-Host "";

	Do {
		$ProjectName = Read-Host -Prompt "Enter project name";
	}
	Until ($ProjectName -match $ValidationRegex);

	Return [ProjectInfo]::new("Distance." + $ProjectName.Replace(" ", ""), "Distance " + $ProjectName);
}

Function Rename-Files {
	Param ([string]$Find, [string]$Replace)

	Rename-Items -Find $Find -Replace $Replace -Attributes "Directory";
	Rename-Items -Find $Find -Replace $Replace -Attributes "!Directory";
	Write-FilesContent -Find $Find -Replace $Replace
}

Function Rename-Items {
	Param ([string]$Find, [string]$Replace, [string]$Attributes)
	$Items = Get-ChildItem -Attributes $Attributes -Recurse | Where-Object -FilterScript {$_.FullName -Ne $Source};

	ForEach ($Item in $Items) {
		If ($Item.Name.Contains($Find)) {
			Rename-Item -Path $Item.PSPath -NewName $Item.Name.Replace($Find, $Replace);
		}
	}
}

Function Write-FilesContent {
	Param ([string]$Find, [string]$Replace)

	$Files = Get-ChildItem -File -Recurse | Where-Object -FilterScript {$_.FullName -Ne $Source -And $Extensions.Contains($_.Extension.ToLower())};

	ForEach ($File in $Files) {
		$Content = (Get-Content -Path $File.FullName -Encoding utf8 -Raw).Replace($Find, $Replace);
		Set-Content -Path $File.FullName -Value $Content -Encoding utf8;
	}
}

Function Remove-Script {
	[OutputType([bool])]
	Param ()

	$Yes = New-Object System.Management.Automation.Host.ChoiceDescription "&Yes", "Remove this powershell script from the disk.";
	$No = New-Object System.Management.Automation.Host.ChoiceDescription "&No", "Keep this powershell script on the disk.";

	$Options = [System.Management.Automation.Host.ChoiceDescription[]]($Yes, $No);

	$ChoiceResult = $Host.Ui.PromptForChoice("Setup complete!", "Do you want to delete this powershell script?", $Options, 0);

	Return $ChoiceResult -Eq 0;
}

Function Init {
	[ProjectInfo]$Info = Read-ProjectInfo;

	Rename-Files -Find "Mod.Template" -Replace $Info.Namespace;
	Rename-Files -Find "Distance.ModTemplate" -Replace $Info.Namespace;
	Rename-Files -Find "Mod Template" -Replace $Info.Title;

	[bool]$RemoveFiles = Remove-Script;
	If ($RemoveFiles) {
		Remove-Item -Path "$Source" -Force
	}
}

Init;