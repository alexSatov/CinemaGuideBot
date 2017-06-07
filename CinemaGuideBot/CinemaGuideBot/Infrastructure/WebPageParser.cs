using System;
using System.Linq;
using System.Net.Http;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace CinemaGuideBot.Infrastructure
{
    public static class WebPageParser
    {
        public static bool TryParsePage(string page, Regex regex, out List<string> groups)
        {
            var match = regex.Match(page);

            groups = match.Success
                ? (from Group @group in match.Groups select @group.Value).ToList()
                : new List<string>();

            return match.Success;
        }

        public static string GetPage(Uri address, string requestUri)
        {
            using (var httpClient = new HttpClient { BaseAddress = address })
            {
                using (var response = httpClient.GetAsync(requestUri).Result)
                {
                    return response.Content.ReadAsStringAsync().Result;
                }
            }
        }
    }
}