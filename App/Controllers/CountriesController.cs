namespace App.Controllers
{
    using AutoMapper;
    using BeachRankings.App.Models.ViewModels;
    using BeachRankings.Data.UnitOfWork;
    using BeachRankings.Models;
    using BeachRankings.Models.Interfaces;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Web.Mvc;

    public class CountriesController : BaseController
    {
        public CountriesController(IBeachRankingsData data)
            : base(data)
        {
        }

        public ActionResult Beaches(int id, int page = 0, int pageSize = 5)
        {
            var country = this.Data.Countries.Find(id);
            var model = Mapper.Map<IPlaceSearchable, LocationBeachesViewModel>(country);
            model.Beaches = model.Beaches.Skip(page * pageSize).Take(pageSize);

            return this.View("LocationBeaches", model);
        }
        
        public PartialViewResult BeachesPartial(int id)
        {
            var beaches = this.Data.Countries.All()
                .Include(c => c.PrimaryDivisions.Select(pd => pd.WaterBody))
                .Include(c => c.SecondaryDivisions)
                .Include(c => c.TertiaryDivisions)
                .Include(c => c.QuaternaryDivisions)
                .FirstOrDefault(c => c.Id == id)
                .Beaches
                .Where(b => b.TotalScore != null);
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