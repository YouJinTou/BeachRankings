namespace BeachRankings.Services.Crawlers
{
    public interface IArticleCrawler
    {
        string GetArticleTitle(string url);
    }
}
