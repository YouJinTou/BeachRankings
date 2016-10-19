namespace BeachRankings.App.Utils.CustomHelpers
{
    using System.Web;
    using System.Web.Mvc;

    public static class Extensions
    {
        public static IHtmlString ParseNullableScore(this HtmlHelper helper, double? score, string message)
        {
            string htmlString = score.HasValue ? score.Value.ToString() : message;

            return new HtmlString(htmlString);
        }
    }
}