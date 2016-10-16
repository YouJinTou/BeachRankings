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

        public ActionResult Autocomplete(string prefix)
        {
            var beachIds = this.Data.Beaches.GetTermsByKeystroke(prefix);

            if (beachIds == null)
            {
                return null;
            }

            var beaches = this.Data.Beaches.All().Where(b => beachIds.Contains(b.Id));
            var model = Mapper.Map<IEnumerable<Beach>, IEnumerable<AutocompleteViewModel>>(beaches);

            return PartialView("_Autocomplete", model);
        }

        public ActionResult Top(FormCollection form)
        {
            var query = form["main-search-field"];

            if (string.IsNullOrEmpty(query))
            {
                return View("Index", "Home");
            }

            var beachIds = this.Data.Beaches.GetBeachIdsByQuery(query);
            var beaches = this.Data.Beaches.All().Where(b => beachIds.Contains(b.Id));
            var model = Mapper.Map<IEnumerable<Beach>, IEnumerable<ConciseBeachViewModel>>(beaches);
            
            return PartialView("_Top", model);
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
                location = new Location(bindingModel.LocationName);

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