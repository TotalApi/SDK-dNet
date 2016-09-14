using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading.Tasks;
using TotalApi.Core.Events;
using TotalApi.Telematics;

namespace WebApp.Models
{
    public class Transport
    {
        public Transport() : this(true)
        {
        }

        public Transport(bool clear)
        {
            if (clear)
                Clear();
        }

        public static List<Transport> Transports = new List<Transport>
        {
            new Transport(false) {Id = "1", Name = "Tr1", DeviceIdentifier = DeviceIdentifier.OwnId("05b5607d-a0d1-4a09-8c27-5e908c64c32f")},
            new Transport(false) {Id = "2", Name = "Tr2", DeviceIdentifier = DeviceIdentifier.OwnId("7a80275e-a43e-4f84-b20c-572b16503e08") },
            new Transport(false) {Id = "3", Name = "Tr3", DeviceIdentifier = DeviceIdentifier.PhoneNumber("+822542119082") },
            new Transport(false) {Id = "4", Name = "Tr4", DeviceIdentifier = DeviceIdentifier.PhoneNumber("+704337715079") }
        };
        private void Clear()
        {
            Task.Delay(TimeSpan.FromMinutes(60)).ContinueWith(r =>
            {
                Transports.Remove(this);
                OnDataChanged.Publish(typeof(Device), TelematicsApi.Telematics.FindDevice(DeviceIdentifier).Id, OnDataChanged.Action.Delete);
            });
        }

        public string Id { get; set; }

        public string Name { get; set; }

        public DeviceIdentifier DeviceIdentifier { get; set; }

        [NotMapped]
        public Device Device { get; set; }

        [NotMapped]
        public DeviceStatus LastStatus { get; set; }
    }
}