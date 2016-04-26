using System.Threading;
using TotalApi.Core.Authentication;
using TotalApi.Core.Events;

namespace DemoChat
{
    public class ChatEventObject : TotalApiEventObject
    {
        public string Message { get; set; }

        public string UserName { get; set; }

        public ChatEventObject(string message)
        {
            Message = message;
            UserName = Thread.CurrentPrincipal.UserLogin();
        }
    }
}
