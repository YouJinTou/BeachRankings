namespace App.Controllers
{
    using AutoMapper;
    using BeachRankings.Data.UnitOfWork;
    using BeachRankings.Models;
    using BeachRankings.App.Models.ViewModels;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    public class WaterBodiesController : BaseController
    {
        public WaterBodiesController(IBeachRankingsData data)
            : base(data)
        {
        }

        public PartialViewResult Beaches(int id)
        {
            var beaches = this.Data.WaterBodies.Find(id).Beaches.Where(b => b.Reviews.Count > 0);
            var model = Mapper.Map<IEnumerable<Beach>, IEnumerable<BeachTableRowViewModel>>(beaches);

            return this.PartialView(model);
        }
    }
}