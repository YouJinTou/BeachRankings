namespace BeachRankings.App.Models.ViewModels
{
    using BeachRankings.Models;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;

    public class AddBeachViewModel
    {
        [Required(ErrorMessage = "The name field is required.")]
        [MinLength(2, ErrorMessage = "The name should be at least 2 characters long.")]
        [MaxLength(100, ErrorMessage = "The name cannot be longer than 100 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "The country field is required.")]
        [Display(Name = "Country")]
        public int CountryId { get; set; }

        [Required(ErrorMessage = "The region field is required.")]
        [Display(Name = "Region")]
        public int RegionId { get; set; }       

        [Required(ErrorMessage = "The area field is required.")]
        [Display(Name = "Area")]
        public int AreaId { get; set; }

        public IEnumerable<SelectListItem> Countries { get; set; }

        [MaxLength(350, ErrorMessage = "The description cannot be longer than 350 characters.")]
        public string Description { get; set; }        

        [RegularExpression(
            @"[-+]?([1-8]?\d(\.\d+)?|90(\.0+)?),\s*[-+]?(180(\.0+)?|((1[0-7]\d)|([1-9]?\d))(\.\d+)?)", 
            ErrorMessage = "Invalid coordinates.")]
        public string Coordinates { get; set; }

        public string Image { get; set; }
    }

    public class ConciseBeachViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Country { get; set; }

        public string Area { get; set; }

        public double? TotalScore { get; set; }

        public string Description { get; set; }

        public string ImagePath { get; set; }
    }

    public class DetailedBeachViewModel : ConciseBeachViewModel
    {
        public string Region { get; set; }

        public string WaterBody { get; set; }

        public string Coordinates { get; set; }

        [UIHint("ConciseReviewViewModel.cshtml")]
        public IEnumerable<ConciseReviewViewModel> Reviews { get; set; }

        public IEnumerable<BeachImage> Images { get; set; }
    }

    public class AutocompleteBeachViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Country { get; set; }

        public string Area { get; set; }
    }    
}