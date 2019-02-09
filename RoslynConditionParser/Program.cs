using Newtonsoft.Json;
using RoslynConditionParser.Core;
using System;
using System.IO;
using System.Threading.Tasks;

namespace RoslynConditionParser
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            var config = LoadConfig();
            var sensors = LoadSensors();

            if (config.Conditions?.Length == 0)
            {
                Console.WriteLine("No conditions provided..");
                return;
            }

            var parser = new ConditionParser();
            while (true)
            {
                foreach (var condition in config.Conditions)
                {
                    var result = await parser.EvaluateConditionAsync(condition, sensors);
                    if (result.IsSuccess)
                    {
                        Console.WriteLine($"Processed condition '{condition}' successfully. Result was: {result.Value}");
                    }
                    else
                    {
                        Console.WriteLine($"Processed condition '{condition}' failed: {result.ErrorMessage}");
                    }
                }
                Console.WriteLine("Press enter to evaluate all again. Type exit to exit.");
                var content = Console.ReadLine();
                if ("exit".Equals(content, StringComparison.InvariantCultureIgnoreCase))
                    return;
            }
        }

        private static Sensor[] LoadSensors()
        {
            return new[]
            {
                // dummy sensors for now, real sensors would get their values from actual sources of course
                new Sensor("temperature", () => DateTime.UtcNow.Second),
                new Sensor("poolTemperature", () => DateTime.UtcNow.Second % 40)
            };
        }

        private static Config LoadConfig()
        {
            try
            {
                var configFile = "config.json";
                var content = File.ReadAllText(configFile);
                return JsonConvert.DeserializeObject<Config>(content);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }
    }
}
