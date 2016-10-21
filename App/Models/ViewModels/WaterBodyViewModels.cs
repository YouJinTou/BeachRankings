namespace BeachRankings.App.Models.ViewModels
{
    using System.Collections.Generic;

    public class AddBeachWaterBodyViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }

    public class AutocompleteWaterBodyViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int BeachCount { get; set; }
    }

    public class WaterBodyBeachesViewModel
    {
        public string Name { get; set; }

        public IEnumerable<ConciseBeachViewModel> Beaches { get; set; }
    }
}