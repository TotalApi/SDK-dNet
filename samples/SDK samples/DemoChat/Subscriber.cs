using System;
using System.Threading;
using TotalApi.Core.Authentication;
using TotalApi.Utils;
using TotalApi.Utils.Console;

namespace DemoChat
{
    public class Subscriber : IEvent<ChatEventObject>
    {
        public static Subscriber Instance { get; } = new Subscriber();

        public void HandleEvent(ChatEventObject e)
        {
            if (e.UserName == Thread.CurrentPrincipal.UserLogin())
                ColorConsole.Do(ConsoleColor.Yellow, () =>
                {
                    Console.WriteLine($"\n                                               {e.EventTime} - Me > {e.Message}");
                });
            else
                ColorConsole.Do(ConsoleColor.Green, () =>
                {
                    Console.WriteLine($"\n{e.EventTime} - {e.UserName} > {e.Message}");
                });
            Console.Write("> ") ;
        }
    }
}
