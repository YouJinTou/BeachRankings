namespace BeachRankings.App.CustomAttributes
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class UrisValidAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var tokens = value.ToString().Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var token in tokens)
            {
                var startsCorrectly = token.StartsWith("http://") || token.StartsWith("https://");
                var uri = startsCorrectly ? token : token.Insert(0, "http://");
                Uri uriResult;
                var isValidAddress = Uri.TryCreate(uri, UriKind.Absolute, out uriResult) && 
                    (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);

                if (!isValidAddress)
                {
                    return false;
                }
            }

            return true;
        }
    }
}