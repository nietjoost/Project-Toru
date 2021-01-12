#!/bin/sh

echo "To use this script, install dos2unix"
echo "ex: brew install dos2unix"

cd ..
find ./Assets -name "*.cs" -maxdepth 1 | xargs -I {} dos2unix {}
find ./Assets/Scripts -name "*.cs" | xargs -I {} dos2unix {}