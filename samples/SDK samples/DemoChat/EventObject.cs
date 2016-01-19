using TotalApi.Core.Authentication;
using TotalApi.Core.Events;
using TotalApi.Utils;

namespace DemoChat
{
    public class ChatEventObject : TotalApiEventObject
    //public class ChatEventObject : EventObject
    {
        // Subject
        public string Message { get; set; }

        public string UserName { get; set; }

        public ChatEventObject(string message)
        {
            Message = message;
            UserName = TotalApiAuth.UserLogin;
        }
    }
}
