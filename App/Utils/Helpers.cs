namespace BeachRankings.App.Utils
{
    using BeachRankings.Models;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;

    public static class GenericHelper
    {
        public static string GetUriHostName(string address)
        {
            var startsCorrectly = address.StartsWith("http://") || address.StartsWith("https://");
            Uri uri;

            try
            {
                uri = startsCorrectly ? new Uri(address) : new Uri(address.Trim().Insert(0, "http://"));
            }
            catch (Exception)
            {
                return string.Empty;
            }

            var host = uri.Host.Replace("www.", string.Empty);

            return host;
        }

        public static string RemoveDomain(string address)
        {
            var lastDotIndex = address.LastIndexOf('.');

            return ((lastDotIndex > -1) ? address.Substring(0, lastDotIndex) : address);
        }
    }

    public static class BeachHelper
    {
        public static string GetBeachImagesRelativeDir(string name)
        {
            var formattedBeachName = Regex.Replace(name, @"[^A-Za-z]", string.Empty);
            var relativeBeachDir = Path.Combine("Uploads", "Images", "Beaches", formattedBeachName);

            return relativeBeachDir;
        }
    }

    public static class UserHelper
    {
        public static string GetUserAvatarsRelativeDir()
        {
            var relativeAvatarsDir = Path.Combine("Uploads", "Images", "UserAvatars");

            return relativeAvatarsDir;
        }

        public static string GetUserDefaultAvatarPath()
        {
            var defaultAvatarPath = Path.Combine("\\", "Content", "Images", "unknown_profile.jpg");

            return defaultAvatarPath;
        }
    }

    public static class BlogHelper
    {
        public static ICollection<string> SplitArticleUrls(string articleLinks)
        {
            return TrimArticleUrl(articleLinks).Split('@').Select(s => s.Trim()).Where(s => s != string.Empty).ToList();
        }

        public static string TrimArticleUrl(string url)
        {
            return url.Replace("@,", "@");
        }

        public static ICollection<BlogArticle> GetBlogArticles(Blog blog, string articleLinks, int beachId, int reviewId)
        {
            if (blog == null || string.IsNullOrEmpty(articleLinks))
            {
                return new HashSet<BlogArticle>();
            }

            var blogArticles = CreateBlogArticles(articleLinks);
            var blogUrl = GenericHelper.RemoveDomain(blog.Url);

            foreach (var blogArticle in blogArticles)
            {
                var articleHostName = GenericHelper.RemoveDomain(GenericHelper.GetUriHostName(blogArticle.Url));
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

        public static bool AllArticleUrlsMatched(Blog blog, string articleLinks)
        {
            if (string.IsNullOrEmpty(articleLinks))
            {
                return true;
            }

            var articlesCount = SplitArticleUrls(articleLinks).Count;
            var matchedArticlesCount = GetBlogArticles(blog, articleLinks, 0, 0).Count(a => !string.IsNullOrEmpty(a.BlogId));

            return (articlesCount == matchedArticlesCount);
        }

        private static ICollection<BlogArticle> CreateBlogArticles(string articleLinks)
        {
            if (string.IsNullOrEmpty(articleLinks))
            {
                return new HashSet<BlogArticle>();
            }

            var urls = SplitArticleUrls(articleLinks);
            var blogArticles = new HashSet<BlogArticle>();
            var processedLinks = new HashSet<string>();

            foreach (var url in urls)
            {
                if (processedLinks.Contains(url))
                {
                    continue;
                }

                blogArticles.Add(new BlogArticle() { Url = url });

                processedLinks.Add(url);
            }

            return blogArticles;
        }
    }
}