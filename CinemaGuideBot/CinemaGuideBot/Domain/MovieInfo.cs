using System;

namespace CinemaGuideBot.Domain
{
    public struct MovieInfo
    {
        public string Title { get; set; }
        public string Country { get; set; }

        private int year;
        public int Year
        {
            get { return year; }
            set
            {
                if (value < 1 || value > DateTime.Now.Year)
                    throw new ArgumentException($"Incorrect year ({value})");
                year = value;
            }
        }

        private double rating;
        public double Rating
        {
            get { return rating; }
            set
            {
                if (value < 0 || value > 10)
                    throw new ArgumentException($"Incorrect rating ({value})");
                rating = value;
            }
        }
    }
}