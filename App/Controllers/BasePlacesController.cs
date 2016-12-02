namespace App.Controllers
{
    using BeachRankings.App.Models.ViewModels;
    using BeachRankings.Data.UnitOfWork;
    using System;
    using System.Linq;

    public class BasePlacesController : BaseController
    {
        protected BasePlacesController(IBeachRankingsData data)
            : base(data)
        {
        }

        protected Func<ConciseBeachViewModel, bool> UserHasRated
        {
            get
            {
                return (b => (this.User.Identity.IsAuthenticated ? this.UserProfile.Reviews.Any(r => r.BeachId == b.Id) : false));
            }
        }        
    }
}