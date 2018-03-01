namespace App.Code.Blogs
{
    using System.Collections.Generic;
    using System.Linq;
    using App.Code.Web;
    using BeachRankings.Models;
    using BeachRankings.Services.Crawlers;

    public class BlogQueryManager : IBlogQueryManager
    {
        private IArticleCrawler crawler;

        public BlogQueryManager(IArticleCrawler crawler)
        {
            this.crawler = crawler;
        }

        public ICollection<string> GetSplitArticleUrls(string articleLinks)
        {
            if (string.IsNullOrEmpty(articleLinks))
            {
                return new HashSet<string>();
            }

            return this.GetTrimmedArticleUrl(articleLinks)
                .Split('@')
                .Select(s => s.Trim())
                .Where(s => s != string.Empty)
                .ToList();
        }

        public string GetTrimmedArticleUrl(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                return null;
            }

            return url.Replace("@,", "@");
        }

        public ICollection<BlogArticle> GetUserBlogArticles(
            IWebNameParser parser,
            Blog blog, 
            string articleLinks, 
            int beachId, 
            int reviewId)
        {
            if (blog == null || string.IsNullOrEmpty(articleLinks))
            {
                return new HashSet<BlogArticle>();
            }

            var blogArticles = this.CreateBlogArticles(articleLinks);
            var blogUrl = parser.RemoveDomain(blog.Url);

            foreach (var blogArticle in blogArticles)
            {
                var articleHostName = parser.RemoveDomain(parser.GetUriHostName(blogArticle.Url));
                var foundBlog = blogUrl.Equals(articleHostName);

                if (foundBlog)
                {
                    blogArticle.BlogId = blog.Id;
                    blogArticle.ReviewId = reviewId;
                    blogArticle.BeachId = beachId;
                }
            }

            return blogArticles;
        }

        public ICollection<BlogArticle> GetAdminBlogArticles(string articleLinks, int beachId)
        {
            if (string.IsNullOrEmpty(articleLinks))
            {
                return new HashSet<BlogArticle>();
            }

            var blogArticles = this.CreateBlogArticles(articleLinks);

            foreach (var blogArticle in blogArticles)
            {
                blogArticle.BeachId = beachId;
            }

            return blogArticles;
        }

        private ICollection<BlogArticle> CreateBlogArticles(string articleLinks)
        {
            if (string.IsNullOrEmpty(articleLinks))
            {
                return new HashSet<BlogArticle>();
            }

            var urls = this.GetSplitArticleUrls(articleLinks);
            var blogArticles = new HashSet<BlogArticle>();
            var processedLinks = new HashSet<string>();

            foreach (var url in urls)
            {
                if (processedLinks.Contains(url))
                {
                    continue;
                }

                blogArticles.Add(new BlogArticle()
                {
                    Url = url,
                    Title = this.crawler.GetArticleTitle(url)
                });

                processedLinks.Add(url);
            }

            return blogArticles;
        }
    }
}