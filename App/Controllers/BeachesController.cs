namespace App.Controllers
{
    using AutoMapper;
    using BeachRankings.Data.UnitOfWork;
    using BeachRankings.Models;
    using BeachRankings.App.Models.BindingModels;
    using BeachRankings.App.Models.ViewModels;
    using System.Collections.Generic;
    using System.Data.Entity;
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
            var countries = this.Data.Countries.All().Select(c => c.Name).ToList();

            this.ViewData["ddlCountries"] = countries;

            return this.View();
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(AddBeachBindingModel bindingModel)
        {
            bool beachNameUnique = !this.Data.Beaches.All().Any(b => b.Name.ToLower() == bindingModel.Name.ToLower());

            if (!beachNameUnique)
            {
                this.Response.Clear();
                this.Response.StatusDescription = "A beach with this name already exists.";

                return new HttpStatusCodeResult(412, "A beach with this name already exists.");
            }

            if (!this.ModelState.IsValid)
            {
                return new HttpStatusCodeResult(412, "Invalid data. Please review your input and try again.");
            }

            var location = this.Data.Locations.All().FirstOrDefault(l => l.Name.ToLower() == bindingModel.LocationName.ToLower());
            var country = this.Data.Countries.All().FirstOrDefault(c => c.Name.ToLower().Contains(bindingModel.CountryName));

            if (location == null) // The location the user has typed in doesn't exist in the database. Store it.
            {
                location = new Location()
                {
                    Name = bindingModel.LocationName,
                    CountryId = (country == null) ? (int?)null : country.Id
                };

                this.Data.Locations.Add(location);
                this.Data.Locations.SaveChanges();

                this.Data.Locations.AddLocationToIndex(location);
            }

            var beach = Mapper.Map<AddBeachBindingModel, Beach>(bindingModel);
            beach.LocationId = location.Id;
            beach.CountryId = (country == null) ? (int?)null : country.Id;

            this.Data.Beaches.Add(beach);
            this.Data.Beaches.SaveChanges();

            this.Data.Beaches.AddBeachToIndex(beach);

            return this.Json(new { redirectUrl = Url.Action("Post", "Reviews", new { id = beach.Id }) });
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
                .Where(l => l.Name.StartsWith(term))
                .Select(l => l.Name)
                .ToListAsync();

            return this.Json(locations, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> WaterBodies(string term)
        {
            var waterBodies = await this.Data.WaterBodies.All().Where(wb => wb.Name.StartsWith(term)).ToListAsync();
            var model = Mapper.Map<IEnumerable<WaterBody>, IEnumerable<AddBeachWaterBodyViewModel>>(waterBodies);

            return this.Json(model, JsonRequestBehavior.AllowGet);
        }
    }
}