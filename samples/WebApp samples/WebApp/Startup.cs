using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Owin;
using TotalApi.Core;
using TotalApi.Core.Api;
using TotalApi.Core.Authentication;
using TotalApi.Utils.ErrorManager;
using WebApp;
using WebApp.Hubs;

[assembly: OwinStartup(typeof(Startup))]

namespace WebApp
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ErrorLog.Init();

            // Инициализируем MEF.
            TotalApiBootstrapper.Create(".");

            // Configure SignalR
            app.MapSignalR(new HubConfiguration { EnableDetailedErrors = true });

            // подписка на события от сервера передаваемые через SignalR хаб.
            CoreApi.EventManager.Subscribe(MessagingHub.Instance);

        }
    }
}
