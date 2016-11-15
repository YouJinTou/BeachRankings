namespace BeachRankings.App.Utils
{
    using System.IO;
    using System.Text.RegularExpressions;

    public static class BeachHelper
    {
        public static string GetBeachImagesRelativeDir(string name)
        {
            var formattedBeachName = Regex.Replace(name, @"[^A-Za-z]", string.Empty);
            var relativeBeachDir = Path.Combine("Uploads", "Images", "Beaches", formattedBeachName);

            return relativeBeachDir;
        }
    }

    public static class UserHelper
    {
        public static string GetUserAvatarsRelativeDir()
        {
            var relativeAvatarsDir = Path.Combine("Uploads", "Images", "UserAvatars");

            return relativeAvatarsDir;
        }

        public static string GetUserDefaultAvatarPath()
        {
            var defaultAvatarPath = Path.Combine("\\", "Content", "Images", "unknown_profile.jpg");

            return defaultAvatarPath;
        }
    }
}