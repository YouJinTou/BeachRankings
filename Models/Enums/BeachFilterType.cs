namespace BeachRankings.Models.Enums
{
    using System.ComponentModel;

    public enum BeachFilterType
    {
        [Description("All Criteria")]
        AllCriteria,
        Camping,
        [Description("Holiday with Kids")]
        HolidayWithKids,
        Kayaking,
        [Description("Long-term Stay")]
        LongTermStay,
        Snorkeling,
        [Description("Taking a Walk")]
        Walking,
        [Description("Best Beach")]
        BestBeach
    }
}
