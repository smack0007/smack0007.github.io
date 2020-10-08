---
Title: Angular in Visual Studio
Subtitle: 
Date: 2020-10-08
Tags: Angular, VisualStudio
---

Now that the [Angular Language Service](https://devblogs.microsoft.com/visualstudio/angular-language-service-for-visual-studio/)
is available for Visual Studio working on Angular projects from inside Visual Studio is a viable option. The linked blog
post mentions a couple of ways of adding the Angular project to Visual Studio but I felt the suggestions left something
to be desired here is what I came up with.

<!--more-->

Here is the csproj file I came up with:

```xml
<Project>
  <Import Project="Sdk.props" Sdk="Microsoft.NET.Sdk.Web" />
  
  <PropertyGroup>
    <TargetFramework>netcoreapp3.1</TargetFramework>
    <TypeScriptCompileBlocked>true</TypeScriptCompileBlocked>
    <IsPackable>false</IsPackable>

    <!-- Don't show node_modules -->
    <DefaultItemExcludes>$(DefaultItemExcludes);node_modules\**</DefaultItemExcludes>
  </PropertyGroup>

  <ItemGroup>
    <!-- Properties folder really only exists to make VisualStudio happy. -->
    <Content Remove="Properties/*" />
  </ItemGroup>
  
  <Import Project="Sdk.targets" Sdk="Microsoft.NET.Sdk.Web" />

  <Target Name="Restore">
    <Exec Command="npm i" />
  </Target>
  
  <Target Name="Build">
    <Exec Command="npm run build" />
  </Target>
  
</Project>
```

And here is what I ended up with in my launchSettings.json file:

```json
{
  "profiles": {
    "ng serve": {
      "commandName": "Executable",
      "workingDirectory": "$(ProjectDir)",
      "executablePath": "node_modules\\.bin\\ng.cmd",
      "commandLineArgs": "serve"
    }
  }
}
```
