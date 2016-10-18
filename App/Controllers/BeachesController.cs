namespace App.Controllers
{
    using AutoMapper;
    using BeachRankings.Data.UnitOfWork;
    using BeachRankings.Models;
    using BeachRankings.App.Models.BindingModels;
    using BeachRankings.App.Models.ViewModels;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Threading.Tasks;
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

            return this.View(model);
        }

        [Authorize]
        [HttpGet]
        public ActionResult Add()
        {
            var waterLocations = this.Data.Locations.All().Where(l => l.LocationType == LocationType.WaterBody);
            var waterBodies = Mapper.Map<IEnumerable<Location>, IEnumerable<WaterBodyViewModel>>(waterLocations);

            this.ViewData["waterBodies"] = waterBodies;

            return this.View();
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(AddBeachBindingModel bindingModel)
        {
            bool beachNameUnique = this.Data.Beaches.All().Any(b => b.Name.ToLower() == bindingModel.Name.ToLower());

            if (beachNameUnique)
            {
                return this.Json(new { data = "A beach with this name already exists." });
            }

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
            this.Data.Beaches.SaveChanges();


            this.Data.Beaches.AddBeachToIndex(beach);

            return this.Json(new { data = Url.Action("Rate", "Reviews", new { id = beach.Id }) });
        }

        public async Task<JsonResult> Names(string term)
        {
            var beachNames = await this.Data.Beaches.All()
                .Where(l => l.Name.StartsWith(term))
                .Select(l => l.Name)
                .ToListAsync();

            return this.Json(beachNames, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> Locations(string term)
        {
            var locations = await this.Data.Locations.All()
                .Where(l => l.LocationType == LocationType.Land && l.Name.StartsWith(term))
                .Select(l => l.Name)
                .ToListAsync();

            return this.Json(locations, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> WaterBodies(string term)
        {
            var waterBodies = await this.Data.Locations.All()
                .Where(l => l.LocationType == LocationType.WaterBody && l.Name.StartsWith(term))
                .Select(l => l.Name)
                .ToListAsync();

            return this.Json(waterBodies, JsonRequestBehavior.AllowGet);
        }
    }
}