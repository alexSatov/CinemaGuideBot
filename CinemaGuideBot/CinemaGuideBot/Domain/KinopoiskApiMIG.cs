﻿using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using CinemaGuideBot.Infrastructure;
using System.Text.RegularExpressions;

namespace CinemaGuideBot.Domain
{
    public class KinopoiskApiMIG : IMovieInfoGetter
    {
        public static readonly Uri KinopoiskApiUri = new Uri("https://getmovie.cc");
        private const string token = "037313259a17be837be3bd04a51bf678";

        public MovieInfo GetMovieInfo(string searchTitle)
        {
            var movieId = KinopoiskWebPageMIG.GetMovieId(searchTitle);
            var page = WebPageParser.GetPage(KinopoiskApiUri, $"/api/kinopoisk.json?id={movieId}&token={token}");
            page = Regex.Unescape(page);
            var fullMovieInfo = JsonConvert.DeserializeObject<Dictionary<string, object>>(page);

            var director = ((fullMovieInfo["creators"] as JObject)
                    .GetValue("director")
                    .First as JObject)
                .GetValue("name_person_ru");

            var jRating = (JObject) fullMovieInfo["rating"];
            var rating = new Dictionary<string, double>
            {
                ["Кинопоиск"] = double.Parse(jRating.GetValue("imdb").ToString().Replace('.', ',')),
                ["IMDb"] = double.Parse(jRating.GetValue("kp_rating").ToString().Replace('.', ','))
            };

            return new MovieInfo
            {
                Title = fullMovieInfo["name_ru"].ToString(),
                OriginalTitle = fullMovieInfo["name_en"].ToString(),
                Year = int.Parse(fullMovieInfo["year"].ToString()),
                Country = fullMovieInfo["country"].ToString(),
                Director = director.ToString(),
                Rating = rating
            };
        }
    }
}