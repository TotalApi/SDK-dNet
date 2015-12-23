using System;
using TotalApi.Core;
using TotalApi.Core.Api;
using TotalApi.Telematics;
using TotalApi.Utils.Extensions;

namespace Repository
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Running bootstraping. Please, wait a second...");
            TotalApiBootstrapper.Create();
            
            Console.WriteLine("Executing query with no parameters in order to get all devices");
            var devices = CoreApi.Repository.ExecuteQuery<Device>();

            Console.WriteLine("Devices list:");
            devices.ForEach(x => Console.WriteLine("Id: {0}, Name: {1}", x.Id, x.Name));

            Console.WriteLine("Trying to create new device. Just for example, identifying device by OwnId here.");
            var device = new Device()
            {
                OwnId = "TestDeviceId",
                Name = "TestDevice"
            };

            Console.WriteLine("Saving...");
            var savedDevice = (Device)CoreApi.Repository.Save(device, true);

            Console.WriteLine("Trying to get saved object");
            var foundDevice = CoreApi.Repository.Find<Device>(savedDevice.Id);
            
            Console.WriteLine("Device found: {0} with OwnId: {1}", foundDevice.Name, foundDevice.OwnId);

            Console.WriteLine("Trying to delete tested object");
            var ifDeleted = CoreApi.Repository.Delete<Device>(savedDevice.Id);
            Console.WriteLine("Delete status: {0}", ifDeleted);

            Console.ReadKey();
        }

    }
}
