namespace BeachRankings.App.Utils
{
    using BeachRankings.Models;
    using BeachRankings.Models.Enums;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Web;

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

        public static string GetFilteredBeachesTitle(int criterionId, string country, string waterBody, bool isWaterBody)
        {            
            var criterionExist = (criterionId > 0 && criterionId <= 15);
            var place = isWaterBody ? ("the " + waterBody) : country;

            if (!criterionExist)
            {
                return "Top 25 Beaches in " + place;
            }

            var criterion = (Criterion)criterionId;

            switch (criterion)
            {
                case Criterion.Snorkeling:
                    return "Top 25 Beaches in " + place + " for Snorkeling";
                default:
                    return "Top 25 Beaches in " + place;
            }
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
            if (string.IsNullOrEmpty(articleLinks))
            {
                return new HashSet<string>();
            }

            return TrimArticleUrl(articleLinks).Split('@').Select(s => s.Trim()).Where(s => s != string.Empty).ToList();
        }

        public static string TrimArticleUrl(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                return null;
            }

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

    public static class ImageHelper
    {
        public static ICollection<BeachImage> PersistBeachImages(Beach beach, IEnumerable<HttpPostedFileBase> images, string authorId)
        {
            if (images == null || images.ElementAt(0) == null)
            {
                return new List<BeachImage>();
            }

            var relativeBeachDir = BeachHelper.GetBeachImagesRelativeDir(beach.Name);
            var beachDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, relativeBeachDir);
            var beachImages = new List<BeachImage>();

            Directory.CreateDirectory(beachDir);

            foreach (var image in images)
            {
                var uniqueName = Guid.NewGuid().ToString() + Path.GetFileName(image.FileName);
                var imagePath = Path.Combine(beachDir, uniqueName);
                var relativeImagePath = Path.Combine("\\", relativeBeachDir, uniqueName);
                var beachImage = new BeachImage()
                {
                    AuthorId = authorId,
                    BeachId = beach.Id,
                    Name = uniqueName,
                    Path = relativeImagePath
                };

                image.SaveAs(imagePath);
                beachImages.Add(beachImage);
            }

            return beachImages;
        }
    }
}