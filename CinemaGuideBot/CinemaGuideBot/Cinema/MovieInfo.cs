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

        public override bool Equals(object obj)
        {
            var otherMovieInfo = obj as MovieInfo;
            return ReferenceEquals(this, obj) 
                || otherMovieInfo != null && Equals(otherMovieInfo);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        protected bool Equals(MovieInfo other)
        {
            return year == other.year 
                && string.Equals(Title, other.Title) 
                && string.Equals(Country, other.Country) 
                && string.Equals(Director, other.Director) 
                && string.Equals(OriginalTitle, other.OriginalTitle) 
                && Equals(Rating, other.Rating);
        }
    }
}