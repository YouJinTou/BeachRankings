namespace BeachRankings.Services.Crawlers
{
    using System;
    using System.Net;
    using System.Text;
    using System.Text.RegularExpressions;

    public class ArticleCrawler
    {
        public string GetArticleTitle(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                return string.Empty;
            }

            try
            {
                using (var client = new WebClient())
                {
                    client.Encoding = Encoding.UTF8;
                    var html = client.DownloadString(url);
                    var pattern = "<title>(.*?)</title>";
                    var match = Regex.Match(html, pattern);
                    var title = match.Success ? match.Groups[1].Value : string.Empty;

                    return title;
                }
            }
            catch (Exception)
            {
                return url;
            }            
        }
    }
}