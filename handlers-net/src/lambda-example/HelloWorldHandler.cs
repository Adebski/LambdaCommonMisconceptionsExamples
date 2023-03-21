using System;
using System.Collections.Generic;
using System.Text.Json;
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

        private static readonly int DUMMY_INT_STATIC;
        private static readonly String DUMMY_STRING_STATIC;

        static Function()
        {
            LambdaLogger.Log($"{DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.ff")}: Static constructor for the class");
            DUMMY_INT_STATIC = 5;
            DUMMY_STRING_STATIC = getStringAndPrint("DUMMY_STRING_STATIC");
        }
        private static String getStringAndPrint(String variableName)
        {
            LambdaLogger.Log($"{DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.ff")}: Initialising {variableName} during the init phase");
            return variableName + "foo";
        }

        private readonly String dummyStringNonStatic = getStringAndPrint("dummyStringNonStatic");
        private readonly long constructTimeStart;
        private readonly long constructTimeEnd;
        private int invocations = 0;

        public Function()
        {
            this.constructTimeStart = System.Environment.TickCount;
            this.constructTimeEnd = System.Environment.TickCount;
            LambdaLogger.Log($"{DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.ff")}: ENVIRONMENT VARIABLES: " + JsonSerializer.Serialize(System.Environment.GetEnvironmentVariables()));

            LambdaLogger.Log($"{DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.ff")}: Construction took {constructTimeEnd - constructTimeStart}");
        }

        public String FunctionHandler(Dictionary<String, String> input)
        {
            ++invocations;
            long start = System.Environment.TickCount;
            int numberReceived = Int32.Parse(input["testNumber"]);
            String result =
                numberReceived % 2 == 0
                    ? $"{numberReceived} is even"
                    : $"{numberReceived} is odd";

            LambdaLogger.Log(
                $"{DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.ff")}: handleRequest {start - constructTimeEnd} ms after the constructor, {invocations} invocation");

            return result;
        }
    }
}
