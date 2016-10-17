﻿namespace App.Models.ViewModels
{
    using System.Collections.Generic;

    public class AutocompleteMainViewModel
    {
        public IEnumerable<AutocompleteBeachViewModel> Beaches { get; set; }

        public IEnumerable<AutocompleteLocationViewModel> Locations { get; set; }
    }
}