using System;
using System.Collections.Generic;

namespace CinemaGuideBot.Cinema
{
    public class MovieInfo
    {
        public const int DefaultYear = 1800;

        public string Title { get; set; }
        public string Country { get; set; }
        public string Director { get; set; }
        public string OriginalTitle { get; set; }
        public Dictionary<string, string> Rating { get; set; }

        private int year;
        public int Year
        {
            get { return year; }
            set
            {
                if (value < 1 || value > DateTime.Now.Year)
                    throw new ArgumentOutOfRangeException($"Incorrect year ({value})");
                year = value;
            }
        }
    }
}