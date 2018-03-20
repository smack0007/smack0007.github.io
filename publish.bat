PUSHD %~dp0

PUSHD bin
git push https://%1@github.com/smack0007/smack0007.github.io.git head:master
POPD

POPD

EXIT 0