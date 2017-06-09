namespace CinemaGuideBot.TelegramBot.Localisation
{
    public class EnPhraseDict : IPhraseDict
    {
        public string Greeting => "Hi! I'am your guide in the cinema world. Let's go.";

        public string StartCommandDescription => "greeting and help";
        public string HelpCommandDescription => "show this message";
        public string MovieSearchCommandDescription => "search movie info by title";
        public string WeekPremieresCommandDescription => "week premieres";
        public string WeekTopCommandDescription => "5 week top movies";

        public string HelpText => "I'am supporting next commands:";
        public string EnterMovieTitle => "Enter movie mttle";
        public string MovieNotFound => "Movie not found";
        public string UnknownCommand => "Unknown command";
        public string UnexpectedError => "Unexpected error";

        public string Title => "Title";
        public string Year => "Year";
        public string Country => "Country";
        public string Director => "Director";
    }
}