---
Title: Changing the PlatformTarget in Visual C# Express
Date: 2010-01-18
Tags: .net, msbuild
---

Some project types in Visual C# Express (Empty Project) will not allow you to change the PlatformTarget from the UI. You can still change the target platform though by editing the .csproj file in a text editor. Close the project and open it up in your favorite text editor (I use [Notpad++](http://notepad-plus.sourceforge.net/)). The .csproj file is really just a XML file. You should see somewhere in the file something like:

```xml
	<PropertyGroup Condition=" '$(Configuration)|$(Platform)' == 'Debug|AnyCPU' ">
		....
	</PropertyGroup>
	<PropertyGroup Condition=" '$(Configuration)|$(Platform)' == 'Release|AnyCPU' ">
		....
	</PropertyGroup>
```

Inside the PropertyGroup elements, add the PlatformTarget element:

```xml
	<PropertyGroup Condition=" '$(Configuration)|$(Platform)' == 'Debug|AnyCPU' ">
		....
		<PlatformTarget>x86</PlatformTarget>
	</PropertyGroup>
	<PropertyGroup Condition=" '$(Configuration)|$(Platform)' == 'Release|AnyCPU' ">
		....
		<PlatformTarget>x86</PlatformTarget>
	</PropertyGroup>
```

Save the file and open your project back up. Your project's output should now target only x86.


