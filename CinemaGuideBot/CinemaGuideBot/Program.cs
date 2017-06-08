using System;
using Ninject;
using CinemaGuideBot.BotCommands;
using CinemaGuideBot.Domain.MoviesInfoGetter;
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
            container.Bind<Bot>().To<Bot>().InSingletonScope().WithConstructorArgument("apiToken", apiToken);
            container.Bind(x => x.FromThisAssembly().SelectAllClasses().InheritedFrom<ICommand>().BindSingleInterface());
            container.Bind<ICommandExecutor>().To<CommandExecutor>();		
            container.Bind<IMoviesInfoGetter>().To<KinopoiskApi>();			
            return container.Get<Bot>();		
        }

        static void Test1()
        {
            var mig = new KinopoiskApi();
            var moviesInfo = mig.GetWeekTopMovies();
            foreach (var info in moviesInfo)
            {
                Console.WriteLine(info);
            }
        }

        static void Test()
        {
            var mig = new KinopoiskApi();
            var movieInfo = mig.GetMovieInfo("Wonder Woman");
            Console.WriteLine(movieInfo);
        }
    }
}