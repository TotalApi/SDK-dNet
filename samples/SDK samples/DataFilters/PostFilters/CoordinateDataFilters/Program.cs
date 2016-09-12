using System;
using System.Diagnostics;
using System.Linq;
using TotalApi.Core;
using TotalApi.Telematics;
using TotalApi.Telematics.DataFilters.FilterPipeline;
using TotalApi.Telematics.DataFilters.Coordinates.StopsFilter;
using TotalApi.Telematics.DataFilters.Coordinates.SimplificateFilter;

namespace CoordinateDataFilters
{
    public class Program
    {
        private static void Main(string[] args)
        {
            TotalApiBootstrapper.Create();

            const int minSpeed = 3;
            const double tolerance = 0.00000001;
            const int minStopDurationSeconds = 60;
            const string pipeLineName = "Default coordinate";

            // Stop filter
            var stopfilterParams = new StopsPostFilterParameters { FilterAction = StopsPostFilterParameters.Action.RemoveStops,
                MinSpeed = minSpeed, MinStopDuration = TimeSpan.FromSeconds(minStopDurationSeconds) };

            // Simplificate Filter
            var simplificateFilterParams = new SimplificatePostFilterParameters(tolerance);

            // I want removed all vehicle stops and simplification track by tolerance
            var pipelineSettings = PipelineSettings.CreateFromFilterParameters(simplificateFilterParams, stopfilterParams);

            // Save pipeLine settings to cloud
            TelematicsApi.Telematics.SetFiltersPipeline(pipeLineName, pipelineSettings, true, false);

            var device = TelematicsApi.Telematics.FindDevice(DeviceIdentifier.PhoneNumber("+555-0006"));
            Console.WriteLine($"Current device Id: {device.Id} with OwnId: {device.OwnId}");

            // Use without pipeline
            var trackDataWithoutFilters = TelematicsApi.Telematics.ReadCoordinates(new ReadCoordinatesParams(device.DeviceId));
            Console.WriteLine($"Count coordinates without pipeLine: {trackDataWithoutFilters.Count()}");

            // Set default coordinate pipeline to all coordiante devices
            TelematicsApi.Telematics.AssignDefaultCoordinatesPostFiltersPipeline(pipeLineName);

            // Use with filters
            var trackDataWithFilters = TelematicsApi.Telematics.ReadCoordinates(device.DeviceId);
            Console.WriteLine($"Count coordinates with pipeLine: {trackDataWithFilters.Count()}");

            Console.ReadKey();
        }
    }
}
