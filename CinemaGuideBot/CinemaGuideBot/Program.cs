using System;
using CinemaGuideBot.Domain;
using CinemaGuideBot.BotCommands;

namespace CinemaGuideBot
{
    class Program
    {
        static void Main(string[] args)
        {
            var bot = new Bot("355988386:AAFqvo7ldCDoFNJpOCZqpI864Cbsb1H7IOI", new HelpCommand(), new StartCommand());
            Console.Title = bot.UserName;
            bot.StartWorking();
            var lol = Console.ReadLine();
            bot.StopWorking();
        }

        static void Test()
        {
            var mig = new KinopoiskWebPageMIG();
            var movieInfo = mig.GetMovieInfo("Чужой");
            Console.WriteLine(movieInfo);
        }
    }
}