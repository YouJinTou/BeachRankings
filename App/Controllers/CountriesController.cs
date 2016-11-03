namespace App.Controllers
{
    using AutoMapper;
    using BeachRankings.App.Models.ViewModels;
    using BeachRankings.Data.UnitOfWork;
    using BeachRankings.Models;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    public class CountriesController : BaseController
    {
        public CountriesController(IBeachRankingsData data)
            : base(data)
        {
        }
        
        public PartialViewResult Beaches(int id)
        {
            var beaches = this.Data.Countries.Find(id).Beaches;
            var model = Mapper.Map<IEnumerable<Beach>, IEnumerable<BeachTableRowViewModel>>(beaches);

            return this.PartialView(model);
        }

        public JsonResult PrimaryDivisions(int id)
        {
            var primaryDivisions = this.Data.PrimaryDivisions.All().Where(pd => pd.CountryId == id).Select(pd => new SelectListItem()
            {
                Text = pd.Name,
                Value = pd.Id.ToString()
            });

            return this.Json(primaryDivisions, JsonRequestBehavior.AllowGet);
        }
    }
}