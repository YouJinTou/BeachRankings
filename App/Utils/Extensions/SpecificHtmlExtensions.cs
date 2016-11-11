namespace BeachRankings.App.Utils.Extensions
{
    using System.Web;
    using System.Web.Mvc;

    public static class SpecificHtmlExtensions
    {
        public static IHtmlString GetBeachAnchorTagDivisionAddress(this HtmlHelper helper, string primaryDivision, string secondaryDivision, int? primaryDivisionId, int? secondaryDivisionId)
        {
            var countryIsDivisionless = (string.IsNullOrEmpty(primaryDivision) && string.IsNullOrEmpty(secondaryDivision));

            if (countryIsDivisionless)
            {
                return new HtmlString(string.Empty);
            }

            var divisions = string.IsNullOrEmpty(secondaryDivision) ? "PrimaryDivisions" : "SecondaryDivisions";
            var divisionId = (divisions == "PrimaryDivisions") ? primaryDivisionId : secondaryDivisionId;
            var htmlString = "<a href=\"/" + divisions + "/Beaches/" + divisionId + "\">" + GenericHtmlExtensions.ParseNullString(helper, secondaryDivision, primaryDivision) + "</a>";

            return new HtmlString(htmlString);
        }
    }
}