﻿using System;
using System.Linq;
using System.Collections.Generic;

namespace CinemaGuideBot.Domain
{
    public struct MovieInfo
    {
        public string Title { get; set; }
        public string Country { get; set; }
        public string OriginalTitle { get; set; }
        public Dictionary<string, int> Rating { get; set; }

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

        public override string ToString()
        {
            var title = Title == OriginalTitle ? $"Название: {Title}" : $"Название: {Title} ({OriginalTitle})";
            var rating = string.Join(", ", Rating.Select(r => $"{r.Key}: {r.Value}"));
            return $"{title}\r\n" +
                   $"Год: {Year}\r\n" +
                   $"{rating}\r\n";
        }
    }
}