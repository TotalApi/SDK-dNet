using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web.Http;
using TotalApi.Core.Events;
using TotalApi.Telematics;
using WebApp.Models;

namespace WebApp.Api
{
    [RoutePrefix(Consts.RoutePrefix + "Transport")]
    //Transport CRUD actions
    public class TransportApiController: AppApiController
    {
        /// <summary>
        /// Request for all transports
        /// </summary>
        [HttpGet, Route("")]
        public IEnumerable<Transport> Get()
        {
            return Transport.Transports.Select(updateDevice);
        }

        /// <summary>
        /// Appeal to the TelematicsApi for not mapped data fields in data base
        /// </summary>
        private Transport updateDevice(Transport transport)
        {
            if (transport == null) return null;
            try
            {
                transport.Device = TelematicsApi.Telematics.FindDevice(transport.DeviceIdentifier);
                if (transport.Device != null)
                    transport.Device.ApiKey = null; // Clear ApiKey info. It's a secret information
                transport.LastStatus = TelematicsApi.Telematics.GetDeviceLastStatus(transport.DeviceIdentifier);
                if (transport.LastStatus != null)
                {
                    /*
                                        transport.LastStatus.LastActivityTime = DateTime.SpecifyKind(transport.LastStatus.LastActivityTime.ToUniversalTime(), DateTimeKind.Utc);
                                        if (transport.LastStatus.LastCoordinate != null)
                                            transport.LastStatus.LastCoordinate.UtcDate = DateTime.SpecifyKind(transport.LastStatus.LastCoordinate.UtcDate.ToUniversalTime(), DateTimeKind.Utc);
                    */
                }
            }
            catch
            {
            }
            return transport;
        }

        /// <summary>
        /// Request for transport by id
        /// </summary>
        /// <param name="id">Transport id</param>
        [HttpGet, Route("{id}")]
        public Transport Get(string id)
        {
            return updateDevice(Transport.Transports.FirstOrDefault(q => q.Id == id));
        }

        /// <summary>
        /// Create transport 
        /// </summary>
        /// <param name="transport">Transport object type</param>
        [HttpPost, Route("")]
        public Transport Create(Transport transport)
        {
            transport.Id = Guid.NewGuid().ToString();
            Transport.Transports.Add(transport);
            OnDataChanged.Publish(typeof(Transport), transport.Id, OnDataChanged.Action.Create);
            Debug.WriteLine(Transport.Transports);
            return transport;
        }

        /// <summary>
        /// Update transport
        /// </summary>
        /// <param name="transport">Transport object type</param>
        [HttpPut, Route("")]
        public Transport Update(Transport transport)
        {
            if(chackTransportId(transport.Id)) return null;
            Transport.Transports.Remove(Transport.Transports.FirstOrDefault(q => q.Id == transport.Id));
            Transport.Transports.Add(transport);
            OnDataChanged.Publish(typeof(Transport), transport.Id, OnDataChanged.Action.Update);
            Debug.WriteLine(Transport.Transports);
            return transport;
        }

        /// <summary>
        /// Delete transport by id
        /// </summary>
        /// <param name="id">Transport id</param>
        [HttpDelete, Route("{id}")]
        public bool Delete(string id)
        {
            if (chackTransportId(id)) return false;
            Transport.Transports.Remove(Transport.Transports.FirstOrDefault(q => q.Id == id));
            OnDataChanged.Publish(typeof(Transport), id, OnDataChanged.Action.Delete);
            Debug.WriteLine(Transport.Transports);
            return true;
        }

        private bool chackTransportId(string id)
        {
            return id.Length == 1;
        }
    }
}