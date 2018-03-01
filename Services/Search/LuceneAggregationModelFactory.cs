namespace BeachRankings.Services.Search
{
    using BeachRankings.Models.Interfaces;
    using BeachRankings.Services.Aggregation;
    using Lucene.Net.Documents;

    internal class LuceneAggregationModelFactory
    {
        public IBeachAggregatable MapDocToBeachAggregate(Document doc)
        {
            return new BeachAggregationModel(
                int.Parse(doc.Get("Id")),
                int.Parse(doc.Get("ContinentId")),
                int.Parse(doc.Get("CountryId")),
                int.Parse(doc.Get("PrimaryDivisionId")),
                int.Parse(doc.Get("WaterBodyId")),
                double.Parse(doc.Get("TotalScore")),
                double.Parse(doc.Get("SandQuality")),
                double.Parse(doc.Get("BeachCleanliness")),
                double.Parse(doc.Get("BeautifulScenery")),
                double.Parse(doc.Get("CrowdFree")),
                double.Parse(doc.Get("Infrastructure")),
                double.Parse(doc.Get("WaterVisibility")),
                double.Parse(doc.Get("LitterFree")),
                double.Parse(doc.Get("FeetFriendlyBottom")),
                double.Parse(doc.Get("SeaLifeDiversity")),
                double.Parse(doc.Get("CoralReef")),
                double.Parse(doc.Get("Snorkeling")),
                double.Parse(doc.Get("Kayaking")),
                double.Parse(doc.Get("Walking")),
                double.Parse(doc.Get("Camping")),
                double.Parse(doc.Get("LongTermStay"))
            );
        }
    }
}
