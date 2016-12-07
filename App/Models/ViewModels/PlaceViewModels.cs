namespace BeachRankings.App.Models.ViewModels
{
    public class AutocompleteContinentViewModel : AutocompleteBaseViewModel
    {
        public int BeachCount { get; set; }
    }

    public class AutocompleteCountryViewModel : AutocompleteBaseViewModel
    {
        public string Continent { get; set; }

        public int BeachCount { get; set; }
    }

    public class AutocompletePrimaryViewModel : AutocompleteBaseViewModel
    {
        public string Country { get; set; }

        public int BeachCount { get; set; }
    }

    public class AutocompleteSecondaryViewModel : AutocompleteBaseViewModel
    {
        public string Country { get; set; }

        public string PrimaryDivision { get; set; }

        public int BeachCount { get; set; }
    }

    public class AutocompleteTertiaryViewModel : AutocompleteBaseViewModel
    {
        public string Country { get; set; }

        public string PrimaryDivision { get; set; }

        public string SecondaryDivision { get; set; }

        public int BeachCount { get; set; }
    }

    public class AutocompleteQuaternaryViewModel : AutocompleteBaseViewModel
    {
        public string Country { get; set; }

        public string PrimaryDivision { get; set; }

        public string SecondaryDivision { get; set; }

        public string TertiaryDivision { get; set; }

        public int BeachCount { get; set; }
    }

    public class AutocompleteWaterBodyViewModel : AutocompleteBaseViewModel
    {
        public int BeachCount { get; set; }
    }
}