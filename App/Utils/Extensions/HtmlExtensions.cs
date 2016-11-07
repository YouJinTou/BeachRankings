namespace BeachRankings.App.Utils.Extensions
{
    using System.Web;
    using System.Web.Mvc;

    public static class HtmlExtensions
    {
        public static IHtmlString ParseNullableValue(this HtmlHelper helper, double? value, string message)
        {
            string htmlString = value.HasValue ? value.Value.ToString() : message;

            return new HtmlString(htmlString);
        }

        public static IHtmlString PluralizeValue(this HtmlHelper helper, int value, string singularObject)
        {
            string pluralForm = singularObject.EndsWith("s") ? (singularObject + "es") :
                singularObject.EndsWith("f") ? (singularObject.TrimEnd('f') + "ves") :
                singularObject.EndsWith("h") ? (singularObject + "es") :
                singularObject.EndsWith("y") ? (singularObject.TrimEnd('y') + "ies") :
                (singularObject + "s");
            string htmlString = (value == 0) ? ("0 " + pluralForm) : 
                (value == 1) ? ("1 " + singularObject) : (value.ToString() + " " + pluralForm);

            return new HtmlString(htmlString);
        }
    }
}