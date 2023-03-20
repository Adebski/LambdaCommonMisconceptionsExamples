using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon;
using Amazon.Util;
using Amazon.Lambda;
using Amazon.Lambda.Model;
using Amazon.Lambda.Core;
using Amazon.Lambda.SQSEvents;
using Amazon.XRay.Recorder.Core;
using Amazon.XRay.Recorder.Handlers.AwsSdk;
using System.IO;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace lambdaExample
{
    public class Function
    {
        private AmazonLambdaClient lambdaClient;

        public Function()
        {
            LambdaLogger.Log("Initialising handler");
        }

        public String FunctionHandler(Dictionary<String, String> input)
        {
            LambdaLogger.Log("Handling input " + input);
            return "Foo";
        }
    }
}
