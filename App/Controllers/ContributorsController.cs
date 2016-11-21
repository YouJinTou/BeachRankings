namespace App.Controllers
{
    using AutoMapper;
    using BeachRankings.App.Models.ViewModels;
    using BeachRankings.Data.UnitOfWork;
    using BeachRankings.Models;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    public class ContributorsController : BaseController
    {
        public ContributorsController(IBeachRankingsData data)
            : base(data)
        {
        }

        public ActionResult Top()
        {
            var topUsers = this.Data.Users.All().OrderByDescending(u => u.Reviews.Count).Take(20);
            var model = Mapper.Map<IEnumerable<User>, IEnumerable<ContributorViewModel>>(topUsers);

            return this.View(model);
        }
    }
}