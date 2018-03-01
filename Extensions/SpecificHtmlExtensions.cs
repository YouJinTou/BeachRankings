namespace BeachRankings.Extensions
{
    using System.Web;
    using System.Web.Mvc;

    public static class SpecificHtmlExtensions
    {
        public static IHtmlString GetBeachAnchorTagDivisionAddress(
            this HtmlHelper helper, 
            string primaryDivision, 
            string secondaryDivision, 
            int? primaryDivisionId, 
            int? secondaryDivisionId)
        {
            var countryIsDivisionless = (string.IsNullOrEmpty(primaryDivision) && string.IsNullOrEmpty(secondaryDivision));

            if (countryIsDivisionless)
            {
                return new HtmlString(string.Empty);
            }

            var divisions = string.IsNullOrEmpty(secondaryDivision) ? "PrimaryDivisions" : "SecondaryDivisions";
            var divisionId = (divisions == "PrimaryDivisions") ? primaryDivisionId : secondaryDivisionId;
            var htmlString = "<a href=\"/" + divisions + "/Statistics/" + divisionId + "\">" + 
                GenericHtmlExtensions.ParseNullString(helper, secondaryDivision, primaryDivision) + "</a>, ";

            return new HtmlString(htmlString);
        }

        public static IHtmlString GetReviewBeachName(
            this HtmlHelper helper, string beach, string primaryDivision, string secondaryDivision)
        {
            var name = $"{beach}";
            name += string.IsNullOrEmpty(primaryDivision) ? string.Empty : 
                string.IsNullOrEmpty(secondaryDivision) ? string.Empty : ", ";
            name += secondaryDivision;

            return new HtmlString(name);
        }
    }
}