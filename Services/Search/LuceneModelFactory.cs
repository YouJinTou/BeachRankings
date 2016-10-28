namespace BeachRankings.Services.Search
{
    using BeachRankings.Models.Interfaces;
    using BeachRankings.Services.Search.Enums;
    using BeachRankings.Services.Search.Models;
    using Lucene.Net.Documents;

    internal static class LuceneModelFactory
    {
        public static ISearchable MapDocToModel(Document doc)
        {
            switch (LuceneSearch.ModelType)
            {
                case ModelType.Beach:
                    return MapDocToBeachModel(doc);
                case ModelType.PrimaryDivision:
                    return MapDocToPrimaryDivisionModel(doc);
                case ModelType.SecondaryDivision:
                    return MapDocToSecondaryDivisionModel(doc);
                case ModelType.TertiaryDivision:
                    return MapDocToTertiaryDivisionModel(doc);
                case ModelType.QuaternaryDivision:
                    return MapDocToQuaternaryDivisionModel(doc);
                case ModelType.Country:
                    return MapDocToCountryModel(doc);
                case ModelType.WaterBody:
                    return MapDocToWaterBodyModel(doc);                         
                default:
                    return null;
            }
        }

        private static ISearchable MapDocToBeachModel(Document doc)
        {
            return new BeachSearchResultModel()
            {
                Id = int.Parse(doc.Get("Id")),
                Name = doc.Get("Name"),
                Description = doc.Get("Description"),
                Address = doc.Get("Address"),
                Coordinates = doc.Get("Coordinates")
            };
        }

        private static ISearchable MapDocToPrimaryDivisionModel(Document doc)
        {
            return new PlaceSearchResultModel()
            {
                Id = int.Parse(doc.Get("Id")),
                Name = doc.Get("Name"),
                BeachCount = int.Parse(doc.Get("BeachCount"))
            };
        }

        private static ISearchable MapDocToSecondaryDivisionModel(Document doc)
        {
            return new PlaceSearchResultModel()
            {
                Id = int.Parse(doc.Get("Id")),
                Name = doc.Get("Name"),
                BeachCount = int.Parse(doc.Get("BeachCount"))
            };
        }

        private static ISearchable MapDocToTertiaryDivisionModel(Document doc)
        {
            return new PlaceSearchResultModel()
            {
                Id = int.Parse(doc.Get("Id")),
                Name = doc.Get("Name"),
                BeachCount = int.Parse(doc.Get("BeachCount"))
            };
        }

        private static ISearchable MapDocToQuaternaryDivisionModel(Document doc)
        {
            return new PlaceSearchResultModel()
            {
                Id = int.Parse(doc.Get("Id")),
                Name = doc.Get("Name"),
                BeachCount = int.Parse(doc.Get("BeachCount"))
            };
        }

        private static ISearchable MapDocToWaterBodyModel(Document doc)
        {
            return new PlaceSearchResultModel()
            {
                Id = int.Parse(doc.Get("Id")),
                Name = doc.Get("Name"),
                BeachCount = int.Parse(doc.Get("BeachCount"))
            };
        }

        private static ISearchable MapDocToCountryModel(Document doc)
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