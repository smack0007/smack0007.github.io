#!/bin/bash
ROOT_PATH=$(dirname $0)
cd "$ROOT_PATH"

if [ "$1" != "-publish" ]; then
  if [ -d ./bin ]; then rm -rf ./bin; fi
fi

npx tsc
[ "$1" = "-publish" ] && node ./bin/compiler/main.js -publish || node ./bin/compiler/main.js
rm -rf ./bin/compiler && rm -rf ./bin/src
