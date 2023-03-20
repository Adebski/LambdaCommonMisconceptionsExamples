# 1. Overview
This repository holds code used for my presentation "How to avoid common mistakes and misconceptions when working with Java on AWS Lambda".

Slides from JDD 2022: https://www.slideshare.net/adebski/jdd2022-how-to-avoid-common-mistakes-and-misconceptions-when-working-with-java-on-aws-lambda.

# 2. Tools needed to compiple and deploy thecode
* JDK11+
* CDK CLI and all of its dependencies installed
* AWS account configured in the `~/.aws/config` file, the CDK code assumes the account is defined there
* Installed .net 6 SDK and runtime. Follow instructions from https://learn.microsoft.com/en-us/dotnet/core/install/linux-ubuntu-1804.
* Install the `Amazon.Lambda.Tools` extensions to the .NET CLI: `dotnet tool install -g Amazon.Lambda.Tools`.

You can execute `package-and-deploy.sh` to compiple, package, and execute the code through the CDK to your
AWS account. 

## 2.1. Troubleshooting

### 2.1.1. Cannot find name 'process' during `cdk synth`
For some reason the `@types/node` dependency was not download/present despite
it being declared in the `package.json`. 

Following the suggestions in https://stackoverflow.com/questions/53529521/typescript-error-cannot-find-name-process 
you can manually download the depndency. This should resolve the issue.
```
npm i --save-dev @types/node
```

# 3. Additional reading
Using AWS Lambda with GraalVM:
* https://github.com/aws-samples/serverless-graalvm-demo
* https://www.formkiq.com/blog/tutorials/aws-lambda-graalvm/
* https://guides.micronaut.io/latest/mn-application-aws-lambda-graalvm-gradle-java.html
* https://shinesolutions.com/2021/08/30/improving-cold-start-times-of-java-aws-lambda-functions-using-graalvm-and-native-images/
