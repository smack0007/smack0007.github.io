---
Title: Execute different commands in MSBuild depending on platform
Subtitle: 
Date: 2020-11-20
Tags: msbuild
---

Another quick snippet that shows how to change what command will be executed in MSBuild
based on the OS the script is running on.

<!--more-->

```xml
<Target Name="GenerateFiles" BeforeTargets="CoreGenerateAssemblyInfo">
  <PropertyGroup>
    <_GenerateFilesCommand Condition="'$(OS)' == 'Windows_NT'">foo.exe</_GenerateFilesCommand>
    <_GenerateFilesCommand Condition="'$(OS)' == 'Unix'">mono foo.exe</_GenerateFilesCommand>
  </PropertyGroup>
  
  <Exec Command="$(_GenerateFilesCommand)" />
</Target>
```
