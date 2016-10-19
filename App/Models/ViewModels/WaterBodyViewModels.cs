namespace BeachRankings.App.Models.ViewModels
{
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
}