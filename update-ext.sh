#!/bin/bash
ROOT_PATH=$(realpath $(dirname $0))
cd "$ROOT_PATH"

# if [ -d ./tmp ]; then rm -rf ./tmp; fi

# mkdir -p ./tmp
cd ./tmp

# npm install --ignore-scripts highlight.js@11.7.0 html-entities@2.3.3 marked@4.2.3

if [ -d "${ROOT_PATH}/ext/html-entities" ]; then rm -rf "${ROOT_PATH}/ext/html-entities"; fi
mv ./node_modules/html-entities ${ROOT_PATH}/ext