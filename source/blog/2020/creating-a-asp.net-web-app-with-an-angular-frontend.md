---
Title: Creating a asp.net web app with an Angular frontend
Subtitle: WebShell
Date: 2020-10-02
Tags: asp.net, netcore, angular
---

We're going to create a what I call a "WebShell" application with asp.net core on the backend and Angular on the
frontend.

<!--more-->

Let's create the backend first:

```cmd
dotnet new webapi -n WebShellBackend -o backend
```

Which just means to create a `webapi` project with the name "WebShellBackend" and place the output in a folder named
"backend". Open up the project in Visual Studio or vscode and replace the launchSettings.json in Properties folder with
the following:

```json
{
  "iisSettings": {
    "windowsAuthentication": false,
    "anonymousAuthentication": true,
    "iisExpress": {
      "applicationUrl": "http://localhost:54317",
      "sslPort": 44326
    }
  },
  "$schema": "http://json.schemastore.org/launchsettings.json",
  "profiles": {
    "WebShellBackend": {
      "commandName": "Project",
      "environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Development"
      },
      "applicationUrl": "https://localhost:5001;http://localhost:5000"
    }
  }
}
```

We want all the urls for our endpoints on the backend to be prefixed with "/api" so the we need to the route for the
example controller to `api/[controller]`:

```cs
[ApiController]
[Route("api/[controller]")]
public class WeatherForecastController : ControllerBase
{
    // The content of the controller is not important for us here.
}
```

Once you've made that change you can start up the backend and visit "https://localhost:5001/api/weatherforecast" in
your web browser and you should see the reponse as json. In chrome I had to explicitly type the entire url including
the "https" othwerwise it would not work.

Now on to the frontend. From the root folder of your application again execute the following command:

```cmd
ng new web-shell-frontend --directory frontend
```

Now in the newly created frontend we need to add a file to the root folder of the Angular project called
`proxy.conf.json` with the following contents:

```json
{
    "/api/*": {
        "target": "https://localhost:5001",
        "secure": false,
        "logLevel": "debug"
    }
}
```

This file will tell the Angular development server that start with "/api/" should be forwarded to our backend. We need
to add a reference to this file in our `angular.json` file. Look for the section `options` under `serve` (line 68
on my machine) and add the following `proxyConfig` key:

```json
"options": {
  "browserTarget": "web-shell-frontend:build",
  "proxyConfig": "proxy.conf.json"
},
```

The last change to our frontend is we will make the app component call our backend and display the json that is
returned. Change `src/app.component.ts` to:

```ts
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  weather = '';

  public ngOnInit(): void {
    fetch('/api/weatherforecast').then((response) => {
      response.json().then(json => {
          this.weather = json;
      });
    });
  }
}
```

And then change `src/app.component.html` to:

```html
<h1>WebShell Frontend</h1>

{{ weather | json }}
```

Now start up your frontend with `ng serve` and visit `http://localhost:4200`. You should see the json your backend
delivers if everything works. You should see a message from the Angular dev server indicating that a redirect occured:

```
[HPM] GET /api/weatherforecast -> https://localhost:5001
```

Now to marry the two together. We need first enable the backend to serve the static files created by our frontend. On
the backend in the `Configure` method of the `Startup.cs` file add calls to `app.UseDefaultFiles()` and
`app.UseStaticFiles()` between the calls to `app.UseHttpsRedirection()` and `app.UseRouting()`. The order of the calls
is important:

```cs
app.UseHttpsRedirection();

app.UseDefaultFiles();
app.UseStaticFiles();

app.UseRouting();
```

In `Program.cs` add the following line to the beginning of the `ConfigureWebHostDefaults` method:

```cs
#if RELEASE
webBuilder.UseContentRoot(Path.Combine(Path.GetDirectoryName(typeof(Program).Assembly.Location), "wwwroot"));
#endif
```

Which just ensures the content root is always in the same place regardless of where the assembly is exeucted from. In
the `WebShellBackend.csproj` file add the following to the end of the `Project` node:

```xml
<PropertyGroup Condition="$(Configuration) == 'Release'">
  <AppendTargetFrameworkToOutputPath>false</AppendTargetFrameworkToOutputPath>
  <OutputPath>$(MSBuildThisFileDirectory)../bin</OutputPath>
</PropertyGroup>
```

This will force the output of the backend to be placed in the `/bin` directory when building in `Release` mode.

Now in the `angular.json` file of the frontend find the `outputPath` for the section `options` under `build` (line
20 on my machine) and set the path to `../bin/wwwroot`

```json
"outputPath": "../bin/wwwroot",
```

Now we'll create a `build.cmd` file which will build both the backend and the frontend:

```bat
@ECHO OFF
PUSHD %~dp0

PUSHD backend
ECHO Building backend...
dotnet build -c Release
POPD

PUSHD frontend
ECHO Building frontend...
CALL ng build --prod
POPD

POPD
```

After building the `/bin` directory should contain both the output for the backend and the frontend. The backend
output being directly in `/bin` and the frontent being in `/bin/wwwroot`. Once you've started the app you should be
able to visit `https://localhost:5001` and have the frontend served to the browser.

