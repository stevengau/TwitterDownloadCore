using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TwitterDownloadCore.Models;

namespace TwitterDownloadCore.Helper
{
    public class StaticHttp
    {
        public static async Task<List<Models.VideoResultViewModel>> GetVideo(string domain, FormUrlEncodedContent formContent)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri("https://cors-anywhere.herokuapp.com/");
                    var result = await client.PostAsync(domain, formContent);
                    string resultContent = await result.Content.ReadAsStringAsync();

                    return ParserFromTwdown(resultContent);
                }
            }
            catch (Exception e)
            {
                if (e.Message == "ParserFromTwdown Error")
                {

                }
            }
            return new List<VideoResultViewModel>();
        }

        public static List<Models.VideoResultViewModel> ParserFromTwdown(string source)
        {
            try
            {
                string pat = @"href=""https:\/\/video.twimg.com\S*";
                string pat2 = @"<td>[0-9][0-9][0-9]x[0-9][0-9][0-9]<\/td>";

                // Instantiate the regular expression object.
                Regex r = new Regex(pat, RegexOptions.IgnoreCase);
                Regex r2 = new Regex(pat2, RegexOptions.IgnoreCase);

                // Match the regular expression pattern against a text string.
                Match m = r.Match(source);
                Match m2 = r2.Match(source);

                List<VideoResultViewModel> viewModel = new List<VideoResultViewModel>();
                int matchCount = 0;
                while (m.Success)
                {

                    viewModel.Add(new VideoResultViewModel());
                    viewModel[matchCount].HrefAttribute = m.Groups[0].Value;
                    viewModel[matchCount].Resolution = m2.Groups[0].Value;
                    matchCount++;
                    m = m.NextMatch();
                    m2 = m2.NextMatch();
                }
                return viewModel;
            }
            catch (Exception)
            {
                throw new Exception("ParserFromTwdown Error");
            }
        }
    }
}
