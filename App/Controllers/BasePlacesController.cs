namespace App.Controllers
{
    using BeachRankings.App.Models.ViewModels;
    using BeachRankings.Data.UnitOfWork;
    using BeachRankings.Services.Aggregation;
    using System;
    using System.Linq;

    public class BasePlacesController : BaseController
    {
        protected IDataAggregationService aggregationService;

        protected BasePlacesController(IBeachRankingsData data, IDataAggregationService aggregationService)
            : base(data)
        {
            this.aggregationService = aggregationService;
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