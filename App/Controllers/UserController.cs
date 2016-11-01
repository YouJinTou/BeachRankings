namespace App.Controllers
{
    using AutoMapper;
    using BeachRankings.Data.UnitOfWork;
    using BeachRankings.Models;
    using BeachRankings.App.Models.ViewModels;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    public class UserController : BaseController
    {
        public UserController(IBeachRankingsData data)
            : base(data)
        {
        }

        [Authorize]
        public ActionResult Dashboard()
        {
            var reviews = this.Data.Reviews.All().Where(r => r.AuthorId == this.UserProfile.Id);
            var model = Mapper.Map<IEnumerable<Review>, IEnumerable<DashboardReviewViewModel>>(reviews);

            return this.View(model);
        }

        [Authorize]
        public ActionResult Images()
        {
            var imageGroups = this.Data.BeachImages.All().Where(i => i.AuthorId == this.UserProfile.Id).GroupBy(i => i.Beach.Name);
            var model = new List<DashboardImageViewModel>();

            foreach (var group in imageGroups)
            {
                model.Add(new DashboardImageViewModel()
                {
                    BeachName = group.Key,
                    Paths = Mapper.Map<IEnumerable<BeachImage>, IEnumerable<BeachImageThumbnailViewModel>>(group.ToList())
                });
            }

            return this.View(model);
        }
    }
}