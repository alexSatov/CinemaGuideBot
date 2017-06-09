using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace CinemaGuideBot.Web
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

        public static async Task<string> GetPageAsync(Uri address, string requestUri)
        {
            using (var httpClient = new HttpClient { BaseAddress = address })
            {
                using (var response =  await httpClient.GetAsync(requestUri))
                {
                    return await response.Content.ReadAsStringAsync();
                }
            }
        }

        public static IEnumerable<string> UniteParsedMultibleValues(string pageElement, Regex valueExpr)
        {
            var groups = from Match match in valueExpr.Matches(pageElement) select match.Groups;
            return groups.Select(g => (from Group @group in g select @group.Value).Skip(1).First());
        }
    }
}