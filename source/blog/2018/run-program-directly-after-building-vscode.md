---
Title: Run a program directly after building in VS Code
Subtitle: 
Date: 2018-04-11
Category: .net 
Tags: .net, .netcore, msbuild, vscode
---

Whenever I have a program that is just a generator of some kind I like to have that
program execute directly after having built the program successfully. I'm going to show
how to run the program via the dotnet cli but this trick can easily be applied to regular
.NET programs or any program which is built using MSBuild.

<!--more-->

Add the following to your csproj file:

```xml
<Target Name="ExecuteAfterBuild" AfterTargets="AfterBuild" Condition=" $(ExecuteAfterBuild) == true ">
    <Exec WorkingDirectory="$(OutputPath)" Command="dotnet $(AssemblyName).dll" />
</Target>
```

This instructs MSBuild to execute the dotnet cli in the output directory of the assembly if the property
`ExecuteAfterBuild` is set to true. This can be accomplished via the dotnet cli:

```bat
dotnet build MyProj.csproj -p:ExecuteAfterBuild=true
```

Or if you're using VS Code you can set it as a parameter in your tasks.json:

```json
{
    "version": "2.0.0",
    "tasks": [
        {
            "label": "generate",
            "group": "build",
            "command": "dotnet",
            "type": "process",
            "args": [
                "build",
                "${workspaceFolder}/MyProj.csproj",
                "-p:ExecuteAfterBuild=true"
            ],
            "problemMatcher": "$msCompile"
        }
    ]
}
```
Then you can use the build shortcut `Ctrl + Shift + B` to build your project and
execute it directly.
