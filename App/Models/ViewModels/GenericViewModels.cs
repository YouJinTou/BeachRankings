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
}