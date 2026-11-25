#!/bin/bash
ROOT_PATH=$(dirname $0)
cd "$ROOT_PATH/bin"
python3 -m http.server
