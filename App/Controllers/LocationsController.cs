namespace App.Controllers
{
    using AutoMapper;
    using BeachRankings.Data.UnitOfWork;
    using BeachRankings.Models;
    using BeachRankings.App.Models.ViewModels;
    using System.Web.Mvc;

    public class LocationsController : BaseController
    {
        public LocationsController(IBeachRankingsData data)
            : base(data)
        {
        }

        public ActionResult Beaches(int id)
        {
            var location = this.Data.Locations.Find(id);
            var model = Mapper.Map<Location, LocationBeachesViewModel>(location);

            return View(model);
        }
    }
}