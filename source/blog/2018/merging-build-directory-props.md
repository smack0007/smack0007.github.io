---
Title: Merging Build.Directory.props
Subtitle: 
Date: 2018-04-04
Tags: msbuild, .net
---

MSBuild version 15 introduced the concept of "Directory.Build.props" files. From the docs:

- `Directory.Build.props` is a user-defined file that provides customizations to projects under a directory. This
   file is automatically imported from Microsoft.Common.props unless the property `ImportDirectoryBuildTargets` is
   set to false.

What is not stated here is that only one `Directory.Build.props` will be imported automatically. Imagine your project
exists in the directory `C:\repo\src\foo\foo.csproj` and there exists a file in both `C:\repo\src\Directory.Build.props` and
`C:\repo\Directory.Build.props` then only `C:\repo\src\Directory.Build.props` will be automatically included when building
`C:\repo\src\foo\foo.csproj`. If you would like `C:\repo\Directory.Build.props` to be included as well, then 
`C:\repo\src\Directory.Build.props` will have to include `C:\repo\Directory.Build.props` like so:

```xml
<Project>
    <Import Project="$([MSBuild]::GetPathOfFileAbove('Directory.Build.props', '$(MSBuildThisFileDirectory)../'))" />
</Project>
```

This is documented [here](https://docs.microsoft.com/en-us/visualstudio/msbuild/customize-your-build#use-case-multi-level-merging).