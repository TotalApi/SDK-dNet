using System;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using TotalApi.Core.Events;
using TotalApi.Telematics;
using TotalApi.Telematics.Events;
using TotalApi.Utils;
using TotalApi.Utils.Extensions;
using TotalApi.Utils.Wcf.Events;

namespace WebApp.Hubs
{
    [HubName("messagingHub")]
    public class MessagingHub : Hub
//        , IEvent
        , IEvent<OnPing>
        , IEvent<OnSubSytemRegistered>
        , IEvent<OnBillingStarted>
        , IEvent<OnDataChanged>
        , IEvent<OnProgress>
        , IEvent<OnDeviceStatusChanged>
    {
        private static readonly ConnectionMapping<string> _connections = new ConnectionMapping<string>();
        private static readonly Lazy<MessagingHub> _instance = new Lazy<MessagingHub>(() => new MessagingHub());
        public static MessagingHub Instance { get { return _instance.Value; } }

        public string SessionId { get { return (ClaimsPrincipal.Current.Identity as ClaimsIdentity).With(_ => _.Name); } }

        public override Task OnConnected()
        {
            _connections.Add(SessionId, Context.ConnectionId);

            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            _connections.Remove(SessionId, Context.ConnectionId);

            return base.OnDisconnected(stopCalled);
        }

        public override Task OnReconnected()
        {
            if (!_connections.GetConnections(SessionId).Contains(Context.ConnectionId))
            {
                _connections.Add(SessionId, Context.ConnectionId);
            }
            return base.OnReconnected();
        }

        public IHubContext GlobalContext { get { return GlobalHost.ConnectionManager.GetHubContext<MessagingHub>(); } }

        public void HandleEvent(OnProgress e)
        {
            GlobalContext.Clients.All.OnProgress(e.Sanity());
        }

        public void HandleEvent(OnPing e)
        {
            Trace.WriteLine("ev -> OnPing: {0}".Fmt(e.Content));
            GlobalContext.Clients.All.OnPing(e.Content);
        }

        public void HandleEvent(OnDataChanged e)
        {
            Trace.WriteLine("ev -> OnDataChanged: {0}: {1}, {2}".Fmt(e.TypeId, e.ActionType, e.ObjectId));
            GlobalContext.Clients.All.OnDataChanged(e.Sanity());
        }

        public void HandleEvent(OnSubSytemRegistered e)
        {
            Trace.WriteLine("ev -> OnSubSytemRegistered: {0}".Fmt(e.SSID));
            GlobalContext.Clients.All.OnSubSytemRegistered(e.SSID);
        }

        public void HandleEvent(OnBillingStarted e)
        {
            Trace.WriteLine("ev -> OnBillingStarted...");
            GlobalContext.Clients.All.OnBillingStarted();
        }

        public void HandleEvent(OnDeviceStatusChanged e)
        {
            if (e.DeviceStatus == null) return;
            e.DeviceStatus.LastActivityTime = DateTime.SpecifyKind(e.DeviceStatus.LastActivityTime.ToUniversalTime(), DateTimeKind.Utc);
            if (e.DeviceStatus.LastCoordinate != null)
                e.DeviceStatus.LastCoordinate.UtcDate = DateTime.SpecifyKind(e.DeviceStatus.LastCoordinate.UtcDate.ToUniversalTime(), DateTimeKind.Utc);
            Trace.WriteLine("ev -> OnDeviceStatusChanged: {0}: {1}".Fmt(e.DeviceStatus.Id, e.DeviceStatus.LastCoordinate));
            var res = e.DeviceStatus.ToTransportFormat();
            GlobalContext.Clients.All.OnDeviceStatusChanged(res);
        }

        public void HandleEvent(object e)
        {
            Trace.WriteLine("ev -> {0}".Fmt(e.ToString()));
        }
    }
}
