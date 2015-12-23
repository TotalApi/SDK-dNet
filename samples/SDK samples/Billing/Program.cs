using System;
using TotalApi.Core;
using TotalApi.Core.Api;
using TotalApi.Utils.Extensions;

namespace Billing
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Running bootstraping. Please, wait a second...");
            TotalApiBootstrapper.Create();

            Console.Write("\r\nRetrieving subsystems connected...");
            var ssystems = CoreApi.SubSystems.All();
            Console.Write("Done.\r\n");
            ssystems.ForEach(x => Console.WriteLine("Subsystem Id: {0}, Description: {1}", x.Id, x.Description));

            Console.Write("\r\nRetrieving applications list...");
            var apps = CoreApi.Applications.ExecuteQuery();
            Console.Write("Done.\r\n");
            apps.ForEach(x => Console.WriteLine("Subsystem Id: {0}, Name: {1}", x.Id, x.Name));

            Console.Write("\r\nRetrieving users list...");
            var users = CoreApi.ApiUsers.ExecuteQuery();
            Console.Write("Done.\r\n");
            users.ForEach(x => Console.WriteLine("User Id: {0}, Name: {1}", x.UserId, x.Login));

            Console.ReadKey();
        }
        
    }
}
