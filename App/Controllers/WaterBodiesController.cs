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

        public ActionResult Beaches(int id)
        {
            var waterBody = this.Data.WaterBodies.Find(id);
            var model = Mapper.Map<WaterBody, LocationBeachesViewModel>(waterBody);

            return this.View("LocationBeaches", model);
        }

        public PartialViewResult BeachesPartial(int id)
        {
            var beaches = this.Data.WaterBodies.Find(id).Beaches.Where(b => b.TotalScore != null);
            var model = Mapper.Map<IEnumerable<Beach>, IEnumerable<BeachTableRowViewModel>>(beaches);

            return this.PartialView(model);
        }
    }
}