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
                case ModelType.Continent:
                    return this.MapDocToContinentModel(doc);
                case ModelType.Country:
                    return this.MapDocToCountryModel(doc);
                case ModelType.PrimaryDivision:
                    return this.MapDocToPrimaryDivisionModel(doc);
                case ModelType.SecondaryDivision:
                    return this.MapDocToSecondaryDivisionModel(doc);
                case ModelType.TertiaryDivision:
                    return this.MapDocToTertiaryDivisionModel(doc);
                case ModelType.QuaternaryDivision:
                    return this.MapDocToQuaternaryDivisionModel(doc);
                case ModelType.WaterBody:
                    return this.MapDocToWaterBodyModel(doc);
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

        private ISearchable MapDocToContinentModel(Document doc)
        {
            return new ContinentSearchResultModel()
            {
                Id = int.Parse(doc.Get("Id")),
                Name = doc.Get("Name"),
                BeachCount = int.Parse(doc.Get("BeachCount"))
            };
        }

        private ISearchable MapDocToCountryModel(Document doc)
        {
            return new CountrySearchResultModel()
            {
                Id = int.Parse(doc.Get("Id")),
                Name = doc.Get("Name"),
                ContinentName = doc.Get("ContinentName"),
                BeachCount = int.Parse(doc.Get("BeachCount"))
            };
        }

        private ISearchable MapDocToPrimaryDivisionModel(Document doc)
        {
            return new PrimaryDivisionSearchResultModel()
            {
                Id = int.Parse(doc.Get("Id")),
                Name = doc.Get("Name"),
                CountryName = doc.Get("CountryName"),
                BeachCount = int.Parse(doc.Get("BeachCount"))
            };
        }

        private ISearchable MapDocToSecondaryDivisionModel(Document doc)
        {
            return new SecondaryDivisionSearchResultModel()
            {
                Id = int.Parse(doc.Get("Id")),
                Name = doc.Get("Name"),
                CountryName = doc.Get("CountryName"),
                PrimaryDivisionName = doc.Get("PrimaryName"),
                BeachCount = int.Parse(doc.Get("BeachCount"))
            };
        }
        
        private ISearchable MapDocToTertiaryDivisionModel(Document doc)
        {
            return new TertiaryDivisionSearchResultModel()
            {
                Id = int.Parse(doc.Get("Id")),
                Name = doc.Get("Name"),
                CountryName = doc.Get("CountryName"),
                PrimaryDivisionName = doc.Get("PrimaryName"),
                SecondaryDivisionName = doc.Get("SecondaryName"),
                BeachCount = int.Parse(doc.Get("BeachCount"))
            };
        }

        private ISearchable MapDocToQuaternaryDivisionModel(Document doc)
        {
            return new QuaternaryDivisionSearchResultModel()
            {
                Id = int.Parse(doc.Get("Id")),
                Name = doc.Get("Name"),
                CountryName = doc.Get("CountryName"),
                PrimaryDivisionName = doc.Get("PrimaryName"),
                SecondaryDivisionName = doc.Get("SecondaryName"),
                TertiaryDivisionName = doc.Get("TertiaryName"),
                BeachCount = int.Parse(doc.Get("BeachCount"))
            };
        }

        private ISearchable MapDocToWaterBodyModel(Document doc)
        {
            return new WaterBodySearchResultModel()
            {
                Id = int.Parse(doc.Get("Id")),
                Name = doc.Get("Name"),
                BeachCount = int.Parse(doc.Get("BeachCount"))
            };
        }
    }
}