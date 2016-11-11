namespace BeachRankings.App.Models.ViewModels
{
    using System.Collections.Generic;
    
    public class DashboardImageViewModel
    {
        public string BeachName { get; set; }

        public IEnumerable<BeachImageThumbnailViewModel> Paths { get; set; }
    }
}