using System;
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
using TotalApi.Utils;

namespace DataFilters
{
    public class Program
    {
        private static void Main(string[] args)
        {
            TotalApiBootstrapper.Create();

            var apps = CoreApi.Repository.ExecuteQuery<Application>();

            var device = CoreApi.Repository.ExecuteQuery<Device>(q => q.Where(x => x.PhoneNumber == "+555-0005")).FirstOrDefault();
            var sensor = CoreApi.Repository.ExecuteQuery<Sensor>(q => q.Where(x => x.DeviceId == device.Id)).FirstOrDefault();

            //var customPipline = TelematicsApi.Telematics.GetFiltersPipeline($"Temperature_Sensor_{sensor?.Id}", false, false);

            var newCustomPipline = PipelineSettings.CreateFromFilterParameters(new MedianPostFilterParameters {PointsNumber = 11});

            // Save custom filter to cloud
            TelematicsApi.Telematics.SetFiltersPipeline($"Temperature_Sensor_{sensor?.Id}", newCustomPipline, false, false);

            // Get custom pipline from cloud
            var customPipline = TelematicsApi.Telematics.GetFiltersPipeline($"Temperature_Sensor_{sensor?.Id}", false, false);

            var task = Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    var sensorValues = TelematicsApi.Telematics.ReadSensorValues(new ReadSensorValuesParams
                    {
                        SensorType = 0,
                        SensorNumber = 1,
                        DeviceId = device.DeviceId,
                        From = DateTime.Now.AddHours(-5),
                    });
                    Console.WriteLine($"Count values: {sensorValues.Values.Count}");

                    var sensorValuesWithFilterCustom = TelematicsApi.Telematics.ReadSensorValues(new ReadSensorValuesParams
                    {
                        SensorType = 0,
                        SensorNumber = 1,
                        DeviceId = device.DeviceId,
                        From = DateTime.Now.AddHours(-5),
                        Filters = new object[] {customPipline.Name}
                    });
                    Console.WriteLine($"Count values after custom filter: {sensorValuesWithFilterCustom.Values.Count}");


                    // with personal pipline
                    var sensorValuesWithFilterPersonal = TelematicsApi.Telematics.ReadSensorValues(new ReadSensorValuesParams
                    {
                        SensorType = 0,
                        SensorNumber = 1,
                        DeviceId = device.DeviceId,
                        From = DateTime.Now.AddHours(-5),
                        Filters = new object[] {new MedianPostFilterParameters { PointsNumber = 11 }}
                    });
                    Console.WriteLine($"Count values after perosnal filter: {sensorValuesWithFilterPersonal.Values.Count}");

                    Thread.Sleep(TimeSpan.FromSeconds(1));
                }
            }, TaskCreationOptions.LongRunning);


            Console.WriteLine("Press any key to exit");
            task.Wait();
            Console.ReadKey();
        }
    }
}