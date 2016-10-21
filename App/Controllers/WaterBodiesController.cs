namespace App.Controllers
{
    using AutoMapper;
    using BeachRankings.Data.UnitOfWork;
    using BeachRankings.Models;
    using BeachRankings.App.Models.ViewModels;
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
            var model = Mapper.Map<WaterBody, WaterBodyBeachesViewModel>(waterBody);

            return View(model);
        }
    }
}