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
                case ModelType.Region:
                    return MapDocToRegionModel(doc);
                case ModelType.Area:
                    return MapDocToAreaModel(doc);
                case ModelType.WaterBody:
                    return MapDocToWaterBodyModel(doc);                
                case ModelType.Country:
                    return MapDocToCountryModel(doc);          
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

        private static ISearchable MapDocToRegionModel(Document doc)
        {
            return new RegionSearchResultModel()
            {
                Id = int.Parse(doc.Get("Id")),
                Name = doc.Get("Name")
            };
        }

        private static ISearchable MapDocToAreaModel(Document doc)
        {
            return new AreaSearchResultModel()
            {
                Id = int.Parse(doc.Get("Id")),
                Name = doc.Get("Name")
            };
        }

        private static ISearchable MapDocToWaterBodyModel(Document doc)
        {
            return new WaterBodySearchResultModel()
            {
                Id = int.Parse(doc.Get("Id")),
                Name = doc.Get("Name")
            };
        }

        private static ISearchable MapDocToCountryModel(Document doc)
        {
            return new RegionSearchResultModel()
            {
                Id = int.Parse(doc.Get("Id")),
                Name = doc.Get("Name")
            };
        }
    }
}