namespace BeachRankings.App.Models.ViewModels
{
    using BeachRankings.App.CustomAttributes;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class ConciseReviewViewModel
    {
        public int Id { get; set; }

        public string AuthorId { get; set; }

        public string UserName { get; set; }

        public bool IsBlogger { get; set; }

        public string BlogUrl { get; set; }

        public IEnumerable<BlogArticleViewModel> BlogArticles { get; set; }

        public int ReviewsCount { get; set; }

        public int CountriesVisited { get; set; }

        public int Level { get; set; }

        public string AvatarPath { get; set; }

        public DateTime PostedOn { get; set; }

        [Range(0, 10)]
        public double? TotalScore { get; set; }

        public string Content { get; set; }

        public int Upvotes { get; set; }

        public bool AlreadyUpvoted { get; set; }
    }

    public class DetailedReviewViewModel : CriteriaBaseModel
    {
        public int Id { get; set; }

        public string UserName { get; set; }

        public int ReviewsCount { get; set; }

        public int CountriesVisited { get; set; }

        public int Level { get; set; }

        public string AvatarPath { get; set; }

        public DateTime PostedOn { get; set; }

        public string Content { get; set; }

        public string AuthorId { get; set; }

        public bool IsBlogger { get; set; }

        public string BlogUrl { get; set; }

        public IEnumerable<BlogArticleViewModel> BlogArticles { get; set; }

        public bool AlreadyUpvoted { get; set; }

        public int Upvotes { get; set; }

        [Range(0, 10)]
        public double? TotalScore { get; set; }

        public int BeachId { get; set; }

        public string BeachName { get; set; }

        public string BeachCountry { get; set; }

        public int BeachCountryId { get; set; }

        public string BeachPrimaryDivision { get; set; }

        public int? BeachPrimaryDivisionId { get; set; }

        public string BeachSecondaryDivision { get; set; }

        public int? BeachSecondaryDivisionId { get; set; }

        public int BeachReviewsCount { get; set; }

        public IEnumerable<BeachImageThumbnailViewModel> BeachImages { get; set; }

        [Range(0, 10)]
        public double? BeachTotalScore { get; set; }

        #region Beachline

        [Display(Name = "Sand quality")]
        public double? BeachSandQuality { get; set; }

        [Display(Name = "Beach cleanliness")]
        public double? BeachBeachCleanliness { get; set; }

        [Display(Name = "Beautiful scenery")]
        public double? BeachBeautifulScenery { get; set; }

        [Display(Name = "Crowd-free")]
        public double? BeachCrowdFree { get; set; }

        #endregion

        #region Water

        [Display(Name = "Water purity")]
        public double? BeachWaterPurity { get; set; }

        [Display(Name = "Waste-free seabed")]
        public double? BeachWasteFreeSeabed { get; set; }

        [Display(Name = "Feet-friendly bottom")]
        public double? BeachFeetFriendlyBottom { get; set; }

        [Display(Name = "Sea life diversity")]
        public double? BeachSeaLifeDiversity { get; set; }

        [Display(Name = "Coral reef wow factor")]
        public double? BeachCoralReef { get; set; }

        #endregion

        #region Activities

        [Display(Name = "Taking a walk")]
        public double? BeachWalking { get; set; }

        [Display(Name = "Snorkeling")]
        public double? BeachSnorkeling { get; set; }

        [Display(Name = "Kayaking")]
        public double? BeachKayaking { get; set; }

        [Display(Name = "Camping")]
        public double? BeachCamping { get; set; }

        #endregion

        #region Tourist Infrastructure

        [Display(Name = "Environment-friendly infrastructure")]
        public double? BeachInfrastructure { get; set; }

        [Display(Name = "Long-term stay")]
        public double? BeachLongTermStay { get; set; }

        #endregion        

        public bool UserHasRated { get; set; }
    }

    public class PostReviewViewModel : CriteriaBaseModel
    {
        public int BeachId { get; set; }

        [Required(ErrorMessage = "The review field is required.")]
        [MinLength(100, ErrorMessage = "100 characters should be doable.")]
        [MaxLength(3000, ErrorMessage = "We'll happily accept 3000 symbols and below.")]
        [Display(Name = "Review")]
        public string Content { get; set; }

        public bool IsBlogger { get; set; }

        [Display(Name = "Blog article links")]
        [UrisValid(ErrorMessage = "We couldn't process the provided links.")]
        public string ArticleLinks { get; set; }

        public string BeachName { get; set; }

        public string BeachCountry { get; set; }

        public int CountryId { get; set; }

        public string BeachPrimaryDivision { get; set; }

        public int? PrimaryDivisionId { get; set; }

        public string BeachSecondaryDivision { get; set; }

        public int? SecondaryDivisionId { get; set; }

        public double? BeachTotalScore { get; set; }

        public int BeachReviewsCount { get; set; }

        public IEnumerable<BeachImageThumbnailViewModel> BeachImagePaths { get; set; }
    }

    public class EditReviewViewModel : CriteriaBaseModel
    {
        public int Id { get; set; }

        public string AuthorId { get; set; }

        [Required(ErrorMessage = "The review field is required.")]
        [MinLength(100, ErrorMessage = "100 characters should be doable.")]
        [MaxLength(3000, ErrorMessage = "We'll happily accept 3000 symbols and below.")]
        [Display(Name = "Review")]
        public string Content { get; set; }

        public bool IsBlogger { get; set; }

        [Display(Name = "Blog article links")]
        [UrisValid(ErrorMessage = "We couldn't process the provided links.")]
        public string ArticleLinks { get; set; }

        public int BeachId { get; set; }

        public string BeachName { get; set; }

        public string BeachCountry { get; set; }

        public int CountryId { get; set; }

        public string BeachPrimaryDivision { get; set; }

        public int? PrimaryDivisionId { get; set; }

        public string BeachSecondaryDivision { get; set; }

        public int? SecondaryDivisionId { get; set; }

        public double? BeachTotalScore { get; set; }

        public int BeachReviewsCount { get; set; }

        public IEnumerable<BeachImageThumbnailViewModel> BeachImagePaths { get; set; }                
    }
}