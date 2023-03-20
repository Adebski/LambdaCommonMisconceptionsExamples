#!/usr/bin/env bash

set -o xtrace    # expands variables and prints a little + sign before the line
set -o errexit   # abort on nonzero exitstatus
set -o nounset   # abort on unbound variable
set -o pipefail  # don't hide errors within pipes

rm -f assets/*

mvn package

cd handlers-net/
dotnet lambda package
cd ../

# Prepare the Lambda .jar function
cp handlers/target/*.jar assets/
# Prepare the .NET Lambda handler
cp ./handlers-net/bin/Release/net6.0/handlers-net.zip assets/

# Prepare the .zip file with the Lambda layer

## Create zip
archive="hello-world-extension.zip"
cd layer/
zip "$archive" -j target/layer-1.0.jar
zip "$archive" extensions/hello-world-extension
mv "$archive" ../assets/
cd ../

cd cdk
cdk synth && cdk deploy