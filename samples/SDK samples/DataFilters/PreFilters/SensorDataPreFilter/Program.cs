using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using TotalApi.Core;
using TotalApi.Core.Api;
using TotalApi.Telematics;
using TotalApi.Telematics.DataFilters.FilterPipeline;
using TotalApi.Telematics.DataFilters.SensorsData.ThresholdFilter;
using TotalApi.Telematics.Events;
using TotalApi.Utils;
using TotalApi.Utils.Console;
using TotalApi.Utils.ErrorManager;
using TotalApi.Utils.Wcf.Events;

namespace SensorDataPreFilter
{
    public class Program
    {
        private static void Main(string[] args)
        {
            TotalApiBootstrapper.Create();

            const int sensorType = 9;
            const int sensorNumber = 1;

            const int sensorPort = 5;
            const int sensorAddress = 4;

            // Find test device. If it is not found - create it.
            //var deviceOwnId = "TEST_DEVICE";
            var deviceOwnId = $"TEST-DEVICE{Guid.NewGuid()}";

            var device = TelematicsApi.Telematics.FindDevice(DeviceIdentifier.OwnId(deviceOwnId));
            if (device == null)
            {
                device = new Device
                {
                    Name = "My Test Device",
                    ModelCode = 100,
                    OwnId = deviceOwnId,
                };
                device = CoreApi.Repository.Save(device, true);
            }
            Console.WriteLine($"Current device Id: {device.Id} with OwnId: {device.OwnId}");


            // Find test sensor. If it is not found - create it.
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

            var customEvent = new EventSensorDataChanged();
            CoreApi.EventManager.Subscribe(customEvent);

            // |-----------------------+-----+-----------------------+----------------------
            // | [lowerSpeedState = 1] | [0] | [overSpeed1State = 2] | [overSpeed2State = 4]
            // 0                       3     4                       6

            const long lowerSpeedState = 1; const double lowerSpeedValue = 3;
            const long overSpeed1State = 2; const double overSpeed1Value = 4;
            const long overSpeed2State = 4; const double overSpeed2Value = 6;

            // I want filtering my sensor speed data by parameters before saving to cloud
            var testPipeline = PipelineSettings.CreateFromFilterParameters(
                 new SensorDataThresholdPreFilterParameters(overSpeed1State, overSpeed1Value, EqualCondition.Greater),
                 new SensorDataThresholdPreFilterParameters(overSpeed2State, overSpeed2Value, EqualCondition.Greater),
                 new SensorDataThresholdPreFilterParameters(lowerSpeedState, lowerSpeedValue, EqualCondition.Lower)
            );
            
            // Save custom filter to cloud
            var testPipelineName = $"Velocity_Sensor_{sensor.Id}";
            TelematicsApi.Telematics.SetFiltersPipeline(testPipelineName, testPipeline, isCoordinatesPipeline: false, isPreFiltersPipeline: true);

            // Assign pipeline for the all sensors by sensor type and sensor number
            TelematicsApi.Telematics.AssignDefaultSensorDataPreFiltersPipeline(testPipelineName, sensorType, sensorNumber);

            do
            {
//                Console.Clear();
                try
                {
                    var source = DataSourceEmulation.LoadSensorValues("noisy", true, sensorType, sensorNumber);
                    Console.WriteLine("Writting values");
//                    TelematicsApi.Telematics.WriteSensorValues(device.DeviceId, sensor.PortId, sensor.Address, source.Values.ToDictionary(svp => svp.UtcDate, svp => svp.Value));
                    var priorTime = DateTime.MinValue;
                    foreach (var sensorValuePoint in source.Values)
                    {
                        Console.Write(".");
                        if (priorTime != DateTime.MinValue)
                            Thread.Sleep(sensorValuePoint.UtcDate - priorTime);
                        priorTime = sensorValuePoint.UtcDate;
                        TelematicsApi.Telematics.WriteSensorValues(device.DeviceId, sensor.PortId, sensor.Address, new Dictionary<DateTime, object> { { sensorValuePoint.UtcDate, sensorValuePoint.Value } });
                    }
                    Console.WriteLine();

                    var sensorValuesParams = new ReadSensorValuesParams
                    {
                        SensorType = sensorType,
                        SensorNumber = sensorNumber,
                        DeviceId = device.DeviceId,
                        From = DateTime.Today
                    };

                    var sensorValues = TelematicsApi.Telematics.ReadSensorValues(sensorValuesParams);
                    Console.WriteLine($"Count sensor values after using pipeline: {sensorValues.Count()}");
                }
                catch (Exception e)
                {
                    e.Error();
                }
            } while (Console.ReadKey().Key != ConsoleKey.Escape);
        }

        public class EventSensorDataChanged :/* IEvent<OnSensorStatusChanged>, */IEvent<OnSensorStateChanged>
        {
            public void HandleEvent(OnSensorStatusChanged e)
            {
                ColorConsole.Do(ConsoleColor.Gray, () => Console.WriteLine($"SensorStatus [{e.SensorStatus.LastData.UtcDate.ToLocalTime()}]: {e.SensorStatus.LastData.Value} - {e.SensorStatus.LastStates}"));
            }

            #region Implementation of IEvent<in OnSensorStateChanged>

            public void HandleEvent(OnSensorStateChanged e)
            {
                ColorConsole.Do(ConsoleColor.White, () => Console.WriteLine($"\r\n----- SensorState [{e.SensorStatus.LastData.UtcDate.ToLocalTime()}]: {e.SensorStatus.LastData.Value} - {e.SensorStatus.LastStates}"));
            }

            #endregion
        }
    }
}
