@ECHO OFF
PUSHD %~dp0

SET SCRIPT="%~dp0\scripts\%1.csx"

IF NOT EXIST "%SCRIPT%" (
    ECHO "Unknown Task"
    GOTO END
)

REM Get all arguments except the first.
FOR /f "tokens=1,* delims= " %%a IN ("%*") DO ( SET SCRIPTARGS=%%b )

ECHO %SCRIPTARGS%

dotnet script %SCRIPT% -- %SCRIPTARGS%

:END
POPD