namespace BeachRankings.Services.Initializers
{
    using System;
    using System.Collections.Generic;

    public static class LetterInitializer
    {
        private static Lazy<Dictionary<char, char>> letterPairs
             = new Lazy<Dictionary<char, char>>(() => LoadLetterPairs());

        public static Dictionary<char, char> LetterPairs
        {
            get
            {
                return letterPairs.Value;
            }
        }

        private static Dictionary<char, char> LoadLetterPairs()
        {
            var letterPairs = new Dictionary<char, char>
            {
                { 'ă', 'a' },
                { 'á', 'a' },
                { 'ã', 'a' },
                { 'å', 'a' },
                { 'ä', 'a' },
                { 'ā', 'a' },
                { 'â', 'a' },
                { 'à', 'a' },
                { 'ả', 'a' },
                { 'ạ', 'a' },
                { 'ẵ', 'a' },
                { 'ậ', 'a' },
                { 'ć', 'c' },
                { 'č', 'c' },
                { 'ç', 'c' },
                { 'đ', 'd' },
                { 'ð', 'd' },
                { 'ë', 'e' },
                { 'é', 'e' },
                { 'ê', 'e' },
                { 'è', 'e' },
                { 'ė', 'e' },
                { 'ệ', 'e' },
                { 'ế', 'e' },
                { 'ğ', 'g' },
                { 'ï', 'i' },
                { 'í', 'i' },
                { 'ì', 'i' },
                { 'î', 'i' },
                { 'ı', 'i' },
                { 'ĩ', 'i' },
                { 'ị', 'i' },
                { 'ñ', 'n' },
                { 'ó', 'o' },
                { 'ô', 'o' },
                { 'ō', 'o' },
                { 'õ', 'o' },
                { 'ö', 'o' },
                { 'ò', 'o' },
                { 'ồ', 'o' },
                { 'ố', 'o' },
                { 'ø', 'o' },
                { 'ş', 's' },
                { 'š', 's' },
                { 'ț', 't' },
                { 'ü', 'u' },
                { 'ú', 'u' },
                { 'ū', 'u' },
                { 'ũ', 'u' },
                { 'ừ', 'u' },
                { 'ž', 'z' },
            };

            return letterPairs;
        }
    }
}