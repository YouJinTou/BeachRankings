namespace App.Controllers
{
    using AutoMapper;
    using BeachRankings.Data.UnitOfWork;
    using BeachRankings.Models;
    using Models.BindingModels;
    using Models.ViewModels;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    public class BeachesController : BaseController
    {
        public BeachesController(IBeachRankingsData data)
            : base(data)
        {
        }        

        public ActionResult Details(int id)
        {
            var beach = this.Data.Beaches.All().FirstOrDefault(b => b.Id == id);
            var model = Mapper.Map<Beach, DetailedBeachViewModel>(beach);

            model.Reviews.OrderByDescending(r => r.PostedOn);

            return View(model);
        }

        [Authorize]
        [HttpGet]
        public ActionResult Add()
        {
            var waterLocations = this.Data.Locations.All().Where(l => l.LocationType == LocationType.WaterBody);
            var waterBodies = Mapper.Map<IEnumerable<Location>, IEnumerable<WaterBodyViewModel>>(waterLocations);

            this.ViewData["waterBodies"] = waterBodies; 

            return View();
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(AddBeachBindingModel bindingModel)
        {
            var location = this.Data.Locations.All().FirstOrDefault(
                l => l.Name.ToLower() == bindingModel.LocationName.ToLower());

            if (location == null)
            {
                location = new Location(bindingModel.LocationName, LocationType.Land);

                this.Data.Locations.Add(location);
                this.Data.Locations.SaveChanges();

                this.Data.Locations.AddLocationToIndex(location);
            }

            var beach = Mapper.Map<AddBeachBindingModel, Beach>(bindingModel);
            beach.LocationId = location.Id;

            this.Data.Beaches.Add(beach);
            this.Data.Beaches.SaveChanges(); // Try catch for duplicates

            this.Data.Beaches.AddBeachToIndex(beach);

            return Json(new { redirectUrl = Url.Action("Rate", "Reviews", new { id = beach.Id }) });
        }
    }
}