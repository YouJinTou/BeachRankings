namespace BeachRankings.App.Utils.Extensions
{
    using System.Web;
    using System.Web.Mvc;

    public static class HtmlExtensions
    {
        public static IHtmlString ParseNullableValue(this HtmlHelper helper, double? value, string message)
        {
            var htmlString = value.HasValue ? value.Value.ToString() : message;

            return new HtmlString(htmlString);
        }

        public static IHtmlString ParseNullString(this HtmlHelper helper, string value, string message)
        {
            var htmlString = string.IsNullOrEmpty(value) ? message : value;

            return new HtmlString(htmlString);
        }

        public static IHtmlString PluralizeValue(this HtmlHelper helper, int value, string singularObject)
        {
            var pluralForm = singularObject.EndsWith("s") ? (singularObject + "es") :
                singularObject.EndsWith("f") ? (singularObject.TrimEnd('f') + "ves") :
                singularObject.EndsWith("h") ? (singularObject + "es") :
                singularObject.EndsWith("y") ? (singularObject.TrimEnd('y') + "ies") :
                (singularObject + "s");
            var htmlString = (value == 0) ? ("0 " + pluralForm) : 
                (value == 1) ? ("1 " + singularObject) : (value.ToString() + " " + pluralForm);

            return new HtmlString(htmlString);
        }

        public static IHtmlString GetBeachAnchorTagDivisionAddress(this HtmlHelper helper, string primaryDivision, string secondaryDivision, int primaryDivisionId, int? secondaryDivisionId)
        {
            var divisions = string.IsNullOrEmpty(secondaryDivision) ? "PrimaryDivisions" : "SecondaryDivisions";
            var divisionId = (divisions == "PrimaryDivisions") ? primaryDivisionId : secondaryDivisionId;
            var htmlString = "<a href=\"/" + divisions + "/Beaches/" + divisionId + "\">" + ParseNullString(helper, secondaryDivision, primaryDivision) + "</a>";

            return new HtmlString(htmlString);
        }
    }
}