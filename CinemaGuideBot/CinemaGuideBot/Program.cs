using System;
using Ninject;
using CinemaGuideBot.BotCommands;
using Ninject.Extensions.Conventions;
using CinemaGuideBot.Domain.MoviesInfoGetters;
using CinemaGuideBot.Domain.MovieInfoFormatters;

namespace CinemaGuideBot
{
    class Program
    {
        static void Main(string[] args)
        {
            var bot = CreateBotClient("355988386:AAFqvo7ldCDoFNJpOCZqpI864Cbsb1H7IOI");
            Console.Title = bot.Name;
            bot.StartWorking();
            var str = Console.ReadLine();
            bot.StopWorking();
        }

        private static Bot CreateBotClient(string token)
        {
            var container = new StandardKernel();
            container.Bind<IMovieInfoFormatter>().To<SimpleMovieInfoFormatter>().InSingletonScope();
            container.Bind<Bot>().To<Bot>().InSingletonScope().WithConstructorArgument("token", token);
            container.Bind<IMoviesInfoGetter>().To<KinopoiskApi>();
            container.Bind(x => x.FromThisAssembly().SelectAllClasses().InheritedFrom<ICommand<string>>().BindSingleInterface());
            container.Bind<ICommandExecutor<string>>().To<CommandExecutor>().InSingletonScope();
            return container.Get<Bot>();
        }
    }
}