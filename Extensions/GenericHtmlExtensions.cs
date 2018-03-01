namespace BeachRankings.Extensions
{
    using System.Collections.Generic;
    using System.Text;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Mvc.Html;

    public static class GenericHtmlExtensions
    {
        public static bool IsDebug(this HtmlHelper helper)
        {
#if DEBUG
                return true;
#else
            return false;
#endif
        }

        public static IHtmlString ParseNullableValue(this HtmlHelper helper, double? value, string message)
        {
            var htmlString = value.HasValue ? value.Value.ToString() : message;

            return new HtmlString(htmlString);
        }

        public static IHtmlString SwapIf<T>(this HtmlHelper helper, T actualValue, T equalsValue, string swapWith)
        {
            var htmlString = actualValue.Equals(equalsValue) ? swapWith : actualValue.ToString();

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

        public static IHtmlString AttachSchemeToDomain(this HtmlHelper helper, string domain)
        {
            var schemeExists = domain.StartsWith("http://") || domain.StartsWith("https://");

            if (schemeExists)
            {
                return new HtmlString(domain);
            }

            return new HtmlString(domain.Insert(0, "http://"));
        }

        public static IHtmlString GenerateAnchorTagIfNotNull<T>(
            this HtmlHelper helper,
            T actualValue,
            T equalsValue,
            string text,
            string otherwiseText,
            string action,
            string controller,
            object parameters)
        {
            if (actualValue.Equals(equalsValue))
            {
                return SwapIf(helper, actualValue, equalsValue, otherwiseText);
            }

            return helper.ActionLink(text, action, controller, parameters, null);
        }

        public static IHtmlString ToUnorderedList(this HtmlHelper helper, IEnumerable<string> strings, string ulCssClass = "")
        {
            var sb = new StringBuilder();

            sb.AppendLine($"<ul class=\"{ulCssClass}\">");

            foreach (var str in strings)
            {
                sb.AppendLine($"<li>{str}</li>");
            }

            sb.AppendLine("</ul>");

            return new HtmlString(sb.ToString());
        }
    }
}
