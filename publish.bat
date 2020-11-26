@ECHO OFF
PUSHD %~dp0

SET __HAS_CHANGES=0

FOR /f "tokens=*" %%i IN ('git status -s') DO ( SET __HAS_CHANGES=1 )

IF "%__HAS_CHANGES%" NEQ "0" (
    ECHO There are currently uncommitted changes.
    GOTO EXIT
)

IF EXIST bin (
    IF EXIST bin/.git (
        PUSHD bin
        git reset --hard
        POPD
    ) else (
        RMDIR /s /q bin
        git clone "https://github.com/smack0007/smack0007.github.io.git" -b master bin
    )
)

dotnet build .\compiler\compiler.csproj -p:BuildSite=true -nologo

git log -1 --pretty=%%B > commit.msg

PUSHD bin
git config user.name "smack0007"
git config user.email "zachary.snow@gmail.com"
git add -A && git commit -F "..\commit.msg"
git push https://github.com/smack0007/smack0007.github.io.git head:master
POPD

DEL commit.msg

:EXIT
POPD
