using System;
using System.Threading.Tasks;
using Flurl;
using Flurl.Http;
using Newtonsoft.Json;

namespace KeepCorrect.Translator.WPF
{
    public class Search
    {
        public static async Task<SearchResult> GetSearchResult(string word)
        {
            var content = new
            {
                word = word,
                ajax_action = "ajax_balloon_Show",
                piece_index = 0,
                external = 1,
                partner_id = 2676311
            };

            try
            {
                var @string = await "https://puzzle-english.com" // shortcut for Request().AppendPathSegments(...)
                    .SetQueryParam("word", word)
                    .SetQueryParam("ajax_action", "ajax_balloon_Show")
                    .GetStringAsync();

                var result = JsonConvert.DeserializeObject<SearchResult>(@string,
                    new JsonSerializerSettings
                    {
                        Error = (se, ev) => { ev.ErrorContext.Handled = true; }
                    });
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }
    }
}