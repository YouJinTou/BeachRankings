namespace App.Code.Web
{
    public interface IWebNameParser
    {
        string GetUriHostName(string address);

        string RemoveDomain(string address);
    }
}