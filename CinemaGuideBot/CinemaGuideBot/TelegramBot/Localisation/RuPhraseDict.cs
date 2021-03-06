﻿namespace CinemaGuideBot.TelegramBot.Localisation
{
    public class RuPhraseDict : IPhraseDict
    {
        public string Language => "RU";
        public string Greeting => "Приветствую! Я твой гид в мире кино. Давай же начнем.";

        public string StartCommandDescription => "приветствие и help";
        public string HelpCommandDescription => "показывает это сообщение";
        public string MovieSearchCommandDescription => "поиск информации о фильме по названию";
        public string WeekPremieresCommandDescription => "премьеры недели";
        public string WeekTopCommandDescription => "5 самых популярных фильмов недели";
        public string LangCommandDescription => "смена языка";

        public string HelpText => "Я поддерживаю следующие команды:";
        public string EnterMovieTitle => "Введите название фильма";
        public string MovieNotFound => "Фильм не найден";
        public string EnterLanguage => "Введите язык";
        public string UnsupportedLanguage => "Неподдерживаемый язык. Поддерживаемые языки:";
        public string LanguageChanged => "Язык изменен";
        public string UnknownCommand => "Неизвестная команда";
        public string UnexpectedError => "Непредвиденная ошибка";

        public string Title => "Название";
        public string Year => "Год";
        public string Country => "Страна";
        public string Director => "Режиссер";
    }
}
