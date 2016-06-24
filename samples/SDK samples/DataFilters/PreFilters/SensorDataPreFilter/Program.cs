using System;
using System.Linq;
using System.Threading;
using Linq2Rest;
using TotalApi.Core;
using TotalApi.Core.Api;
using TotalApi.Telematics;
using TotalApi.Telematics.DataFilters.FilterPipeline;
using TotalApi.Telematics.DataFilters.SensorsData.Median;
using TotalApi.Telematics.DataFilters.SensorsData.ThresholdFilter;
using TotalApi.Telematics.Events;
using TotalApi.Utils;
using TotalApi.Utils.Console;
using TotalApi.Utils.Wcf.Events;

namespace SensorDataPreFilter
{
    public class Program
    {
        private static void Main(string[] args)
        {
            TotalApiBootstrapper.Create();

            const int sensorType = 9;
            const int sensorPort = 5;
            const int sensorNumber = 1;
            const int sensorAddress = 4;

            var devicePhone = $"+TEST-{Guid.NewGuid()}";

            // If device is not found, create it
            var device = TelematicsApi.Telematics.FindDevice(DeviceIdentifier.PhoneNumber(devicePhone));
            if (device == null)
            {
                device = new Device
                {
                    Name = "My Test Device",
                    ModelCode = 100,
                    PhoneNumber = devicePhone,
                };
                device = CoreApi.Repository.Save(device, true);
            }
            Console.WriteLine($"Current device Id: {device.Id} with Phone: {device.PhoneNumber}");

            // If sensor is not found, create it
            var sensor = TelematicsApi.Telematics.FindSensor(device.DeviceId, sensorType, sensorNumber);
            if (sensor == null)
            {
                sensor = new Sensor
                {
                    DeviceId = device.Id,
                    SensorType = sensorType,
                    Number = sensorNumber,
                    PortId = sensorPort,
                    Address = sensorAddress
                };
                sensor = CoreApi.Repository.Save(sensor, true);
            }
            Console.WriteLine($"Current sensor Id: {sensor.Id}");

            // Subscribe to velocityPipeline pre events
            var customEvent = new EventSensorDataChanged();
            CoreApi.EventManager.Subscribe(customEvent);

            const long overSpeedState = 1;
            const long overSpeedState1 = 8;
            const long shortStopState = 2;
            const long longStopState = 4;
            const double stopSpeedValue = 6;
            const double overSpeedValue = 40;

            // I want filtering my sensor speed data by parameters before saving to cloud
            var velocityPipeline = PipelineSettings.CreateFromFilterParameters(
                  new SensorDataThresholdPreFilterParameters(overSpeedState, overSpeedValue, EqualCondition.Greater)
                , new SensorDataThresholdPreFilterParameters(overSpeedState1, overSpeedValue, EqualCondition.Lower)
                , new SensorDataThresholdPreFilterParameters(shortStopState, stopSpeedValue, EqualCondition.Lower)
                , new SensorDataThresholdPreFilterParameters(longStopState, stopSpeedValue, EqualCondition.Lower)
            );
            
            // Save custom filter to cloud
            var pipelineName = $"Velocity_Sensor_{sensor.Id}";
            TelematicsApi.Telematics.SetFiltersPipeline(pipelineName, velocityPipeline, false, isPreFiltersPipeline: true);

            // Assign pipeline for the all sensors by sensor type and sensor number
            TelematicsApi.Telematics.AssignDefaultSensorDataPreFiltersPipeline(pipelineName, sensorType, sensorNumber);

            // Load emulation sensor data
            var source = DataSourceEmulation.LoadSensorValues("noisy", true, sensorType, sensorNumber);
            TelematicsApi.Telematics.WriteSensorValues(device.DeviceId, sensor.PortId, sensor.Address, source.Values.ToDictionary(svp => svp.UtcDate, svp => svp.Value));

            // Console sniff
            ConsoleListener.Start();
        }

        public class EventSensorDataChanged : IEvent<OnSensorStatusChanged>, IEvent<OnPing>
        {
            public void HandleEvent(OnSensorStatusChanged e)
            {
                Console.WriteLine($"SensorStatus is changed: {e.SensorStatus.Id}: {e.SensorStatus.LastStates}");
            }

            public void HandleEvent(OnPing e)
            {
                Console.WriteLine($"OnPing: {e.Content}");
            }
        }
    }
}
