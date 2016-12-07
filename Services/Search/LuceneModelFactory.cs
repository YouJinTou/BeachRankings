namespace BeachRankings.Services.Search
{
    using BeachRankings.Models.Interfaces;
    using BeachRankings.Services.Search.Enums;
    using BeachRankings.Services.Search.Models;
    using Lucene.Net.Documents;
    using System;

    internal class LuceneModelFactory
    {
        private ModelType modelType;

        public LuceneModelFactory(ModelType modelType)
        {
            this.modelType = modelType;
        }

        public ISearchable MapDocToModel(Document doc)
        {            
            switch (this.modelType)
            {
                case ModelType.Beach:
                    return this.MapDocToBeachModel(doc);
                case ModelType.Place:
                    return this.MapDocToPlace(doc);
                default:
                    return null;
            }
        }

        private ISearchable MapDocToBeachModel(Document doc)
        {
            var addressTokens = doc.Get("Address").Split(new char[] { '-' }, StringSplitOptions.RemoveEmptyEntries);
            var tokensCount = addressTokens.Length;
            var country = addressTokens[0];
            var primaryDivision = (tokensCount > 2) ? addressTokens[1] : null;
            var secondaryDivision = (tokensCount > 3) ? addressTokens[2] : null;
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

        private ISearchable MapDocToPlace(Document doc)
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