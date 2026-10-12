#!/bin/bash
ROOT_PATH=$(dirname $0)
cd "$ROOT_PATH"

if [ -d ./bin ]; then rm -rf ./bin; fi

deno run --allow-env --allow-read=. --allow-run --allow-write=./bin ./compiler/main.ts
