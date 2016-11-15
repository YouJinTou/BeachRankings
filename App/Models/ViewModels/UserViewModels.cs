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

        public IEnumerable<TableRowViewModel> Reviews { get; set; }
    }

    public class ChangeAvatarViewModel
    {
        public string AvatarPath { get; set; }

        [UserAvatarValid]
        public HttpPostedFileBase Avatar { get; set; }
    }
}