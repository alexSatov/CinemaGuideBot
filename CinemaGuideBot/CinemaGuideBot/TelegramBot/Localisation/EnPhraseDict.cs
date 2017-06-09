namespace CinemaGuideBot.TelegramBot.Localisation
{
    public class EnPhraseDict : IPhraseDict
    {
        public string Language => "EN";
        public string Greeting => "Hi! I'am your guide in the cinema world. Let's go.";

        public string StartCommandDescription => "greeting and help";
        public string HelpCommandDescription => "show this message";
        public string MovieSearchCommandDescription => "search movie info by title";
        public string WeekPremieresCommandDescription => "week premieres";
        public string WeekTopCommandDescription => "5 week top movies";
        public string LangCommandDescription => "language change";

        public string HelpText => "I'am supporting next commands:";
        public string EnterMovieTitle => "Enter movie mttle";
        public string MovieNotFound => "Movie not found";
        public string EnterLanguage => "Enter language";
        public string UnsupportedLanguage => "Unsupported language. Supported languages:";
        public string LanguageChanged => "Language changed";
        public string UnknownCommand => "Unknown command";
        public string UnexpectedError => "Unexpected error";

        public string Title => "Title";
        public string Year => "Year";
        public string Country => "Country";
        public string Director => "Director";
    }
}