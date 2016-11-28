namespace BeachRankings.App.Models.ViewModels
{
    using BeachRankings.App.CustomAttributes;
    using System.Collections.Generic;
    using System.Web;

    public class DashboardImageViewModel
    {
        public string BeachName { get; set; }

        public IEnumerable<BeachImageThumbnailViewModel> Paths { get; set; }
    }

    public class TableUserReviewsViewModel
    {
        public string AuthorName { get; set; }

        public IEnumerable<ReviewRowViewModel> Reviews { get; set; }
    }

    public class ChangeAvatarViewModel
    {
        public string AvatarPath { get; set; }

        [UserAvatarValid]
        public HttpPostedFileBase Avatar { get; set; }
    }

    public class ContributorViewModel
    {
        public string Id { get; set; }

        public int Rank { get; set; }

        public int Level { get; set; }

        public string UserName { get; set; }

        public string AvatarPath { get; set; }

        public int ReviewsCount { get; set; }

        public int CountriesVisited { get; set; }

        public int ThanksReceived { get; set; }

        public bool IsBlogger { get; set; }

        public string BlogUrl { get; set; }
    }
}