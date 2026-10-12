#!/bin/bash
ROOT_PATH=$(dirname $0)
cd "$ROOT_PATH"

if [ -d ./bin ]; then rm -rf ./bin; fi

npx tsc
node ./bin/compiler/main.js
rm -rf ./bin/compiler && rm -rf ./bin/src
