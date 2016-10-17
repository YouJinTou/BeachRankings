namespace App.Models.ViewModels
{
    public class WaterBodyViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }

    public class AutocompleteLocationViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int BeachCount { get; set; }
    }
}