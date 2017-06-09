using Ninject;
using Topshelf;
using CinemaGuideBot.TelegramBot;
using Ninject.Extensions.Conventions;
using CinemaGuideBot.TelegramBot.BotCommands;
using CinemaGuideBot.Cinema.MovieInfoParsers;
using CinemaGuideBot.Cinema.MoviesInfoGetters;
using CinemaGuideBot.TelegramBot.Localisation;

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

            container.Bind<IMoviesInfoGetter>().To<KinopoiskApi>().InSingletonScope();
            container.Bind<IMovieInfoParser>().To<MovieInfoJsonParser<ApiSearchResult>>().InSingletonScope();

            container.Bind(x => x.FromThisAssembly().SelectAllClasses().InheritedFrom<IPhraseDict>().BindSingleInterface());
            container.Bind<BotReply>().To<BotReply>().InSingletonScope();
            container.Bind<Bot>().To<Bot>().InSingletonScope().WithConstructorArgument("token", token);

            container.Bind(x => x.FromThisAssembly().SelectAllClasses().InheritedFrom<ICommand<string>>().BindSingleInterface());
            container.Bind<ICommandExecutor<string>>().To<CommandExecutor>().InSingletonScope();
            
            return container.Get<Bot>();
        }
    }
}