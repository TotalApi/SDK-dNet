using System;
using System.Linq;
using TotalApi.Core;
using TotalApi.Core.Api;
using TotalApi.Telematics;
using TotalApi.Telematics.DataFilters.FilterPipeline;
using TotalApi.Telematics.DataFilters.SensorsData.Median;
using TotalApi.Telematics.DataFilters.SensorsData.ThresholdFilter;

namespace DataFilters
{
    public class Program
    {
        private static void Main(string[] args)
        {
            TotalApiBootstrapper.Create();

            var device = CoreApi.Repository.ExecuteQuery<Device>(q => q.Where(x => x.PhoneNumber == "+555-0006")).FirstOrDefault();
            var sensor = CoreApi.Repository.ExecuteQuery<Sensor>(q => q.Where(x => x.DeviceId == device.Id)).FirstOrDefault();

            // Create two filterls
            var medianFilter = new MedianPostFilterParameters {PointsNumber = 11};
            var threashold = new SensorDataThresholdPostFilterParameters(280, EqualCondition.Greater, TimeSpan.FromSeconds(1));

            // Create pipeline
            var newCustomPipeline = PipelineSettings.CreateFromFilterParameters(medianFilter, threashold);

            // Save custom filter to cloud
            var pipelineName = $"Temperature_Sensor_{sensor?.Id}";
            TelematicsApi.Telematics.SetFiltersPipeline(pipelineName, newCustomPipeline, false, false);

            if (device != null)
            {
                var sensorValuesParams = new ReadSensorValuesParams
                {
                    SensorType = 0,
                    SensorNumber = 1,
                    DeviceId = device.DeviceId,
                    From = DateTime.UtcNow.AddHours(-5)
                };

                // Get sensor values without frilters
                var sensorValues = TelematicsApi.Telematics.ReadSensorValues(sensorValuesParams);
            

                // Get sensor value with pipeline of two filters
                sensorValuesParams.Filters = new object[]
                {
                    // Marks the points by equal condition sensor data values (Greater 280)
                    new SensorDataThresholdPostFilterParameters(280, EqualCondition.Greater, TimeSpan.FromSeconds(1)),
                    // Smooths the data
                    new MedianPostFilterParameters { PointsNumber = 10 }
                };
                var sensorValuesWithPersonalPipeline = TelematicsApi.Telematics.ReadSensorValues(sensorValuesParams);
            

                // Use custom filter by name after saving to cloud
                sensorValuesParams.Filters = new object[] { medianFilter };
                var sensorValuesWithPipelineCustom = TelematicsApi.Telematics.ReadSensorValues(sensorValuesParams);


                // Use custom filter by name after saving to cloud
                sensorValuesParams.Filters = new object[] { pipelineName };
                var sensorValuesWithSavedCustimpipeline = TelematicsApi.Telematics.ReadSensorValues(sensorValuesParams);


                Console.WriteLine($"Count output values: {sensorValues.Values.Count}");
                Console.WriteLine($"Count output values after perosnal filter: {sensorValuesWithPersonalPipeline.Values.Count}");
                Console.WriteLine($"Count output values after custom pipeline: {sensorValuesWithPipelineCustom.Values.Count}");
                Console.WriteLine($"Count output values after load custom pipeline: {sensorValuesWithSavedCustimpipeline.Values.Count}");
            }

            var outputInfo = device == null ? "Error: Device not dound\nPress any key to exit" : "Press any key to exit";
            Console.WriteLine(outputInfo);
            Console.ReadKey();
        }
    }
}