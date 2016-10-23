namespace App.Controllers
{
    using AutoMapper;
    using BeachRankings.Data.UnitOfWork;
    using BeachRankings.Models;
    using BeachRankings.App.Models.ViewModels;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;

    public class HomeController : BaseController
    {
        public HomeController(IBeachRankingsData data)
            : base(data)
        {
        }

        public ActionResult Index()
        {
            return this.View();
        }

        public async Task<PartialViewResult> Autocomplete(string prefix)
        {
            if (prefix.Length <= 1)
            {
                return null;
            }

            var waterBodies = await this.Data.WaterBodies.All().Where(wb => wb.Beaches.Count > 0 && wb.Name.StartsWith(prefix)).ToListAsync();
            var countries = await this.Data.Countries.All().Where(c => c.Beaches.Count > 0 && c.Name.StartsWith(prefix)).ToListAsync();
            var regions = await this.Data.Regions.All().Where(l => l.Beaches.Count > 0 && l.Name.StartsWith(prefix)).ToListAsync();
            var areas = await this.Data.Areas.All().Where(a => a.Beaches.Count > 0 && a.Name.StartsWith(prefix)).ToListAsync();
            var beaches = await this.Data.Beaches.All().Where(b => b.Name.StartsWith(prefix)).ToListAsync();
            var beachesModel = Mapper.Map<IEnumerable<Beach>, IEnumerable<AutocompleteBeachViewModel>>(beaches);
            var areasModel = Mapper.Map<IEnumerable<Area>, IEnumerable<AutocompleteAreaViewModel>>(areas);
            var regionsModel = Mapper.Map<IEnumerable<Region>, IEnumerable<AutocompleteRegionViewModel>>(regions);
            var countriesModel = Mapper.Map<IEnumerable<Country>, IEnumerable<AutocompleteCountryViewModel>>(countries);
            var waterBodiesModel = Mapper.Map<IEnumerable<WaterBody>, IEnumerable<AutocompleteWaterBodyViewModel>>(waterBodies);
            var model = new AutocompleteMainViewModel()
            {
                Beaches = beachesModel,
                WaterBodies = waterBodiesModel,
                Regions = regionsModel,
                Countries = countriesModel,
                Areas = areasModel
            };

            return PartialView("_Autocomplete", model);
        }

        //public ActionResult Top(FormCollection form)
        //{
        //    var query = form["main-search-field"];

        //    if (string.IsNullOrEmpty(query))
        //    {
        //        return null;
        //    }

        //    var beachIds = this.Data.Beaches.GetBeachIdsByQuery(query);
        //    var beaches = this.Data.Beaches.All().Where(b => beachIds.Contains(b.Id));
        //    var model = Mapper.Map<IEnumerable<Beach>, IEnumerable<ConciseBeachViewModel>>(beaches);

        //    return PartialView("_Top", model);
        //}
    }
}