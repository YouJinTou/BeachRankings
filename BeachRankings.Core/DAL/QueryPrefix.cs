using System.Text.RegularExpressions;

namespace BeachRankings.Core.DAL
{
    public class QueryPrefix
    {
        public QueryPrefix(string prefix)
        {
            this.OriginalValue = prefix;
            this.CleanValue = this.GetCleanValue(prefix);
        }

        public string OriginalValue { get; }

        public string CleanValue { get; }

        public static implicit operator string(QueryPrefix prefix)
        {
            return prefix.CleanValue;
        }

        private string GetCleanValue(string prefix)
        {
            var pattern = "[^a-zA-Z0-9.-]";

            return Regex.Replace(prefix, pattern, string.Empty).ToLower();
        }
    }
}
