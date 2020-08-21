---
Title: Creating a tsexec command
Subtitle: 
Date: 2020-08-21
Tags: typscript, batch
---

I want to use TypeScript as a scripting language for my machine and
so I want to be able just to execute TypeScript files directly. There
are npm packages like [ts-node](https://www.npmjs.com/package/ts-node)
that do this already but I wanted to have a crack at implementing it
myself. So this is the batch file I've come up with so far.

<!--more-->

```bat
@ECHO OFF
SETLOCAL enableextensions 

REM Get the location of the the npm-prefix directory.
FOR /f "tokens=*" %%a in ( 'npm config get prefix' ) DO ( SET __NPM_PREFIX=%%a ) 
REM Remove trailing space from npm-prefix.
SET __NPM_PREFIX=%__NPM_PREFIX:~0,-1%

SET __SCRIPTFILE=%1
SET __OUTPUTFILE=%__SCRIPTFILE%

REM Cut the ts or tsx file extension.
IF "%__OUTPUTFILE:~-3%" == ".ts" (
    SET __OUTPUTFILE=%__OUTPUTFILE:~0,-3%
) ELSE IF "%__OUTPUTFILE:~-4%" == ".tsx" (
    SET __OUTPUTFILE=%__OUTPUTFILE:~0,-4%
)

SET __OUTPUTFILE=%__OUTPUTFILE%.js

REM Get all arguments except the first.
SET __SCRIPTARGS=
FOR /f "tokens=1,* delims= " %%a IN ("%*") DO ( SET __SCRIPTARGS=%%b )

tsc --strict --typeRoots "%__NPM_PREFIX%\node_modules\@types" %__SCRIPTFILE% && node %__OUTPUTFILE% %__SCRIPTARGS%

ENDLOCAL
```

I've got the batch file in my path so I can just execute scripts now like this:

```bat
tsexec hello.ts Zac
```

This produces a hello.js file directly next to the hello.ts file. Any imported files
will also be compiled with the output file landing directly next to the input file.