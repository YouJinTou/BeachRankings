namespace BeachRankings.App.Utils
{
    using BeachRankings.Models;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;

    public static class GenericHelpers
    {
        public static string GetUriHostName(string address)
        {
            var startsCorrectly = address.StartsWith("http://") || address.StartsWith("https://");
            var uri = startsCorrectly ? new Uri(address) : new Uri(address.Trim().Insert(0, "http://"));
            var host = uri.Host.Replace("www.", string.Empty);

            return host;
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

    public static class BlogsHelper
    {
        public static ICollection<Blog> GetUserBlogs(string blogsString, string userId)
        {
            if (string.IsNullOrEmpty(blogsString))
            {
                return new HashSet<Blog>();
            }

            var blogUrls = blogsString.Split(',').Select(s => s.Trim()).Where(s => s != string.Empty).ToList();
            var blogs = new HashSet<Blog>();

            foreach (var blogUrl in blogUrls)
            {
                blogs.Add(new Blog() { Url = GenericHelpers.GetUriHostName(blogUrl), UserId = userId });
            }

            return blogs;
        }

        public static ICollection<BlogArticle> GetBlogArticles(ICollection<Blog> blogs, string articleLinks, int beachId, int reviewId)
        {
            if (blogs == null || blogs.Count == 0)
            {
                return new HashSet<BlogArticle>();
            }

            var blogArticles = CreateBlogArticles(articleLinks);

            foreach (var blogArticle in blogArticles)
            {
                var articleHostName = GenericHelpers.GetUriHostName(blogArticle.Url);

                foreach (var blog in blogs)
                {
                    var foundBlog = blog.Url.Equals(articleHostName);

                    if (foundBlog)
                    {
                        blogArticle.BlogId = blog.Id;
                        blogArticle.ReviewId = reviewId;
                        blogArticle.BeachId = beachId;

                        break;
                    }
                }
            }

            return blogArticles;
        }

        public static bool AllArticleUrlsMatched(ICollection<Blog> blogs, string articleLinks)
        {
            if (string.IsNullOrEmpty(articleLinks))
            {
                return true;
            }

            var articlesCount = articleLinks.Split(',').Select(s => s.Trim()).Where(s => s != string.Empty).ToList().Count;
            var matchedArticlesCount = GetBlogArticles(blogs, articleLinks, 0, 0).Count(a => a.BlogId != 0);

            return (articlesCount == matchedArticlesCount);
        }

        private static ICollection<BlogArticle> CreateBlogArticles(string articleLinks)
        {
            if (string.IsNullOrEmpty(articleLinks))
            {
                return new HashSet<BlogArticle>();
            }

            var urls = articleLinks.Split(',').Select(s => s.Trim()).Where(s => s != string.Empty).ToList();
            var blogArticles = new HashSet<BlogArticle>();

            foreach (var url in urls)
            {
                blogArticles.Add(new BlogArticle() { Url = url });
            }

            return blogArticles;
        }
    }
}