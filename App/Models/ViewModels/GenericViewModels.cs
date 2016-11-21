namespace BeachRankings.App.Models.ViewModels
{
    using System.Collections.Generic;

    public class AutocompleteBaseViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }

    public class LocationBeachesViewModel
    {
        public string Name { get; set; }

        public IEnumerable<ConciseBeachViewModel> Beaches { get; set; }
    }

    public class TableRowViewModel : CriteriaBaseModel
    {
        public int BeachId { get; set; }

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
}