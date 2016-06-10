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

            const int sensorType = 0;
            const int sensorNumber = 1;

            var device = TelematicsApi.Telematics.FindDevice(DeviceIdentifier.PhoneNumber("+555-0006"));
            var sensor = TelematicsApi.Telematics.FindSensor(device.DeviceId, sensorType, sensorNumber);

            var source = DataSourceEmulation.LoadSensorValues("noisy", true, sensorType, sensorNumber);
            TelematicsApi.Telematics.WriteSensorValues(device.DeviceId, sensor.PortId, sensor.Address, source.Values.ToDictionary(svp => svp.UtcDate, svp => svp.Value));

            var sensorValuesParams = new ReadSensorValuesParams
            {
                SensorType = 0,
                SensorNumber = 1,
                DeviceId = device.DeviceId,
                From = DateTime.UtcNow.AddHours(-1)
            };

            var sensorValues = TelematicsApi.Telematics.ReadSensorValues(sensorValuesParams);


            Console.Read();
        }
    }
}
