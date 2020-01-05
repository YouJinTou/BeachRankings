namespace BR.Core.Abstractions
{
    public interface IScorable
    {
        double? Score { get; set; }

        double? SandQuality { get; set; }

        double? BeachCleanliness { get; set; }

        double? BeautifulScenery { get; set; }

        double? CrowdFree { get; set; }

        double? Infrastructure { get; set; }

        double? WaterVisibility { get; set; }

        double? LitterFree { get; set; }

        double? FeetFriendlyBottom { get; set; }

        double? SeaLifeDiversity { get; set; }

        double? CoralReef { get; set; }

        double? Snorkeling { get; set; }

        double? Kayaking { get; set; }

        double? Walking { get; set; }

        double? Camping { get; set; }

        double? LongTermStay { get; set; }
    }
}
