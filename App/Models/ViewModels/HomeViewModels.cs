namespace BeachRankings.App.Models.ViewModels
{
    using System.Collections.Generic;

    public class SearchViewModel
    {
        public IEnumerable<AutocompleteBeachViewModel> Beaches { get; set; }

        public IEnumerable<AutocompleteViewModel> WaterBodies { get; set; }

        public IEnumerable<AutocompleteViewModel> PrimaryDivisions { get; set; }

        public IEnumerable<AutocompleteViewModel> SecondaryDivisions { get; set; }

        public IEnumerable<AutocompleteViewModel> TertiaryDivisions { get; set; }

        public IEnumerable<AutocompleteViewModel> QuaternaryDivisions { get; set; }

        public IEnumerable<AutocompleteViewModel> Countries { get; set; }
    }
}