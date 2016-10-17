﻿namespace BeachRankings.Services.Search
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
                case ModelType.Location:
                    return MapDocToLocationModel(doc);
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
                LocationName = doc.Get("LocationName"),
                WaterBody = doc.Get("WaterBody"),
                ApproximateAddress = doc.Get("ApproximateAddress"),
                Coordinates = doc.Get("Coordinates")
            };
        }

        private static ISearchable MapDocToLocationModel(Document doc)
        {
            return new LocationSearchResultModel()
            {
                Id = int.Parse(doc.Get("Id")),
                Name = doc.Get("Name")
            };
        }
    }
}