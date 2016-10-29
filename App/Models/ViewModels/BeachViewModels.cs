namespace BeachRankings.App.Models.ViewModels
{
    using BeachRankings.Models;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web;
    using System.Web.Mvc;
    using BeachRankings.App.ValidationAttributes;

    public class AddBeachViewModel
    {
        [Required(ErrorMessage = "The name field is required.")]
        [MinLength(2, ErrorMessage = "The name should be at least 2 characters long.")]
        [MaxLength(100, ErrorMessage = "The name cannot be longer than 100 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "The country field is required.")]
        [Display(Name = "Country")]
        public int CountryId { get; set; }

        [Required(ErrorMessage = "The first-level field is required.")]
        [Display(Name = "First-level division")]
        public int PrimaryDivisionId { get; set; }       

        [Required(ErrorMessage = "The second-level field is required.")]
        [Display(Name = "Second-level division")]
        public int SecondaryDivisionId { get; set; }

        [Display(Name = "Third-level division")]
        public int? TertiaryDivisionId { get; set; }

        [Display(Name = "Fourth-level division")]
        public int? QuaternaryDivisionId { get; set; }

        public IEnumerable<SelectListItem> Countries { get; set; }

        [MaxLength(350, ErrorMessage = "The description cannot be longer than 350 characters.")]
        public string Description { get; set; }        

        [RegularExpression(
            @"[-+]?([1-8]?\d(\.\d+)?|90(\.0+)?),\s*[-+]?(180(\.0+)?|((1[0-7]\d)|([1-9]?\d))(\.\d+)?)", 
            ErrorMessage = "Invalid coordinates.")]
        public string Coordinates { get; set; }

        [ImagesValid(ErrorMessage = "Failed to upload images. Verify their count, size, and format.")]
        public IEnumerable<HttpPostedFileBase> Images { get; set; }
    }

    public class ConciseBeachViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Country { get; set; }

        public string SecondaryDivision { get; set; }

        public double? TotalScore { get; set; }

        public string Description { get; set; }

        public string ImagePath { get; set; }
    }

    public class DetailedBeachViewModel : ConciseBeachViewModel
    {
        public string PrimaryDivision { get; set; }

        public string WaterBody { get; set; }

        public string Coordinates { get; set; }

        [UIHint("ConciseReviewViewModel.cshtml")]
        public IEnumerable<ConciseReviewViewModel> Reviews { get; set; }

        public IEnumerable<BeachImage> Images { get; set; }
    }

    public class AutocompleteBeachViewModel : AutocompleteBaseViewModel
    {
        public string Country { get; set; }

        public string SecondaryDivision { get; set; }
    }    
}