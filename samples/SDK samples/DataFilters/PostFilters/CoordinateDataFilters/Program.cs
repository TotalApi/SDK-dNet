using System;
using System.Linq;
using TotalApi.Core;
using TotalApi.Telematics;
using TotalApi.Telematics.DataFilters.Coordinates.SimplificateFilter;
using TotalApi.Telematics.DataFilters.Coordinates.StopsFilter;
using TotalApi.Telematics.DataFilters.FilterPipeline;

namespace CoordinateDataFilters
{
    public class Program
    {
        private static void Main(string[] args)
        {
            TotalApiBootstrapper.Create();

            const double tolerance = 0.00000001;
            const int minSpeed = 3;
            const int minStopDurationSeconds = 60;

            var stopfilterParams = new StopsPostFilterParameters { FilterAction = StopsPostFilterParameters.Action.RemoveStops,
                MinSpeed = minSpeed, MinStopDuration = TimeSpan.FromSeconds(minStopDurationSeconds) };
            var simplificateFilterParams = new SimplificatePostFilterParameters(tolerance);
            var pipelineSettings = PipelineSettings.CreateFromFilterParameters(simplificateFilterParams, stopfilterParams);
            TelematicsApi.Telematics.SetFiltersPipeline("Name", pipelineSettings, true, false);

            var device = TelematicsApi.Telematics.FindDevice(DeviceIdentifier.PhoneNumber("+555-0006"));
            Console.WriteLine($"Current device Id: {device.Id} with OwnId: {device.OwnId}");

            var trackDataWithoutFilters = TelematicsApi.Telematics.ReadCoordinates(new ReadCoordinatesParams(device.DeviceId));
            Console.WriteLine($"Count coordinates without pipeLine: {trackDataWithoutFilters.Count()}");

            //TelematicsApi.Telematics.AssignDefaultCoordinatesPostFiltersPipeline("Name");
            var trackDataWithFilters = TelematicsApi.Telematics.ReadCoordinates(device.DeviceId); ;
            Console.WriteLine($"Count coordinates with pipeLine: {trackDataWithFilters.Count()}");



            Console.ReadKey();
        }
    }
}
