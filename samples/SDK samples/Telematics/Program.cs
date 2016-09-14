using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using TotalApi.Core;
using TotalApi.Core.Api;
using TotalApi.Telematics;
using TotalApi.Telematics.Events;
using TotalApi.Utils;
using TotalApi.Utils.Console;

namespace Telematics
{
    class Program
    {
        static void Main(string[] args)
        {
            // Выводим трассировочные сообщения в консоль
            Trace.Listeners.Add(new ColorConsoleTraceListener());

           
                Console.WriteLine("Running bootstraping. Please, wait a second...");
                TotalApiBootstrapper.Create();

            Console.WriteLine("Getting first found device");
            var device = CoreApi.Repository.ExecuteQuery<Device>().First();
            Console.WriteLine("Device found: {0} - {1}\r\n", device.Name, device.Id);

            Console.WriteLine("Subscribing to OnDeviceStatusChanged event, in order to receive statuses changes.\r\nMore info in \"Events\" sample sdk's project");
            CoreApi.EventManager.Subscribe(Subscriber.Instance);


            while (true)
            {
                //Console.WriteLine("Writting test coordinate");
                TelematicsApi.Telematics.WriteCoordinates(DeviceIdentifier.DbId(device.Id), new List<Coordinate>
                {
                    new Coordinate(DateTime.UtcNow, 20, 30, 40, 50, 60)
                });

                //Console.WriteLine("Getting last device status");
                var status = TelematicsApi.Telematics.GetDeviceLastStatus(device.Id);

                //if (status != null)
                    //Console.WriteLine("Last activity: {0}, Last coordinate: {1}\r\n", status.LastActivityTime, status.LastCoordinate);

                //Console.WriteLine("Trying to get track for device for all time period.\r\nBy default, all filters will be used during processing");
                var filteredTrack = TelematicsApi.Telematics.ReadCoordinates(DeviceIdentifier.DbId(device.Id));

                //Console.WriteLine("Coordinate points count: {0}\r\n", filteredTrack.Points.Count);

                //Console.WriteLine("Trying to get full track for device for all time period, no simplificate filter applied.\r\nPass DbNull.Value in order to exclude filtering at all.");
                var fullTrack = TelematicsApi.Telematics.ReadCoordinates(DeviceIdentifier.DbId(device.Id), null, null, DBNull.Value);

                //Console.WriteLine("Coordinate points count: {0}", fullTrack.Points.Count);
            }
            
            Console.ReadKey();
        }

        internal class Subscriber : IEvent<OnDeviceStatusChanged>
        {
            private static readonly Lazy<Subscriber> _instance = new Lazy<Subscriber>(() => new Subscriber());
            public static Subscriber Instance { get { return _instance.Value; } }
            
            public void HandleEvent(OnDeviceStatusChanged e)
            {
                ColorConsole.Do(ConsoleColor.DarkGreen, () =>
                {
                    Console.WriteLine("Device status changed: {0} - {1}", e.EventTime, e.DeviceStatus.Id);
                });
            }
        }
    }
}
