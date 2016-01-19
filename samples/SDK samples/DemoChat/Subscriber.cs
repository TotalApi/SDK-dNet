using System;
using TotalApi.Core.Authentication;
using TotalApi.Utils;
using TotalApi.Utils.Console;

namespace DemoChat
{
    // Customer
    public class Subscriber : IEvent<ChatEventObject>
    {
        public static Subscriber Instance { get; } = new Subscriber();

        public void HandleEvent(ChatEventObject e)
        {
            if (e.UserName == TotalApiAuth.UserLogin)
                ColorConsole.Do(ConsoleColor.Yellow, () =>
                {
                    Console.WriteLine($"\n                                               Me: {e.Message}");
                });
            else
                ColorConsole.Do(ConsoleColor.Green, () =>
                {
                    Console.WriteLine($"\n{e.UserName}: {e.Message}");
                });
            Console.Write("> ") ;
        }
    }
}
