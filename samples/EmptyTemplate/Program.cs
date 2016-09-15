using System;
using System.Reflection;
using TotalApi.Core;
using TotalApi.Core.Api;
using TotalApi.Core.Api.ControlApi;
using TotalApi.Core.Authentication;
using TotalApi.Core.Events;
using TotalApi.Utils;
using TotalApi.Utils.Console;
using TotalApi.Utils.ErrorManager;

namespace EmptyTemplate
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                ColorConsole.Do(ConsoleColor.Yellow, () => Console.WriteLine("-- Loading modules --\r\n"));

                // Initialization client SDK
                TotalApiBootstrapper.Create();

                Console.Title = Params.Get<string>("appTitle") ?? Params.Get<string>("appId") ?? Assembly.GetExecutingAssembly().GetName().Name;

                ColorConsole.Do(ConsoleColor.Yellow, () => Console.WriteLine("-- Modules are loaded --\r\n"));

                CoreApi.Logger.InfoInstance(TotalApiEventManagerBase.LogInstance, "CLIENT_ID: {0}", TotalApiAuth.ClientId);

                //
                // Write your code here
                //

                ConsoleKeyHandlersManager.StartListeningConsole();
            }
            catch (Exception e)
            {
                ColorConsole.Do(ConsoleColor.Red, ()=> Console.WriteLine(e.FullMessage()));
                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
            }
        }
    }
}
