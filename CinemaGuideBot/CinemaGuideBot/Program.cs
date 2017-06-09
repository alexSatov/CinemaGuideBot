using System;
using Ninject;
using CinemaGuideBot.BotCommands;
using CinemaGuideBot.Domain;
using Ninject.Extensions.Conventions;
using CinemaGuideBot.Domain.MoviesInfoGetter;

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
            container.Bind(x => x.FromThisAssembly().SelectAllClasses().InheritedFrom<ICommand>().BindSingleInterface());
            container.Bind<ICommandExecutor>().To<CommandExecutor>();
            return container.Get<Bot>();
        }
    }
}