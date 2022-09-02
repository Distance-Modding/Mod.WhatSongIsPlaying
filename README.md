# Distance mod template

Use this repository as a base to create your own Distance mods.

# Using the template
On github, click the `Use this template` button to create a new repository based on this one

Once the repository is created, be sure to clone it locally **with submodules**:
```sh
git clone --recurse-submodules -j8 <your project .git url>
```

# Setting up the project
After cloning the repository on your local drive, run the [setup.ps1](setup.ps1) script using [PowerShell 7 or higher](https://github.com/PowerShell/PowerShell#get-powershell).

This script will rename the files correctly and replace their content to match your mod name.