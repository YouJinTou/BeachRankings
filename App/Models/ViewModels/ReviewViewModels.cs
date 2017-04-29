namespace BeachRankings.App.Models.ViewModels
{
    using BeachRankings.App.CustomAttributes;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web;

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

        public CriteriaViewModel Criteria { get; set; }
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

        public bool UserHasRated { get; set; }

        public int Upvotes { get; set; }
        
        [Range(0, 10)]
        public double? TotalScore { get; set; }

        public DetailedBeachViewModel BeachHead { get; set; }        
    }

    public class PostReviewViewModel : CriteriaBaseModel
    {
        public DetailedBeachViewModel BeachHead { get; set; }

        [MaxLength(3000, ErrorMessage = "We'll happily accept 3000 symbols and below.")]
        [Display(Name = "Review")]
        public string Content { get; set; }

        public bool IsBlogger { get; set; }

        [Display(Name = "Blog article links")]
        [UrisValid(ErrorMessage = "We couldn't process the provided links.")]
        public string ArticleLinks { get; set; }
        
        public int BeachReviewsCount { get; set; }

        [ImagesValid(ErrorMessage = "Failed to upload images. Verify their total size and format.")]
        public IEnumerable<HttpPostedFileBase> Images { get; set; }
    }

    public class EditReviewViewModel : CriteriaBaseModel
    {
        public int Id { get; set; }

        public string AuthorId { get; set; }

        [MaxLength(3000, ErrorMessage = "We'll happily accept 3000 symbols and below.")]
        [Display(Name = "Review")]
        public string Content { get; set; }

        public bool IsBlogger { get; set; }

        [Display(Name = "Blog article links")]
        [UrisValid(ErrorMessage = "We couldn't process the provided links.")]
        public string ArticleLinks { get; set; }

        public DetailedBeachViewModel BeachHead { get; set; }
        
        public int BeachReviewsCount { get; set; }
    }
}