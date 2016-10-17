namespace App.Controllers
{
    using AutoMapper;
    using BeachRankings.Data.UnitOfWork;
    using BeachRankings.Models;
    using Models.ViewModels;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    public class HomeController : BaseController
    {
        public HomeController(IBeachRankingsData data)
            : base(data)
        {
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Autocomplete(string prefix)
        {
            //var beachIds = this.Data.Beaches.GetTermsByKeystroke(prefix);

            //if (beachIds == null)
            //{
            //    return null;
            //}

            //var beaches = this.Data.Beaches.All().Where(b => beachIds.Contains(b.Id));
            var beaches = this.Data.Beaches.All().Where(b => b.Name.StartsWith(prefix));
            var locations = this.Data.Locations.All()
                .Where(l => l.Name.StartsWith(prefix) ||
                (l.Name.StartsWith(prefix) && l.LocationType == LocationType.WaterBody));
            var beachesModel = Mapper.Map<IEnumerable<Beach>, IEnumerable<AutocompleteBeachViewModel>>(beaches);
            var locationsModel = Mapper.Map<IEnumerable<Location>, IEnumerable<AutocompleteLocationViewModel>>(locations);
            var model = new AutocompleteMainViewModel()
            {
                Beaches = beachesModel,
                Locations = locationsModel
            };

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
    }
}