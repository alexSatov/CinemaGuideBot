using System;
using Ninject;
using CinemaGuideBot.BotCommands;
using Ninject.Extensions.Conventions;
using CinemaGuideBot.Domain.MoviesInfoGetter;

namespace CinemaGuideBot
{
    class Program
    {
        static void Main(string[] args)
        {
            var bot = CreateBotClient("355988386:AAFqvo7ldCDoFNJpOCZqpI864Cbsb1H7IOI");
            Console.Title = bot.UserName;
            bot.StartWorking();
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
    }
}