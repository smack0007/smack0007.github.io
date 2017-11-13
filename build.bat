@ECHO OFF
PUSHD %~dp0

IF EXIST bin (
    RMDIR /s /q bin
)

git clone "https://github.com/smack0007/smack0007.github.io.git" bin
dotnet build .\compiler\compiler.csproj -p:BuildSite=true

git log -1 --pretty=%B > commit.msg

PUSHD bin
git config user.name "smack0007"
git config user.email zachary.snow@gmail.com
git add -A && git commit -F "..\commit.msg"
POPD

DEL commit.msg

POPD

EXIT 0