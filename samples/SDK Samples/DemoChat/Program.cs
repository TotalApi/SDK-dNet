using System;
using TotalApi.Billing;
using TotalApi.Core;
using TotalApi.Core.Api;
using TotalApi.Core.Authentication;
using TotalApi.Utils.Console;
using TotalApi.Utils.ErrorManager;

namespace DemoChat
{
    class Program
    {
        static void Main(string[] args)
        {
            // Initialization client SDK
            TotalApiBootstrapper.Create();

            var isExists = false;
            while (!isExists)
            {
                Console.Write("Login: ");
                var userLogin = Console.ReadLine();
                Console.Write("Password: ");
                var userPassword = Console.ReadLine();
                try
                {
                    isExists = CoreApi.ApiUsers.IsExists(userLogin);
                    if (!isExists)
                    {
                        // User is not registered - auto register it and sign in
                        CoreApi.ApiUsers.Save(new ApiUser { Login = userLogin, Password = userPassword }, true);
                    }

                    TotalApiAuth.UserLogin = userLogin;
                    TotalApiAuth.UserPassword = userPassword;


                    isExists = CoreApi.ApiUsers.IsExists(userLogin);

                    CoreApi.EventManager.Subscribe(Subscriber.Instance);
                }
                catch (Exception e)
                {
                    TotalApiAuth.UserLogin = null;
                    TotalApiAuth.UserPassword = null;
                    isExists = false;
                    ColorConsole.Do(ConsoleColor.Red, () => Console.WriteLine(e.FullMessage()));
                    CoreApi.Logger.Error(e);
                }
            }

            Console.Clear();
            Console.WriteLine($"Hello: {TotalApiAuth.UserLogin}");
            Console.WriteLine("--------------------------------");

            Console.Write("> ");

            while (true)
            {
                var inputString = Convert.ToString(Console.ReadLine());
                CoreApi.EventManager.Publish(new ChatEventObject(inputString));
            }
            // ReSharper disable once FunctionNeverReturns
        }
    }
}
