# 1. Overview
This repository holds code used for my presentation "How to avoid common mistakes and misconceptions when working with Java on AWS Lambda".

# 2. Tools needed to compiple and deploy thecode
* JDK11+
* CDK CLI and all of its dependencies installed
* AWS account configured in the `~/.aws/config` file, the CDK code assumes the account is defined there

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
