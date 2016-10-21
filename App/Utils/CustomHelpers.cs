namespace BeachRankings.App.Utils
{
    using System.Web;
    using System.Web.Mvc;

    public static class CustomHelpers
    {
        public static IHtmlString ParseNullableValue(this HtmlHelper helper, double? value, string message)
        {
            string htmlString = value.HasValue ? value.Value.ToString() : message;

            return new HtmlString(htmlString);
        }

        public static IHtmlString PluralizeValue(this HtmlHelper helper, int value, string singularObject)
        {   
            if (value == 0)
            {
                return new HtmlString(string.Empty);
            }         

            string pluralForm = singularObject.EndsWith("s") ? (singularObject + "es") :
                singularObject.EndsWith("f") ? (singularObject.TrimEnd('f') + "ves") :
                singularObject.EndsWith("h") ? (singularObject + "es") :
                singularObject.EndsWith("y") ? (singularObject.TrimEnd('y') + "ies") : 
                (singularObject + "s");  
            string htmlString = (value == 1) ? ("1 " + singularObject) : (value.ToString() + " " + pluralForm);
            htmlString = "(" + htmlString + ")";

            return new HtmlString(htmlString);
        }
    }
}