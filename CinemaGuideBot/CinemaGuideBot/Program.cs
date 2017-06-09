using Ninject;
using CinemaGuideBot.BotCommands;
using CinemaGuideBot.Domain;
using Ninject.Extensions.Conventions;
using CinemaGuideBot.Domain.MoviesInfoGetter;
using Topshelf;

namespace CinemaGuideBot
{
    class Program
    {
        static void Main(string[] args)
        {
            const string telegramApiToken = "355988386:AAFqvo7ldCDoFNJpOCZqpI864Cbsb1H7IOI";
            HostFactory.Run(x =>                                 
            {
                x.Service<Bot>(s =>                        
                {
                    s.ConstructUsing(name => CreateBotClient(telegramApiToken));   
                    s.WhenStarted(tc => tc.StartWorking());             
                    s.WhenStopped(tc => tc.StopWorking());               
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
            container.Bind<ICommandExecutor<string>>().To<CommandExecutor>();
            return container.Get<Bot>();
        }
    }
}