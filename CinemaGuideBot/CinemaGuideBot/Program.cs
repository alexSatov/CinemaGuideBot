using System;
using System.Net.Http;
using CinemaGuideBot.BotCommands;
using Ninject;
using Ninject.Extensions.Conventions;

namespace CinemaGuideBot
{
    class Program
    {
        static void Main(string[] args)
        {
            var bot = CreateBotClient("355988386:AAFqvo7ldCDoFNJpOCZqpI864Cbsb1H7IOI");
            Console.Title = bot.UserName;
            bot.StartWorking();
            var lol = Console.ReadLine();
            bot.StopWorking();
        }

        private static Bot CreateBotClient(string apiToken)
        {
            var container = new StandardKernel();
            //container.Bind(x => x.FromThisAssembly().SelectAllClasses().BindAllInterfaces());
            container.Bind<Bot>().To<Bot>().InSingletonScope().WithConstructorArgument("apiToken", apiToken);
            container.Bind<ICommand>().To<HelpCommand>();
            container.Bind<ICommand>().To<StartCommand>();
            return container.Get<Bot>();
        }

        static void Test()
        {
            var baseAddress = new Uri("https://www.kinopoisk.ru/");
            var request = "index.php?first=no&what=&kp_query=12 стульев";

            using (var httpClient = new HttpClient { BaseAddress = baseAddress })
            {
                using (var response = httpClient.GetAsync(request).Result)
                {
                    string responseData = response.Content.ReadAsStringAsync().Result;
                    Console.WriteLine(responseData);
                }
            }
        }
    }
}