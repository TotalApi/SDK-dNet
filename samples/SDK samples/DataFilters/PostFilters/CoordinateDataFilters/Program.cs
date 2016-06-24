using System;
using System.Linq;
using TotalApi.Core;
using TotalApi.Telematics;
using TotalApi.Telematics.DataFilters.Coordinates.SimplificateFilter;
using TotalApi.Telematics.DataFilters.FilterPipeline;

namespace CoordinateDataFilters
{
    public class Program
    {
        private static void Main(string[] args)
        {
            TotalApiBootstrapper.Create();

            const double tolerance = 0.00000001;

            var simplificateFilterParams = new SimplificatePostFilterParameters(tolerance);
            var pipelineSettings = PipelineSettings.CreateFromFilterParameters(simplificateFilterParams);
            TelematicsApi.Telematics.SetFiltersPipeline("Name", pipelineSettings, true, false);

            var device = TelematicsApi.Telematics.FindDevice(DeviceIdentifier.PhoneNumber("+555-0006"));
            Console.WriteLine($"Current device Id: {device.Id} with OwnId: {device.OwnId}");

            var trackDataWithoutFilters = TelematicsApi.Telematics.ReadCoordinates(device.DeviceId, null, null, "");
            Console.WriteLine($"Count coordinates without pipeLine: {trackDataWithoutFilters.Count()}");

            TelematicsApi.Telematics.AssignDefaultCoordinatesPostFiltersPipeline("Name");
            var trackDataWithFilters = TelematicsApi.Telematics.ReadCoordinates(device.DeviceId, null, null, "");
            Console.WriteLine($"Count coordinates with pipeLine: {trackDataWithFilters.Count()}");

            Console.ReadKey();
        }
    }
}
