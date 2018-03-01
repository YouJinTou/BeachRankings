namespace App.Code.Web
{
    using System;

    public class WebNameParser : IWebNameParser
    {
        public string GetUriHostName(string address)
        {
            if (string.IsNullOrEmpty(address))
            {
                return address;
            }

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

        public string RemoveDomain(string address)
        {
            if (string.IsNullOrEmpty(address))
            {
                return address;
            }

            var lastDotIndex = address.LastIndexOf('.');

            return ((lastDotIndex > -1) ? address.Substring(0, lastDotIndex) : address);
        }
    }
}