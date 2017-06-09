using Ninject;
using Topshelf;
using CinemaGuideBot.TelegramBot;
using Ninject.Extensions.Conventions;
using CinemaGuideBot.TelegramBot.BotCommands;
using CinemaGuideBot.Cinema.MoviesInfoGetters;
using CinemaGuideBot.Cinema.MovieInfoFormatters;

namespace CinemaGuideBot
{
    class Program
    {
        static void Main(string[] args)
        {
            const string telegramApiToken = "355988386:AAFqvo7ldCDoFNJpOCZqpI864Cbsb1H7IOI";
            HostFactory.Run(x =>                                 
            {
                x.UseNLog();
                x.Service<Bot>(s =>                        
                {
                    s.ConstructUsing(name => CreateBotClient(telegramApiToken));
                    s.WhenStarted(bot => bot.StartReceiving());             
                    s.WhenStopped(bot => bot.StopReceiving());               
                });
                x.RunAsLocalSystem();                            
                x.SetDescription("Topshelf Host of CinemaGuideBot");        
                x.SetDisplayName("CinemaGuideBot");                       
                x.SetServiceName("CinemaGuideBot");                       
            });
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