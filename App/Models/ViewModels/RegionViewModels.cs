namespace BeachRankings.App.Models.ViewModels
{
    using System.Collections.Generic;

    public class AutocompleteRegionViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int BeachCount { get; set; }
    }

    public class RegionBeachesViewModel
    {
        public string Name { get; set; }

        public IEnumerable<ConciseBeachViewModel> Beaches { get; set; }
    }
}