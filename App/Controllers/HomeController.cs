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
            var primaryDivisions = await this.Data.PrimaryDivisions.All().Where(l => l.Beaches.Count > 0 && l.Name.StartsWith(prefix)).ToListAsync();
            var secondaryDivisions = await this.Data.SecondaryDivisions.All().Where(a => a.Beaches.Count > 0 && a.Name.StartsWith(prefix)).ToListAsync();
            var beaches = await this.Data.Beaches.All().Where(b => b.Name.StartsWith(prefix)).ToListAsync();
            var beachesModel = Mapper.Map<IEnumerable<Beach>, IEnumerable<AutocompleteBeachViewModel>>(beaches);
            var secondaryDivisionsModel = Mapper.Map<IEnumerable<SecondaryDivision>, IEnumerable<AutocompleteViewModel>>(secondaryDivisions);
            var primaryDivisionsModel = Mapper.Map<IEnumerable<PrimaryDivision>, IEnumerable<AutocompleteViewModel>>(primaryDivisions);
            var countriesModel = Mapper.Map<IEnumerable<Country>, IEnumerable<AutocompleteViewModel>>(countries);
            var waterBodiesModel = Mapper.Map<IEnumerable<WaterBody>, IEnumerable<AutocompleteViewModel>>(waterBodies);
            var model = new SearchViewModel()
            {
                Beaches = beachesModel,
                WaterBodies = waterBodiesModel,
                PrimaryDivisions = primaryDivisionsModel,
                Countries = countriesModel,
                SecondaryDivisions = secondaryDivisionsModel
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