namespace App.Controllers
{
    using AutoMapper;
    using BeachRankings.App.CustomAttributes;
    using BeachRankings.App.Models.ViewModels;
    using BeachRankings.Data.UnitOfWork;
    using BeachRankings.Models;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Web.Mvc;

    public class CountriesController : BaseLocationsController
    {
        public CountriesController(IBeachRankingsData data)
            : base(data)
        {
        }

        public ActionResult Beaches(int id, int page = 0, int pageSize = 10)
        {
            var country = this.Data.Countries.Find(id);
            var model = Mapper.Map<Country, LocationBeachesViewModel>(country);
            model.Beaches = model.Beaches.Skip(page * pageSize).Take(pageSize);

            model.Beaches.Select(b => { b.UserHasRated = base.UserHasRated(b); return b; }).ToList();

            return this.View("_LocationBeaches", model);
        }
        
        public PartialViewResult Statistics(int id)
        {
            var beaches = this.Data.Countries.All()
                .Include(c => c.PrimaryDivisions.Select(pd => pd.WaterBody))
                .Include(c => c.SecondaryDivisions)
                .Include(c => c.TertiaryDivisions)
                .Include(c => c.QuaternaryDivisions)
                .FirstOrDefault(c => c.Id == id)
                .Beaches
                .Where(b => b.TotalScore != null);
            var model = Mapper.Map<IEnumerable<Beach>, IEnumerable<TableRowViewModel>>(beaches);

            return this.PartialView("_StatisticsPartial", model);
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

        [HttpPost]
        [RestructureAuthorize]
        public ActionResult Add(RestructureViewModel bindingModel)
        {
            var country = new Country() { Name = bindingModel.Country };

            this.Data.Countries.Add(country);
            this.Data.Countries.SaveChanges();
            this.Data.Countries.AddUpdateIndexEntry(country);

            return this.RedirectToAction("Restructure", "Admin");
        }

        [HttpPost]
        [RestructureAuthorize]
        public ActionResult Edit(RestructureViewModel bindingModel)
        {
            var country = this.Data.Countries.Find(bindingModel.CountryId);
            country.Name = bindingModel.Country;

            this.Data.Countries.SaveChanges();
            this.Data.Countries.AddUpdateIndexEntry(country);

            return this.RedirectToAction("Restructure", "Admin");
        }
    }
}