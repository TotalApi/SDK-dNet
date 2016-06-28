﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TotalApi.Core;
using TotalApi.Core.Api;
using TotalApi.Telematics;
using TotalApi.Telematics.DataFilters.Coordinates.SimplificateFilter;
using TotalApi.Telematics.DataFilters.Coordinates.StopsFilter;
using TotalApi.Telematics.DataFilters.FilterPipeline;
using TotalApi.Telematics.Events;
using TotalApi.Utils;
using TotalApi.Utils.Console;

namespace CoordinateDataPreFilter
{
    public class Program
    {
        private static void Main(string[] args)
        {
            TotalApiBootstrapper.Create();

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

            // Use without pipeline
            var trackDataWithoutFilters = TelematicsApi.Telematics.ReadCoordinates(new ReadCoordinatesParams(device.DeviceId));
            Console.WriteLine($"Count coordinates without pipeLine: {trackDataWithoutFilters.Count()}");

            TelematicsApi.Telematics.AssignDefaultCoordinatesPreFiltersPipeline(pipeLineName);


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
