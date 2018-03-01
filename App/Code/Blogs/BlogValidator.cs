using App.Code.Web;
using BeachRankings.Data.UnitOfWork;
using BeachRankings.Models;
using System.Collections.Generic;
using System.Linq;

namespace App.Code.Blogs
{
    public class BlogValidator : IBlogValidator
    {
        private IBeachRankingsData data;
        private IBlogQueryManager blogQueryManager;
        private IWebNameParser webNameParser;

        public BlogValidator(
            IBeachRankingsData data, 
            IBlogQueryManager blogQueryManager, 
            IWebNameParser webNameParser)
        {
            this.data = data;
            this.blogQueryManager = blogQueryManager;
            this.webNameParser = webNameParser;
        }

        public bool AllArticleUrlsMatch(Blog blog, string articleLinks)
        {
            if (string.IsNullOrEmpty(articleLinks) || blog == null)
            {
                return true;
            }

            var articlesCount = this.blogQueryManager.GetSplitArticleUrls(articleLinks).Count;
            var matchedArticlesCount = this.blogQueryManager.GetUserBlogArticles(this.webNameParser, blog, articleLinks, 0, 0)
                .Count(a => !string.IsNullOrEmpty(a.BlogId));

            return (articlesCount == matchedArticlesCount);
        }

        public ICollection<string> ValidateBlogger(bool isBlogger, string blogUrl)
        {
            var errors = new List<string>();

            if (isBlogger && string.IsNullOrEmpty(blogUrl))
            {
                errors.Add("A blog URL is required.");
            }

            var url = this.webNameParser.GetUriHostName(blogUrl);

            if (this.data.Blogs.All().FirstOrDefault(b => b.Url == url) != null)
            {
                errors.Add("A blog with this URL already exists.");
            }

            return errors;
        }
    }
}