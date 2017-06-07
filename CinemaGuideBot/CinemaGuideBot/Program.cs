using System;
using System.Net.Http;
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
            var baseAddress = new Uri("https://www.kinopoisk.ru/");
            var request = "index.php?first=no&what=&kp_query=12 стульев";

            using (var httpClient = new HttpClient { BaseAddress = baseAddress })
            {
                using (var response = httpClient.GetAsync(request).Result)
                {
                    string responseData = response.Content.ReadAsStringAsync().Result;
                    Console.WriteLine(responseData);
                }
            }
        }
    }
}