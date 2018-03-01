namespace BeachRankings.Services.Aggregation
{
    using BeachRankings.Models.Enums;
    using System.Collections.Generic;

    public class RankContainer
    {
        public int? ContinentId { get; set; }

        public int CountryId { get; set; }

        public int? PrimaryDivisionId { get; set; }

        public int WaterBodyId { get; set; }

        public BeachFilterType FilterType { get; set; }

        public IEnumerable<string> FilterDescriptors { get; set; }

        public string RankBy { get; set; }

        public int WorldRank { get; set; }

        public int ContinentRank { get; set; }

        public int CountryRank { get; set; }

        public int AreaRank { get; set; }

        public int WaterBodyRank { get; set; }
    }
}
