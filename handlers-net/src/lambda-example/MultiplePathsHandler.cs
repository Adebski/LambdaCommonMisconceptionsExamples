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
    public class Account
    {

        static Account()
        {
            LambdaLogger.Log($"{DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.ff")}: Account() staitc constructor invoked!");
        }
        public string Email { get; set; }
        public bool Active { get; set; }
        public DateTime CreatedDate { get; set; }
        public IList<string> Roles { get; set; }
    }

    public class FunctionMultiplePaths
    {


        private readonly long constructTimeStart;
        private readonly long constructTimeEnd;
        private int invocations = 0;

        public FunctionMultiplePaths()
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
            LambdaLogger.Log(
                $"{DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.ff")}: handleRequest {start - constructTimeEnd} ms after the constructor, {invocations} invocation");
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
                Account account = new Account
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
                Account account2 = Newtonsoft.Json.JsonConvert.DeserializeObject<Account>(json);
                return json + account2.ToString();
            }
        }
    }
}
