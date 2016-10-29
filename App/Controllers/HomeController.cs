namespace App.Controllers
{
    using AutoMapper;
    using BeachRankings.Data.UnitOfWork;
    using BeachRankings.Models;
    using BeachRankings.Models.Interfaces;
    using BeachRankings.App.Models.ViewModels;
    using BeachRankings.Services.Search;
    using BeachRankings.Services.Search.Models;
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

        public PartialViewResult Autocomplete(string prefix)
        {
            if (prefix.Length <= 1)
            {
                return null;
            }

            var beaches = Mapper.Map<IEnumerable<ISearchable>, IEnumerable<AutocompleteBeachViewModel>>
               (this.Data.Beaches.GetSearchResultsByKeyStroke(prefix));
            var primaryDivisions = Mapper.Map<IEnumerable<ISearchable>, IEnumerable<AutocompletePrimaryViewModel>>
                (this.Data.PrimaryDivisions.GetSearchResultsByKeyStroke(prefix));
            var secondaryDivisions = Mapper.Map<IEnumerable<ISearchable>, IEnumerable<AutocompleteSecondaryViewModel>>
                (this.Data.SecondaryDivisions.GetSearchResultsByKeyStroke(prefix));
            var tertiaryDivisions = Mapper.Map<IEnumerable<ISearchable>, IEnumerable<AutocompleteTertiaryViewModel>>
                (this.Data.TertiaryDivisions.GetSearchResultsByKeyStroke(prefix));
            var quaternaryDivisions = Mapper.Map<IEnumerable<ISearchable>, IEnumerable<AutocompleteQuaternaryViewModel>>
                (this.Data.QuaternaryDivisions.GetSearchResultsByKeyStroke(prefix));
            var countries = Mapper.Map<IEnumerable<ISearchable>, IEnumerable<AutocompleteCountryViewModel>>
                (this.Data.Countries.GetSearchResultsByKeyStroke(prefix));
            var waterBodies = Mapper.Map<IEnumerable<ISearchable>, IEnumerable<AutocompleteWaterBodyViewModel>>
                (this.Data.WaterBodies.GetSearchResultsByKeyStroke(prefix));

            var model = new SearchViewModel()
            {
                Beaches = beaches,
                PrimaryDivisions = primaryDivisions,
                SecondaryDivisions = secondaryDivisions,
                TertiaryDivisions = tertiaryDivisions,
                QuaternaryDivisions = quaternaryDivisions,
                Countries = countries,
                WaterBodies = waterBodies
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