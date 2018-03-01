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
            var topUsers = this.Data.Users.All().OrderByDescending(u => u.Points).ToList();
            var model = Mapper.Map<IEnumerable<User>, IEnumerable<ContributorRowViewModel>>(topUsers).ToList();

            return this.View(model);
        }
    }
}