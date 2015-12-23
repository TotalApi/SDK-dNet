using System;
using TotalApi.Core;

namespace Startup
{
    class Program
    {
        static void Main(string[] args)
        {
            TotalApiBootstrapper.Create();

            /*
                Further use the SDK features...

                Access to core apis, use correspondent properties to acces the module:
                    TotalApi.Core.Api.CoreApi.<further use module you want to explore>

                Access to telematics methods:
                    TotalApi.Telematics.TelematicsApi.Telematics.<method to execute>

                Access to billing api
                    TotalApi.Core.Billing.<further use module you want to explore, SubSystemManager for example>
            */
            
            Console.ReadKey();
        }

    }
}
