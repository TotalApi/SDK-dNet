using System;
using System.Diagnostics;
using TotalApi.Core;
using TotalApi.Utils.Console;

namespace TotalApi.SampleModule.TestSubSystem
{
    class Program
    {
        static void Main(string[] args)
        {
            //Tracing debugging info to console
            Trace.Listeners.Add(new ColorConsoleTraceListener());

            /*
                Copy SubSystemConfig.json from Config folder to bin folder.
                Copy TotalApi.SampleModule.dll and TotalApi.SampleModule.Api.dll to bin/Modules folder.
            */

            Console.WriteLine("\r\nRunning bootstraping. Please, wait a second...\r\n");
            TotalApiBootstrapper.Create();

            Console.WriteLine("Now you can start TestClient app to see if SampleModule avaliable and works.\r\n");

            Console.ReadKey();
        }

    }
}
