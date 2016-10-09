namespace BeachRankings.Models.Misc.Interfaces
{
    public interface IRateable
    {
        double? WaterQuality { get; set; }

        double? SeafloorCleanliness { get; set; }

        double? CoralReefFactor { get; set; }

        double? SeaLifeDiversity { get; set; }

        double? SnorkelingSuitability { get; set; }

        double? BeachCleanliness { get; set; }

        double? CrowdFreeFactor { get; set; }

        double? SandQuality { get; set; }

        double? BreathtakingEnvironment { get; set; }

        double? TentSuitability { get; set; }

        double? KayakSuitability { get; set; }

        double? LongStaySuitability { get; set; }
    }
}