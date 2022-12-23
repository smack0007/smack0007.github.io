---
Title: Checking for uncommited git changes in a batch program
Subtitle: 
Date: 2020-07-22
Tags: batch, git
---

Sometimes it's helpful to exit out of a batch script if there are pending
changes to a repo. This is done in batch script in a non intuitive way.

```bat
@ECHO OFF
PUSHD %~dp0

SET __HAS_CHANGES=0

REM The loop won't be executed if 'git status -s' doesn't produce any output.
FOR /f "tokens=*" %%i IN ('git status -s') DO ( SET __HAS_CHANGES=1 )

REM Check if the loop was executed and goto EXIT if it was.
IF "%__HAS_CHANGES%" NEQ "0" (
    ECHO There are currently uncommitted changes.
    GOTO EXIT
)

REM Do the work you want to do here.

:EXIT
POPD
```