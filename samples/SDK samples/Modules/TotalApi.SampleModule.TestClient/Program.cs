using System;
using System.Linq;
using TotalApi.Core;
using TotalApi.Core.Api;
using TotalApi.SampleModule.Api;
using TotalApi.SampleModule.Api.Models;

namespace TotalApi.SampleModule.TestClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Running bootstraping. Please, wait a second...\r\n");
            TotalApiBootstrapper.Create();

            Console.WriteLine("Running bootstraping.\r\n");
            var result = SampleModuleApi.SampleModule.DoSomeStuff("this is test stuff");
            Console.WriteLine(result);
            
            var sampleObject = CoreApi.Repository.ExecuteQuery<SampleObjectModel>().First();

            Console.WriteLine("Sample object's test property value: {0}", sampleObject.TestProp);

            Console.ReadKey();
        }

    }
}
