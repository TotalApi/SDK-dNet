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

            // Write emulatuin coordinates
            var coords = CoordinateSourceEmulation.LoadCoordsValues("track1");

            var deviceOwnId = $"TEST-DEVICE{Guid.NewGuid()}";

            #region create pipeline
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

            // Save pipeline to cloud
            TelematicsApi.Telematics.SetFiltersPipeline(pipeLineName, pipelineSettings, isCoordinatesPipeline: true, isPreFiltersPipeline: true);
            #endregion


            // Create device
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
            TelematicsApi.Telematics.WriteCoordinates(device.DeviceId, coords);

            // Before assign
            var trackDataWithoutAssign = TelematicsApi.Telematics.ReadCoordinates(new ReadCoordinatesParams(device.DeviceId));
            Console.WriteLine($"Count coordinates without pipeLine: {trackDataWithoutAssign.Count()}");

            // Set default pipeline for the all devices
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
            Console.WriteLine($"OnDeviceStateChanged: {e.DeviceStatus.LastStates}");
        }
    }
}