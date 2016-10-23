namespace App.Controllers
{
    using AutoMapper;
    using BeachRankings.App.Models.ViewModels;
    using BeachRankings.Data.UnitOfWork;
    using BeachRankings.Models;
    using System.Linq;
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

        public JsonResult Regions(int id)
        {
            var regions = this.Data.Regions.All().Where(r => r.CountryId == id).Select(r => new SelectListItem()
            {
                Text = r.Name,
                Value = r.Id.ToString()
            });

            return this.Json(regions, JsonRequestBehavior.AllowGet);
        }
    }
}