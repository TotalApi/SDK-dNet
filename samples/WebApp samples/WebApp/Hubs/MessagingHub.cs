using System;
using System.Diagnostics;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using TotalApi.Core.Events;
using TotalApi.Telematics;
using TotalApi.Telematics.Events;
using TotalApi.Utils;

namespace WebApp.Hubs
{
    [HubName("messagingHub")]
    public class MessagingHub : Hub
        , IEvent<OnDataChanged>
        , IEvent<OnDeviceStatusChanged>
    {
        private static readonly Lazy<MessagingHub> _instance = new Lazy<MessagingHub>(() => new MessagingHub());
        public static MessagingHub Instance { get { return _instance.Value; } }

        public IHubContext GlobalContext { get { return GlobalHost.ConnectionManager.GetHubContext<MessagingHub>(); } }

        public void HandleEvent(OnDataChanged e)
        {
            Trace.WriteLine($"ev -> OnDataChanged: {e.ActionType}: {e.TypeId}, {e.ObjectId}");
            GlobalContext.Clients.All.OnDataChanged(e.Sanity());
        }
        public void HandleEvent(OnDeviceStatusChanged e)
        {
            if (e.DeviceStatus == null) return;
            e.DeviceStatus.LastActivityTime = DateTime.SpecifyKind(e.DeviceStatus.LastActivityTime.ToUniversalTime(), DateTimeKind.Utc);
            if (e.DeviceStatus.LastCoordinate != null)
                e.DeviceStatus.LastCoordinate.UtcDate = DateTime.SpecifyKind(e.DeviceStatus.LastCoordinate.UtcDate.ToUniversalTime(), DateTimeKind.Utc);
            Trace.WriteLine($"ev -> OnDeviceStatusChanged: {e.DeviceStatus.Id}: {e.DeviceStatus.LastCoordinate}");
            var res = e.DeviceStatus.ToTransportFormat();
            GlobalContext.Clients.All.OnDeviceStatusChanged(res);
        }
    }
}
