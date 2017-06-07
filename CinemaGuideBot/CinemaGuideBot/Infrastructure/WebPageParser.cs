using System;
using System.Linq;
using System.Net.Http;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace CinemaGuideBot.Infrastructure
{
    public static class WebPageParser
    {
        private static readonly Regex valueExpr = new Regex(
            @">([\d\w\s]+?)<", RegexOptions.Compiled);

        public static bool TryParsePage(string page, Regex regex, out List<string> groups)
        {
            var match = regex.Match(page);

            groups = match.Success
                ? (from Group @group in match.Groups select @group.Value).Skip(1).ToList()
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

        public static string UniteParsedMultibleValues(string pageElement)
        {
            var groups = from Match match in valueExpr.Matches(pageElement) select match.Groups;
            var values = groups.Select(g => (from Group @group in g select @group.Value).Skip(1).First());
            return string.Join(", ", values);
        }
    }
}