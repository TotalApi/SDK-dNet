using System;
using System.Threading;
using System.Threading.Tasks;
using TotalApi.Core;
using TotalApi.Core.Api;
using TotalApi.Utils;
using TotalApi.Utils.Console;

namespace Events
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Running bootstraping. Please, wait a second...");
            TotalApiBootstrapper.Create();

            Console.WriteLine("Subscribing to event");

            //Pass IEvent or IEvent<> here. Object type used to support complex subscribers that implements lots of IEvent interfaces. 
            CoreApi.EventManager.Subscribe(Subscriber.Instance);
            
            Task.Factory.StartNew(() =>
            {
                for (int i = 0; i < 10; i++)
                {
                    var j = i;
                    ColorConsole.Do(ConsoleColor.DarkGreen, () => { Console.WriteLine("Posting event: {0}", j); });
                    Thread.Sleep(1000);
                    CoreApi.EventManager.Publish(new SampleEventClass(j));
                }
            }, TaskCreationOptions.PreferFairness);
            
            Console.ReadKey();
        }

        /// <summary>
        /// Sample event class. Just a simple class.
        /// </summary>
        internal class SampleEventClass
        {
            public readonly string Message;
            public SampleEventClass(int num)
            {
                Message = string.Format("Sample event message {0}", num);
            }
        }

        /// <summary>
        /// Subscriber for the events.
        /// </summary>
        internal class Subscriber : IEvent<SampleEventClass> //, you can implement several interfaces here, to support different events
        {
            private static readonly Lazy<Subscriber> _instance = new Lazy<Subscriber>(() => new Subscriber());
            public static Subscriber Instance { get { return _instance.Value; } }
            
            public void HandleEvent(SampleEventClass e)
            {
                ColorConsole.Do(ConsoleColor.Yellow, () =>
                {
                    Console.WriteLine("Event received: {0}", e.Message);
                });
            }
        }
    }
}
