namespace App.Controllers
{
    using AutoMapper;
    using BeachRankings.Data.UnitOfWork;
    using BeachRankings.Models.Interfaces;
    using BeachRankings.App.Models.ViewModels;
    using System.Collections.Generic;
    using System.Web.Mvc;

    [SessionState(System.Web.SessionState.SessionStateBehavior.ReadOnly)]
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

            return this.PartialView("_Autocomplete", model);
        }
    }
}