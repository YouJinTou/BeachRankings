namespace BeachRankings.App.Models.ViewModels
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class WatchlistsTableViewModel
    {
        public string AuthorId { get; set; }

        public ContributorVerticalViewModel Contributor { get; set; }

        public ICollection<UserWatchlistViewModel> Watchlists { get; set; }
    }

    public class UserWatchlistViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int BeachesCount { get; set; }

        public string OwnerId { get; set; }
    }

    public class ConciseWatchlistViewModel
    {
        public int BeachId { get; set; }

        public IEnumerable<BeachWatchlistViewModel> Watchlists { get; set; }
    }

    public class BeachWatchlistViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public bool BeachPresentInWatchlist { get; set; }

        public int BeachId { get; set; }
    }

    public class AddEditWatchlistViewModel
    {
        public int Id { get; set; }

        public int BeachId { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; }
    }
}