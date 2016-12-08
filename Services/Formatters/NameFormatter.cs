namespace BeachRankings.Services.Formatters
{
    using BeachRankings.Services.Initializers;
    using System;
    using System.Text;
    using System.Text.RegularExpressions;

    public class NameFormatter
    {
        public string GetFormattedBeachName(string name)
        {
            var result = name.ToLower().Trim();

            if (!result.Contains("beach"))
            {
                result += " beach";
            }

            result = this.GetFormattedPlaceName(result);

            return result;
        }

        public string GetFormattedPlaceName(string name)
        {
            var result = this.ReplaceNonLatinLetters(name);
            var lettersDigitsPattern = @"[^A-Za-z0-9]";
            var multipleSpacesPattern = @"\s\s+";
            result = Regex.Replace(result, lettersDigitsPattern, " ").ToLower().Trim();
            result = Regex.Replace(result, multipleSpacesPattern, " ");

            return result;
        }

        public string GetOutboundName(string name)
        {
            var tokens = name.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < tokens.Length; i++)
            {
                var firstLetter = tokens[i][0].ToString().ToUpper();
                tokens[i] = firstLetter + tokens[i].Substring(1);
            }

            var result = string.Join(" ", tokens);

            return result;
        }

        private string ReplaceNonLatinLetters(string name)
        {
            var result = new StringBuilder();

            foreach (var character in name)
            {
                if (LetterInitializer.LetterPairs.ContainsKey(character))
                {
                    result.Append(LetterInitializer.LetterPairs[character]);

                    continue;
                }

                result.Append(character);
            }

            return result.ToString();       
        }
    }
}