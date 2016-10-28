namespace App.Controllers
{
    using AutoMapper;
    using BeachRankings.Data.UnitOfWork;
    using BeachRankings.Models;
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

        public async Task<PartialViewResult> Autocomplete(string prefix)
        {
            if (prefix.Length <= 1)
            {
                return null;
            }

            try
            {
                var primaryDivisions = Mapper.Map<IEnumerable<PlaceSearchResultModel>, IEnumerable<AutocompleteViewModel>>
                    (this.Data.PrimaryDivisions.GetSearchResultsByKeyStroke(prefix));
                var secondaryDivisions = Mapper.Map<IEnumerable<PlaceSearchResultModel>, IEnumerable<AutocompleteViewModel>>
                    (this.Data.SecondaryDivisions.GetSearchResultsByKeyStroke(prefix));
                var tertiaryDivisions = Mapper.Map<IEnumerable<PlaceSearchResultModel>, IEnumerable<AutocompleteViewModel>>
                    (this.Data.TertiaryDivisions.GetSearchResultsByKeyStroke(prefix));
                var quaternaryDivisions = Mapper.Map<IEnumerable<PlaceSearchResultModel>, IEnumerable<AutocompleteViewModel>>
                    (this.Data.QuaternaryDivisions.GetSearchResultsByKeyStroke(prefix));
                var countries = Mapper.Map<IEnumerable<PlaceSearchResultModel>, IEnumerable<AutocompleteViewModel>>
                    (this.Data.WaterBodies.GetSearchResultsByKeyStroke(prefix));
                var waterBodies = Mapper.Map<IEnumerable<PlaceSearchResultModel>, IEnumerable<AutocompleteViewModel>>
                    (this.Data.WaterBodies.GetSearchResultsByKeyStroke(prefix));
                var beaches = Mapper.Map<IEnumerable<BeachSearchResultModel>, IEnumerable<AutocompleteBeachViewModel>>
                    (this.Data.Beaches.GetSearchResultsByKeyStroke(prefix));
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
            }
            catch (System.Exception e)
            {
            }


            return PartialView("_Autocomplete");
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