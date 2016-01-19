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
                    // AppKey-authority is used in this call
                    isExists = CoreApi.ApiUsers.IsExists(userLogin);
                    if (!isExists)
                    {
                        // User is not registered - auto register it and sign in
                        // AppKey-authority is used in this call
                        CoreApi.ApiUsers.Save(new ApiUser { Login = userLogin, Password = userPassword }, true);
                    }
                    // Set AppUser auth information
                    TotalApiAuth.UserLogin = userLogin;
                    TotalApiAuth.UserPassword = userPassword;
                    // Check whether auth information is valid
                    // AppUser-authority is used in this call
                    isExists = CoreApi.ApiUsers.IsExists(userLogin);
                    // If auth is valid - initialize subscriber, otherwise exception will be thrown
                    CoreApi.EventManager.Subscribe(Subscriber.Instance);
                }
                catch (Exception e)
                {
                    // Clear AppUser auth information
                    TotalApiAuth.UserLogin = null;
                    isExists = false;
                    // Display error text
                    ColorConsole.Do(ConsoleColor.Red, () => Console.WriteLine(e.FullMessage()));
                }
            }

            Console.Clear();
            Console.WriteLine($"Hello: {TotalApiAuth.UserLogin}");
            Console.WriteLine("--------------------------------");

            Console.Write("> ");

            while (true)
            {
                var inputString = Console.ReadLine();
                CoreApi.EventManager.Publish(new ChatEventObject(inputString));
            }
            // ReSharper disable once FunctionNeverReturns
        }
    }
}
