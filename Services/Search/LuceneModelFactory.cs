namespace BeachRankings.Services.Search
{
    using BeachRankings.Models.Interfaces;
    using BeachRankings.Services.Search.Enums;
    using BeachRankings.Services.Search.Models;
    using Lucene.Net.Documents;
    using System;

    internal static class LuceneModelFactory
    {
        public static ISearchable MapDocToModel(Document doc)
        {
            switch (LuceneSearch.ModelType)
            {
                case ModelType.Beach:
                    return MapDocToBeachModel(doc);
                case ModelType.Place:
                    return MapDocToPlace(doc);                       
                default:
                    return null;
            }
        }

        private static ISearchable MapDocToBeachModel(Document doc)
        {
            var addressTokens = doc.Get("Address").Split(new char[] { '-' }, StringSplitOptions.RemoveEmptyEntries);
            var tokensCount = addressTokens.Length;
            var country = addressTokens[0];
            var primaryDivision = addressTokens[1];
            var secondaryDivision = addressTokens[2];
            var tertiaryDivision = (tokensCount > 4) ? addressTokens[3] : null;
            var quaternaryDivision = (tokensCount > 5) ? addressTokens[4] : null;
            var waterBody = addressTokens[tokensCount - 1];

            return new BeachSearchResultModel()
            {
                Id = int.Parse(doc.Get("Id")),
                Name = doc.Get("Name"),
                Description = doc.Get("Description"),
                Country = country,
                PrimaryDivision = primaryDivision,
                SecondaryDivision = secondaryDivision,
                TertiaryDivision = tertiaryDivision,
                QuaternaryDivision = quaternaryDivision,
                WaterBody = waterBody,
                Coordinates = doc.Get("Coordinates")
            };
        }

        private static ISearchable MapDocToPlace(Document doc)
        {
            return new PlaceSearchResultModel()
            {
                Id = int.Parse(doc.Get("Id")),
                Name = doc.Get("Name"),
                BeachCount = int.Parse(doc.Get("BeachCount"))
            };
        }        
    }
}