namespace BeachRankings.App.Utils
{
    using System.IO;
    using System.Text.RegularExpressions;

    public static class Helper
    {
        public static string GetBeachImagesRelativeDir(string name)
        {
            var formattedBeachName = Regex.Replace(name, @"[^A-Za-z]", string.Empty);
            var relativeBeachDir = Path.Combine("Uploads", "Images", "Beaches", formattedBeachName);

            return relativeBeachDir;
        }
    }
}