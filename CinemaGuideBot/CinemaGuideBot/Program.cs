using System;
using Ninject;
using CinemaGuideBot.BotCommands;
using CinemaGuideBot.Domain.MovieInfoGetter;

namespace CinemaGuideBot
{
    class Program
    {
        static void Main(string[] args)
        {
            Test();
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
            container.Bind<ICommand>().To<MovieSearchCommand>();		
            container.Bind<IMovieInfoGetter>().To<KinopoiskApi>();		
            container.Bind<ICommand>().To<StartCommand>();		
            return container.Get<Bot>();		
        }

        static void Test()
        {
            var mig = new KinopoiskApi();
            var movieInfo = mig.GetMovieInfo("Викинг");
            Console.WriteLine(movieInfo);
        }
    }
}