using System;
using System.Linq;
using TotalApi.Core;
using TotalApi.Core.Api;
using TotalApi.Telematics;
using TotalApi.Telematics.DataFilters.Coordinates.SimplificateFilter;
using TotalApi.Telematics.DataFilters.Coordinates.StopsFilter;
using TotalApi.Telematics.DataFilters.FilterPipeline;
using TotalApi.Telematics.Events;
using TotalApi.Utils;
using TotalApi.Utils.Spatials;

namespace CoordinateDataPreFilter
{
    public class Program
    {
        private static void Main(string[] args)
        {
            TotalApiBootstrapper.Create();

            var series = CoordinateSourceEmulation.LoadCoordsValues("track1");

            const int minSpeed = 3;
            const double tolerance = 0.00000001;
            const int minStopDurationSeconds = 60;
            const string pipeLineName = "Default pre coordinate";

            var customEvent = new EventDeviceStateChanged();
            CoreApi.EventManager.Subscribe(customEvent);

            // Stop filter
            var stopfilterParams = new StopsPostFilterParameters
            {
                FilterAction = StopsPostFilterParameters.Action.RemoveStops,
                MinSpeed = minSpeed,
                MinStopDuration = TimeSpan.FromSeconds(minStopDurationSeconds)
            };

            // Simplificate Filter
            var simplificateFilterParams = new SimplificatePostFilterParameters(tolerance);

            var pipelineSettings = PipelineSettings.CreateFromFilterParameters(simplificateFilterParams, stopfilterParams);

            TelematicsApi.Telematics.SetFiltersPipeline(pipeLineName, pipelineSettings, isCoordinatesPipeline: true, isPreFiltersPipeline: true);

            var device = TelematicsApi.Telematics.FindDevice(DeviceIdentifier.PhoneNumber("+555-0006"));
            Console.WriteLine($"Current device Id: {device.Id} with OwnId: {device.OwnId}");

            // Before assign
            var trackDataWithoutAssign = TelematicsApi.Telematics.ReadCoordinates(new ReadCoordinatesParams(device.DeviceId));
            Console.WriteLine($"Count coordinates without pipeLine: {trackDataWithoutAssign.Count()}");

            TelematicsApi.Telematics.AssignDefaultCoordinatesPreFiltersPipeline(pipeLineName);

            // After assign
            var trackDataWithAssign = TelematicsApi.Telematics.ReadCoordinates(new ReadCoordinatesParams(device.DeviceId));
            Console.WriteLine($"Count coordinates without pipeLine: {trackDataWithAssign.Count()}");

            Console.WriteLine("Press any key to exit...");

            Console.ReadKey();
        }
    }

    public class EventDeviceStateChanged : IEvent<OnDeviceStateChanged>
    {
        public void HandleEvent(OnDeviceStateChanged e)
        {
            Console.WriteLine($"DeviceStatus: {e.DeviceStatus}");
        }
    }
}


/*
 const int minSpeed = 3;
            const double tolerance = 0.00000001;
            const int minStopDurationSeconds = 60;
            const string pipeLineName = "Default pre coordinate";

            var customEvent = new EventDeviceStateChanged();
            CoreApi.EventManager.Subscribe(customEvent);

            // Stop filter
            var stopfilterParams = new StopsPostFilterParameters
            {
                FilterAction = StopsPostFilterParameters.Action.RemoveStops,
                MinSpeed = minSpeed,
                MinStopDuration = TimeSpan.FromSeconds(minStopDurationSeconds)
            };

            // Simplificate Filter
            var simplificateFilterParams = new SimplificatePostFilterParameters(tolerance);

            var pipelineSettings = PipelineSettings.CreateFromFilterParameters(simplificateFilterParams, stopfilterParams);

            TelematicsApi.Telematics.SetFiltersPipeline(pipeLineName, pipelineSettings, isCoordinatesPipeline: true, isPreFiltersPipeline: true);

            var device = TelematicsApi.Telematics.FindDevice(DeviceIdentifier.PhoneNumber("+555-0006"));
            Console.WriteLine($"Current device Id: {device.Id} with OwnId: {device.OwnId}");

            // Before assign
            var trackDataWithoutAssign = TelematicsApi.Telematics.ReadCoordinates(new ReadCoordinatesParams(device.DeviceId));
            Console.WriteLine($"Count coordinates without pipeLine: {trackDataWithoutAssign.Count()}");

            TelematicsApi.Telematics.AssignDefaultCoordinatesPreFiltersPipeline(pipeLineName);

            // After assign
            var trackDataWithAssign = TelematicsApi.Telematics.ReadCoordinates(new ReadCoordinatesParams(device.DeviceId));
            Console.WriteLine($"Count coordinates without pipeLine: {trackDataWithAssign.Count()}");

            Console.WriteLine("Press any key to exit...");
*/
