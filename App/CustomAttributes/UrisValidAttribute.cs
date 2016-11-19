namespace BeachRankings.App.CustomAttributes
{
    using BeachRankings.App.Utils;
    using System;
    using System.ComponentModel.DataAnnotations;

    public class UrisValidAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value == null)
            {
                return true;
            }

            var tokens = BlogHelper.SplitArticleUrls(value.ToString());

            foreach (var token in tokens)
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