namespace BeachRankings.App.Models.ViewModels
{
    using System.Collections.Generic;

    public class AutocompleteMainViewModel
    {
        public IEnumerable<AutocompleteBeachViewModel> Beaches { get; set; }

        public IEnumerable<AutocompleteWaterBodyViewModel> WaterBodies { get; set; }

        public IEnumerable<AutocompleteLocationViewModel> Locations { get; set; }

        public IEnumerable<AutocompleteCountryViewModel> Countries { get; set; }
    }
}