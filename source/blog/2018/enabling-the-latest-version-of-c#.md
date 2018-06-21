---
Title: Enabling the latest version of C#
Subtitle: 
Date: 2018-06-21
Category: c#
Tags: c#, msbuild
---

As of this writing when creating a new console project via `dotnet new console` the version of C# used in the project
is version 7.0. This means you're missing out cool features like [Default Literal Expressions](https://docs.microsoft.com/en-us/dotnet/csharp/whats-new/csharp-7-1#default-literal-expressions). The language version
can be changed via Visual Studio though I prefer to enable it via MSBuild. This can be done in the `csproj` file
or globally via a `Build.Directory.props` file. Changing the C# language version is done via a property known as `LangVersion`.

```xml
<PropertyGroup>
    <LangVersion>latest</LangVersion>
</PropertyGroup>
```

 This property can set to any of the values listed [here](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/compiler-options/langversion-compiler-option). As of
this writing VS Code doesn't seem to like it when you use 7.3 instead of latest. VS Code will show errors in your code
although the code will compile without any problems.