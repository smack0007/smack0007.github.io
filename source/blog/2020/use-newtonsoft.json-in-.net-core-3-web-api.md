---
Title: Use Newtonsoft.Json in .NET Core 3 Web Api
Subtitle: 
Date: 2020-10-22
Tags: .net, .netcore, asp.net
---

This post is mostly just distilling information I got from
[.NET Core Tutorials](https://dotnetcoretutorials.com/2019/12/19/using-newtonsoft-json-in-net-core-3-projects/) to
revert back to [Newtonsoft.Json](https://www.newtonsoft.com/json) in asp.net core WebApi.

<!--more-->

In your csproj add the `PackageReference`:

```xml
<ItemGroup>
    <PackageReference Include="Microsoft.AspNetCore.Mvc.NewtonsoftJson" Version="3.1.9" />
</ItemGroup>
```

And then in the `ConfigureServices` of the `Startup` class add the `.AddNewtonsoftJson()` after the call to
`.AddControllers()`:

```csharp
public void ConfigureServices(IServiceCollection services)
{
    services.AddControllers().AddNewtonsoftJson();
}
```
