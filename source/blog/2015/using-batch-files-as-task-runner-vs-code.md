---
Title: Using batch files as the task runner in Visual Studio Code
Layout: Post
Permalink: 2015/07/09/using-batch-files-as-task-runner-vs-code.html
Date: 2015-07-09
Category: Visual Studio
Tags: Visual Studio, Visual Studio Code, Batch Files 
Comments: true
---

[Visual Studio Code](https://code.visualstudio.com/) allows you specify tasks which can be in a task runner. Most examples
I've seen show how to integrate with Javascript task runners such as Gulp. There is no reason why you can't simpley use
batch files though.

<!--more-->

An example tasks.json:

```js
{
	"version": "0.1.0",
	"command": "${workspaceRoot}/runTask.bat",
	"isShellCommand": true,
	"showOutput": "always",
	"args": [],
	"tasks": [
		{
			"taskName": "build",
			"isBuildCommand": true,
			"showOutput": "always"
		},
		{
			"taskName": "serve",
			"showOutput": "never"
		}
	]
}
```

This sets the batch file "runTask.bat" in the root of your workspace as the task runner. There are two tasks
which can be started: "build" and "serve". "build" can be started with "ctrl+shift+T" and "serve" can be
started via "ctrl+shift+P" and typing "run task". I like to map this shortcut to "ctrl+shift+R".

Here is how runTask.bat should look:

```
@ECHO OFF

IF "%1" == "build" GOTO BUILD
IF "%1" == "serve" GOTO SERVE
GOTO UNKNOWN

:BUILD
CALL build.bat
GOTO END

:SERVE
START serve.bat
GOTO END

:UNKNOWN
ECHO Unknown Task
GOTO END

:END
```

 
