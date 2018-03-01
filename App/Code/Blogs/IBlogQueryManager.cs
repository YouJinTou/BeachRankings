namespace App.Code.Blogs
{
    using App.Code.Web;
    using BeachRankings.Models;
    using System.Collections.Generic;

    public interface IBlogQueryManager
    {
        ICollection<string> GetSplitArticleUrls(string articleLinks);

        string GetTrimmedArticleUrl(string url);

        ICollection<BlogArticle> GetUserBlogArticles(
            IWebNameParser parser, 
            Blog blog, 
            string articleLinks, 
            int beachId, 
            int reviewId);

        ICollection<BlogArticle> GetAdminBlogArticles(string articleLinks, int beachId);
    }
}
