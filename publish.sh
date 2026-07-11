#!/bin/bash
ROOT_PATH=$(dirname $0)
cd "$ROOT_PATH"

if [ -d ./bin ]; then rm -rf ./bin; fi

git clone https://github.com/smack0007/smack0007.github.io.git -b pages ./bin
deno task build:ci

git log -1 --pretty=%B > commit.msg
cd ./bin
git add -A && git commit -F "../commit.msg"
git push
rm ../commit.msg