hello world
2022-04-25 18:16:32 REPORT RequestId: e36e7af2-656b-4216-b056-2207818982d2	Duration: 85.24 ms	Billed Duration: 86 ms	Memory Size: 128 MB	Max Memory Used: 109 MB	Init Duration: 1821.08 ms
2022-04-25 18:18:08 REPORT RequestId: 5521fc91-a46e-4f18-a23d-ddc6550e8924	Duration: 1.95 ms	Billed Duration: 2 ms	Memory Size: 128 MB	Max Memory Used: 109 MB	
2022-04-25 18:18:50 REPORT RequestId: 9213b149-9edb-420f-a8fa-9bf062bcebbb	Duration: 1.74 ms	Billed Duration: 2 ms	Memory Size: 128 MB	Max Memory Used: 109 MB	
2022-04-25 18:24:05 REPORT RequestId: 724c1c5e-ad18-43e1-a3d6-67a4e644b1b3	Duration: 1.78 ms	Billed Duration: 2 ms	Memory Size: 128 MB	Max Memory Used: 109 MB	
2022-04-25 18:30:40 REPORT RequestId: a77e1c1d-77d1-4a5f-b895-099d18edb9d9	Duration: 72.91 ms	Billed Duration: 73 ms	Memory Size: 128 MB	Max Memory Used: 109 MB	Init Duration: 1870.40 ms

# multiple paths 1

## even
Duration: 776.76 ms 
Duration: 9.45 ms
Duration: 3.83 ms
Duration: 3.93 ms

## odd
Duration: 1202.92 ms
Duration: 2.48 ms
Duration: 2.06 ms

# multiple paths prov

## prov

### even
Duration: 2.03 ms
Duration: 2.40 ms
Duration: 1.95 ms

### odd 
Duration: 33.12 ms
Duration: 2.09 ms
Duration: 2.34 ms
Duration: 2.04 ms

# Layer vs no layer hello world with sleep
avg: 1.94/10.9
tm99.9: 1.95/17.5

# Layer vs no layer hello world without sleep
avg: 1.88/14.1
tm99.9: 1.88/14.2

# Relevant links
https://www.techempower.com/benchmarks/#section=data-r20&hw=ph&test=db 
single query: vertx-postgres	572,605	TPS

https://stackexchange.com/performance
SO: 300 REQ /S

https://calculator.aws/#/estimate?id=c1825f500176fcc8e47e20770a1d2437279f0d78

https://docs.aws.amazon.com/lambda/latest/dg/configuration-function-common.html#configuration-memory-console
1769 MB = 1vCPU

https://docs.aws.amazon.com/appconfig/latest/userguide/appconfig-integration-lambda-extensions.html

# Invoking lambdas

## Hello world layer
time aws lambda invoke --function-name arn:aws:lambda:us-east-1:690588305730:function:LambdaCommonMisconception-lambdajavacommonmisconce-Jcqfp3ofNXmR --payload '{"testNumber": 15 }' --cli-binary-format raw-in-base64-out  /dev/stdout

## Hello world
time aws lambda invoke --function-name arn:aws:lambda:us-east-1:690588305730:function:LambdaCommonMisconception-lambdajavacommonmisconce-U4viAfbnb69i --payload '{"testNumber": 15 }' --cli-binary-format raw-in-base64-out  /dev/stdout
