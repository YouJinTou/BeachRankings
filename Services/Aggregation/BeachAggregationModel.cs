namespace BeachRankings.Services.Aggregation
{
    using BeachRankings.Models.Interfaces;

    internal class BeachAggregationModel : IBeachAggregatable
    {
        public BeachAggregationModel(
            int id,
            int? continentId,
            int countryId,
            int? primaryDivisionId,
            int waterBodyId,
            double? totalScore,
            double? sandQuality,
            double? beachCleanliness,
            double? beautifulScenery,
            double? crowdFree,
            double? infrastructure,
            double? waterVisibility,
            double? litterFree,
            double? feetFriendlyBottom,
            double? seaLifeDiversity,
            double? coralReef,
            double? snorkeling,
            double? kayaking,
            double? walking,
            double? camping,
            double? longTermStay)
        {
            this.Id = id;
            this.ContinentId = (int)this.ParseNullableNumber(continentId);
            this.CountryId = countryId;
            this.PrimaryDivisionId = (int)this.ParseNullableNumber(primaryDivisionId);
            this.WaterBodyId = waterBodyId;

            this.TotalScore = this.ParseNullableNumber(totalScore);
            this.SandQuality = this.ParseNullableNumber(sandQuality);
            this.BeachCleanliness = this.ParseNullableNumber(beachCleanliness);
            this.BeautifulScenery = this.ParseNullableNumber(beautifulScenery);
            this.CrowdFree = this.ParseNullableNumber(crowdFree);
            this.Infrastructure = this.ParseNullableNumber(infrastructure);

            this.WaterVisibility = this.ParseNullableNumber(waterVisibility);
            this.LitterFree = this.ParseNullableNumber(litterFree);
            this.FeetFriendlyBottom = this.ParseNullableNumber(feetFriendlyBottom);
            this.SeaLifeDiversity = this.ParseNullableNumber(seaLifeDiversity);
            this.CoralReef = this.ParseNullableNumber(coralReef);

            this.Snorkeling = this.ParseNullableNumber(snorkeling);
            this.Kayaking = this.ParseNullableNumber(kayaking);
            this.Walking = this.ParseNullableNumber(walking);
            this.Camping = this.ParseNullableNumber(camping);
            this.LongTermStay = this.ParseNullableNumber(longTermStay);
        }

        public int Id { get; set; }

        public int? ContinentId { get; set; }

        public int CountryId { get; set; }

        public int? PrimaryDivisionId { get; set; }

        public int WaterBodyId { get; set; }

        public double? TotalScore { get; set; }

        public double? SandQuality { get; set; }

        public double? BeachCleanliness { get; set; }

        public double? BeautifulScenery { get; set; }

        public double? CrowdFree { get; set; }

        public double? Infrastructure { get; set; }

        public double? WaterVisibility { get; set; }

        public double? LitterFree { get; set; }

        public double? FeetFriendlyBottom { get; set; }

        public double? SeaLifeDiversity { get; set; }

        public double? CoralReef { get; set; }

        public double? Snorkeling { get; set; }

        public double? Kayaking { get; set; }

        public double? Walking { get; set; }

        public double? Camping { get; set; }

        public double? LongTermStay { get; set; }

        private double? ParseNullableNumber(double? value)
        {
            return value == -1 ? null : value;
        }
    }
}
