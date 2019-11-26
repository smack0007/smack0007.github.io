@ECHO OFF
PUSHD %~dp0

IF EXIST bin (
    RMDIR /s /q bin
)

git clone "https://github.com/smack0007/smack0007.github.io.git" -b master bin
dotnet build .\compiler\compiler.csproj -p:BuildSite=true

git log -1 --pretty=%%B > commit.msg

PUSHD bin
git add -A && git commit -F "..\commit.msg"
git push https://github.com/smack0007/smack0007.github.io.git head:master
POPD

DEL commit.msg

POPD
