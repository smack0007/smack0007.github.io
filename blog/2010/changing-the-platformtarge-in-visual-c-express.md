---
Title: Changing the PlatformTarget in Visual C# Express
Layout: Post
Permalink: 2010/01/18/changing-the-platformtarget-in-visual-c-express.html
Date: 2010-01-18
Category: .NET
Tags: MSBuild 
Comments: true
---

Some project types in Visual C# Express (Empty Project) will not allow you to change the PlatformTarget from the UI. You can still change the target platform though by editing the .csproj file in a text editor. Close the project and open it up in your favorite text editor (I use [Notpad++](http://notepad-plus.sourceforge.net/)). The .csproj file is really just a XML file. You should see somewhere in the file something like:

```xml
	&lt;PropertyGroup Condition=" '$(Configuration)|$(Platform)' == 'Debug|AnyCPU' "&gt;
		....
	&lt;/PropertyGroup&gt;
	&lt;PropertyGroup Condition=" '$(Configuration)|$(Platform)' == 'Release|AnyCPU' "&gt;
		....
	&lt;/PropertyGroup&gt;
```

Inside the PropertyGroup elements, add the PlatformTarget element:

```xml
	&lt;PropertyGroup Condition=" '$(Configuration)|$(Platform)' == 'Debug|AnyCPU' "&gt;
		....
		&lt;PlatformTarget&gt;x86*&lt;/PlatformTarget&gt;
	&lt;/PropertyGroup&gt;
	&lt;PropertyGroup Condition=" '$(Configuration)|$(Platform)' == 'Release|AnyCPU' "&gt;
		....
		&lt;PlatformTarget&gt;x86&lt;/PlatformTarget&gt;
	&lt;/PropertyGroup&gt;
```

Save the file and open your project back up. Your project's output should now target only x86.


