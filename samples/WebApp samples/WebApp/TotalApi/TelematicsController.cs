using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Newtonsoft.Json.Linq;
using TotalApi.Telematics;
using TotalApi.Telematics.DataFilters.Coordinates;
using TotalApi.Telematics.DataFilters.FilterPipeline;
using TotalApi.Telematics.ServiceContracts;
using TotalApi.Utils.Extensions;
using WebApp.Api;

namespace WebApp.TotalApi
{
    [RoutePrefix(Consts.RoutePrefix + "Telematics")]
    public class TelematicsController : AppApiController, ITelematicsService
    {
        void ITelematicsService.WriteCoordinates(DeviceIdentifier deviceId, Coordinate[] coordinates)
        {
            TelematicsApi.Telematics.WriteCoordinates(deviceId, coordinates);
        }

        [HttpGet, Route("FindDevice/{type}/{devId}")]
        public Device FindDevice(string /*DeviceIdentifierTypes*/ type, string devId)
        {
            var res = TelematicsApi.Telematics.FindDevice(DeviceIdentifier.Create(devId, type.As<int>().As<DeviceIdentifierTypes>()));
            return res;
        }

        [HttpGet, Route("DeviceModels")]
        public DeviceModel[] GetDeviceModels()
        {
            return TelematicsApi.Telematics.GetDeviceModels().ToArray();
        }

        [HttpPost, Route("")]
        public void WriteCoordinates(JObject prms)
        {
            // Because of WebApi doesn't allow to create controllers with several parameters in query body have to do this nasty
            // More details: 
            //     http://stackoverflow.com/questions/14407458/webapi-multiple-put-post-parameters
            //     http://weblog.west-wind.com/posts/2012/May/08/Passing-multiple-POST-parameters-to-Web-API-Controller-Methods
            ((ITelematicsService)this).WriteCoordinates(prms["deviceId"].ToObject<DeviceIdentifier>(), prms["coordinates"].ToObject<Coordinate[]>());
        }

        [HttpPost, Route("ReadCoordinates")]
        public CoordinatePoints ReadCoordinates(ReadCoordinatesParams readParams)
        {
            readParams.Filters = DefaultFilterParameters.Create(readParams.Filters);
            //readParams.Filters = new[] {new StopsPostFilterParameters() {FilterAction = StopsPostFilterParameters.Action.Default, MinSpeed = 10, MinStopDuration = TimeSpan.FromSeconds(10)} };
            return TelematicsApi.Telematics.ReadCoordinates(readParams);
        }

        [HttpGet, Route("{id}")]
        public DeviceStatus GetDeviceLastStatus(string id)
        {
            return TelematicsApi.Telematics.GetDeviceLastStatus(id);
        }

        /// <summary>
        /// Finds device last status by its <see cref="Device.DeviceId"/>.
        /// If the device is not found returns <c>null</c>.
        /// </summary>
        /// <param name="type">Device identifier type. It is the text representation of the items of <see cref="DeviceIdentifierTypes"/>.</param>
        /// <param name="devId">Identifier value according its <paramref name="type"/>.</param>
        /// <returns>Found <see cref="Device"/> or <c>null</c> if device not found.</returns>
        [HttpGet, Route("{type}/{devId}")]
        public DeviceStatus GetDeviceIdentifierLastStatus(string type, string devId)
        {
            return TelematicsApi.Telematics.GetDeviceLastStatus(DeviceIdentifier.Create(devId, type.As<int>().As<DeviceIdentifierTypes>()));
        }

        public Sensor FindPhysicalSensor(string type, string devId, string portId, string address)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Finds sensor by its <see cref="P:TotalApi.Telematics.Device.DeviceId"/>, type and port identifier.
        /// If the sensor is not found returns <c>null</c>.
        /// </summary>
        /// <param name="type">Device identifier type. It is the text representation of the items of <see cref="T:TotalApi.Telematics.DeviceIdentifierTypes"/>.</param>
        /// <param name="devId">Identifier value according its <paramref name="type"/>.
        /// </param><param name="portId">Sensor port number.
        /// </param><param name="address">Sensor address.</param>
        /// <returns>
        /// Found <see cref="T:TotalApi.Telematics.Sensor"/> or <c>null</c> if device not found.
        /// </returns>
        [HttpGet, Route("FindSensor/{type}/{devId}/{portId}/{address}")]
        public Sensor FindSensor(string type, string devId, string portId, string address)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Finds sensor by its <see cref="P:TotalApi.Telematics.Device.DeviceId"/>, type and port identifier.
        /// If the sensor is not found returns <c>null</c>.
        /// </summary>
        /// <param name="type">Device identifier type. It is the text representation of the items of <see cref="T:TotalApi.Telematics.DeviceIdentifierTypes"/>.</param><param name="devId">Identifier value according its <paramref name="type"/>.</param><param name="sensorType">Sensor type. It is the text representation of the items of <see cref="T:TotalApi.Telematics.SensorType"/>.</param><param name="number">Sensor number.</param>
        /// <returns>
        /// Found <see cref="T:TotalApi.Telematics.Sensor"/> or <c>null</c> if device not found.
        /// </returns>
        [HttpGet, Route("FindSensorByType/{type}/{devId}/{sensorType}/{number}")]
        public Sensor FindSensorByType(string type, string devId, string sensorType, string number)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Writes data from the sensors of specified device.
        ///             The sensors are identified by <see cref="T:TotalApi.Telematics.SensorType"/> and number.
        /// </summary>
        /// <param name="deviceId">Device identifier.</param><param name="sensorData">Array of sensors data.</param>
        [HttpPost, Route("SensorsData")]
        public void WriteSensorData(DeviceIdentifier deviceId, IEnumerable<SensorData> sensorData)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Writes data from the sensors of specified device.
        ///             The sensors are identified by <paramref name="portId"/> and <paramref name="address"/>.
        ///             If there is no sensor registered with the <paramref name="portId"/> and <paramref name="address"/> do nothing.
        /// </summary>
        /// <param name="deviceId">Device identifier.</param><param name="portId">Number of sensor port.</param><param name="address">Address of sensors data.</param><param name="values">DateTime - Value pairs of the sensor data.</param>
        [HttpPost, Route("SensorValues")]
        public void WriteSensorValues(DeviceIdentifier deviceId, int portId, int address, IDictionary<DateTime, object> values)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets device sensor data last values by its <see cref="P:TotalApi.Telematics.Device.DeviceId"/>.
        ///             If the device is not found returns <c>null</c>.
        ///             Device type here is the text representation of the items of <see cref="T:TotalApi.Telematics.DeviceIdentifierTypes"/>.
        /// </summary>
        /// <param name="type">Device type.</param><param name="devId">Device identifier according to its type.</param>
        /// <returns>
        /// <see cref="T:TotalApi.Telematics.SensorData"/> array or empty array if no sensor data found.
        /// </returns>
        [HttpGet, Route("LastSensorsStatus/{type}/{devId}")]
        public SensorStatus[] GetLastSensorsStatus(string type, string devId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Reads the sensors data of the particular device
        /// </summary>
        /// <param name="readParams">Parameters for reading sensors values <see cref="T:TotalApi.Telematics.ReadSensorValuesParams"/>.</param>
        /// <returns>
        /// Collection of <see cref="T:TotalApi.Telematics.SensorValuePoint"/> or <c>null</c> if no sensor values found.
        /// </returns>
        [HttpPost, Route("ReadSensorValues")]
        public SensorValues ReadSensorValues(ReadSensorValuesParams readParams)
        {
            throw new NotImplementedException();
        }

        public PipelineSettings GetFiltersPipeline(string name, bool isCoordinatesPipeline, bool isPreFiltersPipeline)
        {
            throw new NotImplementedException();
        }

        public string[] GetFilterPipelineNames(bool isCoordinatesPipeline, bool isPreFiltersPipeline)
        {
            throw new NotImplementedException();
        }

        public void SetFiltersPipeline(string name, PipelineSettings pipeline, bool isCoordinatesPipeline, bool isPreFiltersPipeline)
        {
            throw new NotImplementedException();
        }

        public void AssignDefaultCoordinatesPostFiltersPipeline(string name)
        {
            throw new NotImplementedException();
        }

        public void AssignDefaultCoordinatesPreFiltersPipeline(string name)
        {
            throw new NotImplementedException();
        }

        public void AssignDefaultSensorDataPostFiltersPipeline(string name)
        {
            throw new NotImplementedException();
        }

        public void AssignDefaultSensorDataPreFiltersPipeline(string name, int? sensorType, int? sensorNumber)
        {
            throw new NotImplementedException();
        }
    }
}
