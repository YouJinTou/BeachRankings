namespace BeachRankings.App.CustomAttributes
{
    using global::App.Code.Blogs;
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;

    public class UrisValidAttribute : ValidationAttribute
    {
        private IBlogQueryManager blogQueryManager;

        public UrisValidAttribute()
        {
            this.blogQueryManager = DependencyResolver.Current.GetService<IBlogQueryManager>();
        }

        public override bool IsValid(object value)
        {
            if (value == null)
            {
                return true;
            }

            foreach (var token in this.blogQueryManager.GetSplitArticleUrls(value.ToString()))
            {
                var startsCorrectly = token.StartsWith("http://") || token.StartsWith("https://");
                var isSingleWord = !token.Contains(".");

                if (isSingleWord)
                {
                    return false;
                }

                var uri = startsCorrectly ? token : token.Insert(0, "http://");
                Uri uriResult;

                try
                {
                    var isValidAddress = Uri.TryCreate(uri, UriKind.Absolute, out uriResult) &&
                    (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);

                    if (!isValidAddress)
                    {
                        return false;
                    }
                }
                catch (Exception)
                {
                    return false;
                }                
            }

            return true;
        }
    }
}