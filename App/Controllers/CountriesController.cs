namespace App.Controllers
{
    using AutoMapper;
    using BeachRankings.Data.UnitOfWork;
    using BeachRankings.Models;
    using BeachRankings.App.Models.ViewModels;
    using System.Web.Mvc;

    public class CountriesController : BaseController
    {
        public CountriesController(IBeachRankingsData data)
            : base(data)
        {
        }

        public ActionResult Beaches(int id)
        {
            var country = this.Data.Countries.Find(id);
            var model = Mapper.Map<Country, CountryBeachesViewModel>(country);

            return View(model);
        }
    }
}