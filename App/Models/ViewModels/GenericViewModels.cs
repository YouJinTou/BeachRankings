namespace BeachRankings.App.Models.ViewModels
{
    using BeachRankings.Models.Enums;
    using System.Collections.Generic;

    public class AutocompleteBaseViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }

    public abstract class StatsHeadViewModel
    {
        public int Id { get; set; }

        public string Controller { get; set; }

        public abstract string Action { get; }

        public string Name { get; set; }

        public int TotalBeachesCount { get; set; }
    }

    public class PlaceBeachesViewModel : StatsHeadViewModel
    {
        public override string Action => "Statistics";

        public IEnumerable<ConciseBeachViewModel> Beaches { get; set; }
    }

    public class StatisticsViewModel : StatsHeadViewModel
    {
        public override string Action => "Beaches";

        public int SortingCriterion { get; set; }

        public string FilterType { get; set; }

        public IEnumerable<BeachRowViewModel> Rows { get; set; }
    }

    public class WatchlistStatisticsViewModel : StatisticsViewModel
    {
        public override string Action => "GetWatchlistCriteriaTables";
    }

    public class BeachRowViewModel : CriteriaBaseModel
    {
        public int BeachId { get; set; }

        public int? ReviewId { get; set; }

        public string BeachName { get; set; }

        public string Country { get; set; }

        public int CountryId { get; set; }

        public string PrimaryDivision { get; set; }

        public int? PrimaryDivisionId { get; set; }

        public string SecondaryDivision { get; set; }

        public int? SecondaryDivisionId { get; set; }

        public string TertiaryDivision { get; set; }

        public int? TertiaryDivisionId { get; set; }

        public string QuaternaryDivision { get; set; }

        public int? QuaternaryDivisionId { get; set; }

        public string WaterBody { get; set; }

        public int WaterBodyId { get; set; }

        public double? TotalScore { get; set; }
    }

    public class ExportScoresAsHtmlViewModel : CriteriaBaseModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int CountryId { get; set; }

        public string Country { get; set; }

        public int? PrimaryDivisionId { get; set; }

        public string PrimaryDivision { get; set; }

        public int? SecondaryDivisionId { get; set; }

        public string SecondaryDivision { get; set; }

        public double? TotalScore { get; set; }
    }

    public class CriteriaViewModel : CriteriaBaseModel
    {
    }

    public class HorizontalCriteriaViewModel : CriteriaViewModel
    {
        public string BeachName { get; set; }

        public string PrimaryDivisionName { get; set; }

        public double? TotalScore { get; set; }
    }

    public class WatchlistCriteriaTablesViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public ICollection<HorizontalCriteriaViewModel> CriteriaTableRows { get; set; }
    }

    public class CrossTableRowViewModel
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