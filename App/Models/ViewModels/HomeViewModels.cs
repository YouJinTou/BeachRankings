namespace BeachRankings.App.Models.ViewModels
{
    using System.Collections.Generic;

    public class UserOverviewViewModel
    {
        public string AvatarPath { get; set; }

        public int Level { get; set; }

        public string PointsToNextLevel { get; set; }
    }

    public class SearchViewModel
    {
        public IEnumerable<AutocompleteBeachViewModel> Beaches { get; set; }

        public IEnumerable<AutocompleteWaterBodyViewModel> WaterBodies { get; set; }

        public IEnumerable<AutocompletePrimaryViewModel> PrimaryDivisions { get; set; }

        public IEnumerable<AutocompleteSecondaryViewModel> SecondaryDivisions { get; set; }

        public IEnumerable<AutocompleteTertiaryViewModel> TertiaryDivisions { get; set; }

        public IEnumerable<AutocompleteQuaternaryViewModel> QuaternaryDivisions { get; set; }

        public IEnumerable<AutocompleteCountryViewModel> Countries { get; set; }

        public IEnumerable<AutocompleteContinentViewModel> Continents { get; set; }
    }
}