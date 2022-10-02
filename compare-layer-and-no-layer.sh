#!/usr/bin/env bash

set -o xtrace    # expands variables and prints a little + sign before the line
set -o errexit   # abort on nonzero exitstatus
set -o nounset   # abort on unbound variable
set -o pipefail  # don't hide errors within pipes

for i in {1..100}
do
 echo $i
 ## Hello world layer
 time aws lambda invoke --function-name arn:aws:lambda:us-east-1:690588305730:function:LambdaCommonMisconception-lambdajavacommonmisconce-Jcqfp3ofNXmR --payload '{"testNumber": 15 }' --cli-binary-format raw-in-base64-out  /dev/stdout

 ## Hello world
 time aws lambda invoke --function-name arn:aws:lambda:us-east-1:690588305730:function:LambdaCommonMisconception-lambdajavacommonmisconce-U4viAfbnb69i --payload '{"testNumber": 15 }' --cli-binary-format raw-in-base64-out  /dev/stdout
done