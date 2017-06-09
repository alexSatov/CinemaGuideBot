namespace CinemaGuideBot.TelegramBot.Localisation
{
    public interface IPhraseDict
    {
        string Greeting { get; }

        string StartCommandDescription { get; }
        string HelpCommandDescription { get; }
        string MovieSearchCommandDescription { get; }
        string WeekPremieresCommandDescription { get; }
        string WeekTopCommandDescription { get; }
        string LangCommandDescription { get; }

        string HelpText { get; }
        string EnterMovieTitle { get; }
        string MovieNotFound { get; }
        string EnterLanguage { get; }
        string UnsupportedLanguage { get; }
        string LanguageChanged { get; }
        string UnknownCommand { get; }
        string UnexpectedError { get; }

        string Title { get; }
        string Year { get; }
        string Country { get; }
        string Director { get; }
    }
}