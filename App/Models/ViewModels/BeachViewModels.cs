namespace BeachRankings.App.Models.ViewModels
{
    using BeachRankings.Models;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    
    public class AddBeachViewModel
    {
        [Required(ErrorMessage = "The name field is required.")]
        [MinLength(2, ErrorMessage = "The name should be at least 2 characters long.")]
        [MaxLength(100, ErrorMessage = "The name cannot be longer than 100 characters.")]
        public string Name { get; set; }

        [Display(Name = "Country")]
        public string CountryName { get; set; }

        [Required(ErrorMessage = "The location field is required.")]
        [MinLength(2, ErrorMessage = "The location name should be at least 2 characters long.")]
        [MaxLength(100, ErrorMessage = "The location name cannot be longer than 100 characters.")]
        [Display(Name = "Location")]
        public string LocationName { get; set; }

        [MaxLength(350, ErrorMessage = "The description cannot be longer than 350 characters.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "A body of water is required.")]
        [MaxLength(100, ErrorMessage = "There is no body of water with that long of a name.")]
        [Display(Name = "Body of water")]
        public string WaterBody { get; set; }

        public string Coordinates { get; set; }
    }

    public class ConciseBeachViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Location { get; set; }

        public double? TotalScore { get; set; }

        public string Description { get; set; }

        public string ImagePath { get; set; }
    }

    public class DetailedBeachViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Location { get; set; }

        public double? TotalScore { get; set; }

        public string Description { get; set; }

        public string Coordinates { get; set; }

        [UIHint("ConciseReviewViewModel.cshtml")]
        public IEnumerable<ConciseReviewViewModel> Reviews { get; set; }

        public IEnumerable<BeachImage> Images { get; set; }
    }

    public class AutocompleteBeachViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Location { get; set; }

        public string WaterBody { get; set; }
    }    
}