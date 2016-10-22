namespace BeachRankings.App.Utils
{
    using System;
    using System.Drawing;
    using System.IO;

    public static class Helpers
    {
        public static Image ConvertBase64StringToImage(string s)
        {
            var base64 = s.Split(',');

            if ((base64[1].Length % 4) != 0)
            {
                throw new ArgumentException("Invalid base64 input.");
            }

            var bytes = Convert.FromBase64String(base64[1]);
            Image image;

            using (var ms = new MemoryStream(bytes))
            {
                image = Image.FromStream(ms, true);
            }

            return image;
        }
    }
}