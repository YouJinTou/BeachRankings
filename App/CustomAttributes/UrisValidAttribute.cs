namespace BeachRankings.App.CustomAttributes
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    public class UrisValidAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value == null)
            {
                return true;
            }

            var tokens = value.ToString().Split(',').Select(s => s.Trim()).Where(s => s != string.Empty).ToList();

            foreach (var token in tokens)
            {
                var startsCorrectly = token.StartsWith("http://") || token.StartsWith("https://");
                var isSingleWord = !(startsCorrectly && token.Contains("."));

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