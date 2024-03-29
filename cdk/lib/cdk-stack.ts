import { Stack, StackProps, aws_s3, aws_lambda, Duration } from 'aws-cdk-lib';
import { Construct } from 'constructs';
// import * as sqs from 'aws-cdk-lib/aws-sqs';

export class CdkStack extends Stack {
  constructor(scope: Construct, id: string, props?: StackProps) {
    super(scope, id, props);

    const helloWorldLayer = new aws_lambda.LayerVersion(this, 'hello-world-layer', {
      compatibleRuntimes: [
        aws_lambda.Runtime.JAVA_11
      ],
      code: aws_lambda.Code.fromAsset("../assets/hello-world-extension.zip"),
      description: 'Simple hello world external extension/layer',
    });

    const lambdaHelloWorld = new aws_lambda.Function(this, 'lambda-java-common-misconceptions-hello-world', {
      description: new Date().toISOString(),
      runtime: aws_lambda.Runtime.JAVA_11,
      memorySize: 256,
      handler: "com.adebski.LambdaJavaCommonMisconceptionsHelloWorld",
      code: aws_lambda.Code.fromAsset("../assets/handlers-1.0.jar"),
      timeout: Duration.seconds(30),
      reservedConcurrentExecutions: 3
    });

    const lambdaHelloWorldNet = new aws_lambda.Function(this, 'lambda-net-common-misconceptions-hello-world', {
      description: new Date().toISOString(),
      runtime: aws_lambda.Runtime.DOTNET_6,
      memorySize: 256,
      handler: "lambda-example::lambdaExample.Function::FunctionHandler",
      code: aws_lambda.Code.fromAsset("../assets/handlers-net.zip"),
      timeout: Duration.seconds(30),
      reservedConcurrentExecutions: 3
    });

    const lambdaHelloWorldWithExtension = new aws_lambda.Function(this, 'lambda-java-common-misconceptions-hello-world-extension', {
      description: new Date().toISOString(),
      runtime: aws_lambda.Runtime.JAVA_11,
      memorySize: 256,
      handler: "com.adebski.LambdaJavaCommonMisconceptionsHelloWorld",
      code: aws_lambda.Code.fromAsset("../assets/handlers-1.0.jar"),
      timeout: Duration.seconds(30),
      reservedConcurrentExecutions: 3,
      layers: [helloWorldLayer]
    });

    const lambdaMultiplePaths = new aws_lambda.Function(this, 'lambda-java-common-misconceptions-multiple-paths', {
      description: new Date().toISOString(),
      runtime: aws_lambda.Runtime.JAVA_11,
      memorySize: 256,
      handler: "com.adebski.LambdaJavaCommonMisconceptionsMultiplePaths",
      code: aws_lambda.Code.fromAsset("../assets/handlers-1.0.jar"),
      timeout: Duration.seconds(30),
      reservedConcurrentExecutions: 3
    });

    const lambdaNetMultiplePaths = new aws_lambda.Function(this, 'lambda-net-common-misconceptions-multiple-paths', {
      description: new Date().toISOString(),
      runtime: aws_lambda.Runtime.DOTNET_6,
      memorySize: 256,
      handler: "lambda-example::lambdaExample.FunctionMultiplePaths::FunctionHandler",
      code: aws_lambda.Code.fromAsset("../assets/handlers-net.zip"),
      timeout: Duration.seconds(30),
      reservedConcurrentExecutions: 3
    });

    const lambdaMultiplePathsProvisionedConcurrency =
      new aws_lambda.Function(this, 'lambda-java-common-misconceptions-multiple-paths-provisioned-concurrency', {
        description: new Date().toISOString(),
        runtime: aws_lambda.Runtime.JAVA_11,
        memorySize: 256,
        handler: "com.adebski.LambdaJavaCommonMisconceptionsMultiplePathsProvisionedConcurrency",
        code: aws_lambda.Code.fromAsset("../assets/handlers-1.0.jar"),
        timeout: Duration.seconds(30),
        reservedConcurrentExecutions: 3
      });
    const alias = new aws_lambda.Alias(this, 'lambda-java-common-misconceptions-multiple-paths-provisioned-concurrency-alias', {
      aliasName: 'prod',
      version: lambdaMultiplePathsProvisionedConcurrency.currentVersion,
      provisionedConcurrentExecutions: 1
    });

    const lambdaNetMultiplePathsProvsionedConcurrency = new aws_lambda.Function(this, 'lambda-net-common-misconceptions-multiple-paths-provisioned-concurrency', {
      description: new Date().toISOString(),
      runtime: aws_lambda.Runtime.DOTNET_6,
      memorySize: 256,
      handler: "lambda-example::lambdaExample.FunctionMultiplePathsProvisionedConcurrency::FunctionHandler",
      code: aws_lambda.Code.fromAsset("../assets/handlers-net.zip"),
      timeout: Duration.seconds(30),
      reservedConcurrentExecutions: 3
    });
    const aliasNet = new aws_lambda.Alias(this, 'lambda-net-common-misconceptions-multiple-paths-provisioned-concurrency-alias', {
      aliasName: 'prod-net',
      version: lambdaNetMultiplePathsProvsionedConcurrency.currentVersion,
      provisionedConcurrentExecutions: 1
    });
  }
}
