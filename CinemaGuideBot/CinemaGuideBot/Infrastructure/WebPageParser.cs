using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;

namespace CinemaGuideBot.Infrastructure
{
    public static class WebPageParser
    {
        public static List<string> ParsePage(string page, Regex regex)
        {
            var match = regex.Match(page);
            return (from Group @group in match.Groups select @group.Value).ToList();
        }

        public static string GetPage(Uri address, string requestUri)
        {
            using (var httpClient = new HttpClient {BaseAddress = address})
            {
                using (var response = httpClient.GetAsync(requestUri).Result)
                {
                    return response.Content.ReadAsStringAsync().Result;
                }
            }
        }
    }
}