using System;
using CinemaGuideBot.Domain;
using CinemaGuideBot.BotCommands;
using Ninject;

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
            container.Bind<Bot>().To<Bot>().InSingletonScope().WithConstructorArgument("apiToken", apiToken);
            container.Bind<ICommandExecutor>().To<CommandExecutor>();
            container.Bind<ICommand>().To<HelpCommand>();
            container.Bind<ICommand>().To<StartCommand>();
            return container.Get<Bot>();
        }

        static void Test()
        {
            var mig = new KinopoiskWebPageMIG();
            var movieInfo = mig.GetMovieInfo("Чужой");
            Console.WriteLine(movieInfo);
        }
    }
}