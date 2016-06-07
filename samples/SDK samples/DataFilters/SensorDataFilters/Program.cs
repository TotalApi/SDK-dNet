using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TotalApi.Billing;
using TotalApi.Core;
using TotalApi.Core.Api;
using TotalApi.Core.Events;
using TotalApi.Telematics;
using TotalApi.Telematics.DataFilters.FilterPipeline;
using TotalApi.Telematics.DataFilters.SensorsData.Median;
using TotalApi.Telematics.DataFilters.SensorsData.ThresholdFilter;
using TotalApi.Utils;

namespace DataFilters
{
    public class Program
    {
        private static void Main(string[] args)
        {
            TotalApiBootstrapper.Create();

            var device = CoreApi.Repository.ExecuteQuery<Device>(q => q.Where(x => x.PhoneNumber == "+555-0005")).FirstOrDefault();
            //var sensor = CoreApi.Repository.ExecuteQuery<Sensor>(q => q.Where(x => x.DeviceId == device.Id)).FirstOrDefault();

            //var medianFilter = new MedianPostFilterParameters {PointsNumber = 11};
            //var threashold = new SensorDataThresholdPostFilterParameters(280, EqualCondition.Greater, TimeSpan.FromSeconds(1));


            //var prms = PipelineSettings.CreateFromFilterParameters(new List<object> { medianFilter });
            //var pipeLine = TelematicsApi.DataFilterManager.GetSensorDataPostFiltersPipeline(prms);

            //var newCustomPipeline = PipelineSettings.CreateFromFilterParameters(medianFilter, threashold);
            //var pipelineName = $"Temperature_Sensor_{sensor?.Id}";

            // Save custom filter to cloud
            //TelematicsApi.Telematics.SetFiltersPipeline(pipelineName, newCustomPipeline, false, false);

            // Get sensor values without frilters
            var sensorValues = TelematicsApi.Telematics.ReadSensorValues(new ReadSensorValuesParams
            {
                SensorType = 0,
                SensorNumber = 1,
                DeviceId = device.DeviceId,
                From = DateTime.Now.AddHours(-5),
            });
            Console.WriteLine($"Count values: {sensorValues.Values.Count}");

            // Get sensor value with pipline of one filter
            var sensorValuesWithFilterPersonal = TelematicsApi.Telematics.ReadSensorValues(new ReadSensorValuesParams
            {
                SensorType = 0,
                SensorNumber = 1,
                DeviceId = device.DeviceId,
                From = DateTime.Now.AddHours(-5),
                Filters = new object[] { new MedianPostFilterParameters { PointsNumber = 11 } }
            });
            Console.WriteLine($"Count values after perosnal filter: {sensorValuesWithFilterPersonal.Values.Count}");

            /*var sensorValuesWithFilterCustom = TelematicsApi.Telematics.ReadSensorValues(new ReadSensorValuesParams
            {
                SensorType = 0,
                SensorNumber = 1,
                DeviceId = device.DeviceId,
                From = DateTime.Now.AddHours(-5),
                Filters = new object[] { pipelineName }
            });
            Console.WriteLine($"Count values after custom filter: {sensorValuesWithFilterCustom.Values.Count}");*/

            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
    }
}