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
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using MathNet.Numerics.Statistics;

// [assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace lambdaExample
{
    public class AccountProvisionedConcurrency
    {

        static AccountProvisionedConcurrency()
        {
            LambdaLogger.Log($"{DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.ff")}: AccountProvisionedConcurrency() staitc constructor invoked!");
        }
        public string Email { get; set; }
        public bool Active { get; set; }
        public DateTime CreatedDate { get; set; }
        public IList<string> Roles { get; set; }
    }

    public class FunctionMultiplePathsProvisionedConcurrency
    {

        private static readonly int SAMPLE_ITERATIONS = 2000;
        private readonly long constructTimeStart;
        private readonly long constructTimeEnd;
        private int invocations = 0;

        public FunctionMultiplePathsProvisionedConcurrency()
        {
            this.constructTimeStart = System.Environment.TickCount;
            LambdaLogger.Log($"{DateTime.Now.ToString($"yyyy/MM/dd HH:mm:ss.ff")}: pre-warming with {SAMPLE_ITERATIONS} iterations");
            for (int i = 0; i < SAMPLE_ITERATIONS; ++i)
            {
                var exampleInput = new Dictionary<string, string> { { "testNumber", i.ToString() } };
                var exampleOutput = FunctionMultiplePathsProvisionedConcurrency.CalculateResult(exampleInput);
                LambdaLogger.Log($"{DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.ff")}: {exampleOutput}");
            }

            this.constructTimeEnd = System.Environment.TickCount;
            LambdaLogger.Log($"{DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.ff")}: ENVIRONMENT VARIABLES: " + JsonSerializer.Serialize(System.Environment.GetEnvironmentVariables()));

            LambdaLogger.Log($"{DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.ff")}: Construction took {constructTimeEnd - constructTimeStart}");
        }

        public String FunctionHandler(Dictionary<String, String> input)
        {
            ++invocations;
            long start = System.Environment.TickCount;
            LambdaLogger.Log(
                $"{DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.ff")}: handleRequest {start - constructTimeEnd} ms after the constructor, {invocations} invocation");
            return CalculateResult(input);
        }

        public static String CalculateResult(Dictionary<String, String> input)
        {
            int numberReceived = Int32.Parse(input["testNumber"]);
            if (numberReceived % 2 == 0)
            {
                int numbersToGenerate = 1000;
                var random = new Random();
                var numbers = Enumerable.Range(1, numbersToGenerate).Select(x => random.NextDouble());
                var statistics = new DescriptiveStatistics(numbers);
                return $"std-dev {statistics.StandardDeviation} mean {statistics.Mean} skeweness {statistics.Skewness}";
            }
            else
            {
                AccountProvisionedConcurrency account = new AccountProvisionedConcurrency
                {
                    Email = "james@example.com",
                    Active = true,
                    CreatedDate = new DateTime(2013, 1, 20, 0, 0, 0, DateTimeKind.Utc),
                    Roles = new List<string>
                    {
                        "User",
                        "Admin"
                    }
                };
                string json = Newtonsoft.Json.JsonConvert.SerializeObject(account, Newtonsoft.Json.Formatting.Indented);
                AccountProvisionedConcurrency account2 = Newtonsoft.Json.JsonConvert.DeserializeObject<AccountProvisionedConcurrency>(json);
                return json + account2.ToString();
            }
        }
    }
}
