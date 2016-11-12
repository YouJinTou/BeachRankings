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

        public ActionResult Beaches(int id, int page = 0, int pageSize = 10)
        {
            var waterBody = this.Data.WaterBodies.Find(id);
            var model = Mapper.Map<WaterBody, LocationBeachesViewModel>(waterBody);
            model.Beaches = model.Beaches.Skip(page * pageSize).Take(pageSize);

            return this.View("LocationBeaches", model);
        }

        public PartialViewResult Statistics(int id)
        {
            var beaches = this.Data.WaterBodies.Find(id).Beaches.Where(b => b.TotalScore != null);
            var model = Mapper.Map<IEnumerable<Beach>, IEnumerable<TableRowViewModel>>(beaches);

            return this.PartialView(model);
        }
    }
}