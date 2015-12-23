using System;
using System.ComponentModel.Composition;
using System.Threading;
using TotalApi.Core;
using TotalApi.Core.Api;
using TotalApi.Core.Events;
using TotalApi.Utils;
using TotalApi.Utils.Console;
using TotalApi.Utils.Tasks;


namespace AsyncTasks
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Running bootstraping. Please, wait a second...\r\n");
            TotalApiBootstrapper.Create();
            
            Console.WriteLine("Starting sample task...\r\n");
            
            CoreApi.AsyncTaskManager.Start(new SampleTask());
          
            CoreApi.EventManager.Subscribe(Subscriber.Instance);

            Console.ReadKey();
        }

        /// <summary>
        /// Sample task initialization
        /// </summary>
        internal class SampleTask: AsyncTaskObject
        {
            
        }

        /// <summary>
        /// Sample task handler. 
        /// You need to create and export this class in order to transfer the task through the system.
        /// </summary>
        [Export(typeof(IAsyncTaskHandler))]
        internal class SampleTaskHandler : AsyncTaskHandler<SampleTask, int>
        {
            protected override int Start(SampleTask p, CancellationToken cancellationToken, IProgressEx prg)
            {
                //You can specify some settings values here:
                //Settings.Timeout = new TimeSpan(10000);
                //....

                //Do some heavy processing here

                for (var i = 1; i <= 5; i++)
                {
                    if(i == 3)
                        CoreApi.EventManager.Publish(new CustomAsyncTaskEvent(p.TaskName, p, "Event from the task"));

                    Thread.Sleep(1000);
                    prg.Step(20);
                }

                Console.WriteLine("Task execution completed");
                return -1;
            }
        }

        internal class CustomAsyncTaskEvent : OnAsyncTaskEvent
        {
            public readonly string Message;

            public CustomAsyncTaskEvent(string taskId, AsyncTaskObject task, string message) : base(taskId, task)
            {
                Message = message;
            }
        }

        /// <summary>
        /// AsyncTask events handler.
        /// </summary>
        internal class Subscriber : IEvent<OnProgress>, IEvent<OnAsyncTaskStarted>, IEvent<OnAsyncTaskFinished>, IEvent<CustomAsyncTaskEvent>
        {
            private static readonly Lazy<Subscriber> _instance = new Lazy<Subscriber>(() => new Subscriber());
            public static Subscriber Instance { get { return _instance.Value; } }

            public void HandleEvent(OnProgress e)
            {
                ColorConsole.Do(ConsoleColor.Yellow, () =>
                {
                    Console.WriteLine("Task execution progress of {0}: {1}", e.InstanceId, e.Progress);
                });
            }

            public void HandleEvent(OnAsyncTaskStarted e)
            {
                ColorConsole.Do(ConsoleColor.Green, () =>
                {
                    Console.WriteLine("Task started with Id: {0}", e.TaskId);
                });
            }

            public void HandleEvent(OnAsyncTaskFinished e)
            {
                ColorConsole.Do(ConsoleColor.Red, () =>
                {
                    Console.WriteLine("Task with Id: {0} finished", e.TaskId);
                });
            }

            public void HandleEvent(CustomAsyncTaskEvent e)
            {
                ColorConsole.Do(ConsoleColor.Blue, () =>
                {
                    Console.WriteLine("Event from task with Id {0}: {1}", e.TaskId, e.Message);
                });
            }
        }
    }


}
