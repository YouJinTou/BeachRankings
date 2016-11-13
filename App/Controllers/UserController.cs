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
        public PartialViewResult Statistics()
        {
            var reviews = this.Data.Reviews.All().Where(r => r.AuthorId == this.UserProfile.Id);
            var model = Mapper.Map<IEnumerable<Review>, IEnumerable<TableRowViewModel>>(reviews);

            return this.PartialView(model);
        }

        [Authorize]
        public PartialViewResult Images(int page = 0, int pageSize = 10)
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

            model = model.Skip(page * pageSize).Take(pageSize).ToList();

            return this.PartialView(model);
        }
    }
}