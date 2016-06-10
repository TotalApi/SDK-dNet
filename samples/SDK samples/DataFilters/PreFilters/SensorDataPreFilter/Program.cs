using System;
using System.Linq;
using TotalApi.Core;
using TotalApi.Core.Api;
using TotalApi.Telematics;

namespace SensorDataPreFilter
{
    public class Program
    {
        private static void Main(string[] args)
        {
            TotalApiBootstrapper.Create();

            var device = TelematicsApi.Telematics.FindDevice(DeviceIdentifier.PhoneNumber("+555-0006"));
            var sensor = TelematicsApi.Telematics.FindSensor(device.DeviceId, 0, 1);

            var source = DataSourceEmulation.LoadSensorValues("noisy");
            TelematicsApi.Telematics.WriteSensorValues(device.DeviceId, sensor.PortId, sensor.Address, source.Values.ToDictionary(svp => svp.UtcDate, svp => svp.Value));

            Console.Read();
        }
    }
}
