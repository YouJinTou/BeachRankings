namespace BeachRankings.Models.Enums
{
    using System.ComponentModel;

    public enum Criterion
    {
        [Description("Sand quality")]
        SandQuality = 1,
        [Description("Beach cleanliness")]
        BeachCleanliness,
        [Description("Beautiful scenery")]
        BeautifulScenery,
        [Description("Crowd-free")]
        CrowdFree,
        [Description("Infrastructure")]
        Infrastructure,

        [Description("Water visibility")]
        WaterVisibility,
        [Description("Litter-free water")]
        LitterFreeWater,
        [Description("Feet-friendly bottom")]
        FeetFriendlyBottom,
        [Description("Sea life diversity")]
        SeaLifeDiversity,
        [Description("Coral reef")]
        CoralReef,

        [Description("Snorkeling")]
        Snorkeling,
        [Description("Kayaking")]
        Kayaking,
        [Description("Taking a walk")]
        Walking,
        [Description("Camping")]
        Camping,
        [Description("Long-term stay")]
        LongTermStay
    }
}