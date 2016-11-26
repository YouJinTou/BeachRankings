namespace App.Controllers
{
    using AutoMapper;
    using BeachRankings.App.CustomAttributes;
    using BeachRankings.App.Models.ViewModels;
    using BeachRankings.Data.UnitOfWork;
    using BeachRankings.Models;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
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

        public async Task<JsonResult> BeachNames(int id, string term)
        {
            var beachNames = await this.Data.Beaches.All()
                .Where(b => b.CountryId == id && b.Name.StartsWith(term))
                .Select(b => b.Name)
                .ToListAsync();

            return this.Json(beachNames, JsonRequestBehavior.AllowGet);
        }

        public JsonResult AdminBeaches(int id)
        {
            var beaches = this.Data.Beaches.All().Where(b => b.CountryId == id).Select(b => new SelectListItem
            {
                Text = b.Name,
                Value = b.Id.ToString()
            });

            return this.Json(beaches, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [RestructureAuthorize]
        public ActionResult Add(RestructureViewModel bindingModel)
        {
            var country = new Country() { Name = bindingModel.Country };

            try
            {
                this.Data.Countries.Add(country);
                this.Data.Countries.SaveChanges();
                this.Data.Countries.AddUpdateIndexEntry(country);
            }
            catch (Exception)
            {
                this.TempData["ValidationError"] = "The name of the country is either a duplicate or is missing.";

                this.Data.Countries.Detach(country);
            }

            return this.RedirectToAction("Restructure", "Admin");
        }

        [HttpPost]
        [RestructureAuthorize]
        public ActionResult Edit(RestructureViewModel bindingModel)
        {
            var country = this.Data.Countries.Find(bindingModel.CountryId);

            try
            {
                country.Name = bindingModel.Country;

                this.Data.Countries.SaveChanges();
                this.Data.Countries.AddUpdateIndexEntry(country);
            }
            catch (Exception)
            {
                this.TempData["ValidationError"] = "The name of the country is either a duplicate or is missing.";

                this.Data.Countries.Detach(country);
            }

            return this.RedirectToAction("Restructure", "Admin");
        }

        [HttpPost]
        [RestructureAuthorize]
        public ActionResult Delete(RestructureViewModel bindingModel)
        {
            Country country = null;

            try
            {
                country = this.Data.Countries.All()
                    .Include(c => c.Beaches)
                    .Include(c => c.PrimaryDivisions)
                    .Include(c => c.SecondaryDivisions)
                    .Include(c => c.TertiaryDivisions)
                    .Include(c => c.QuaternaryDivisions)
                    .FirstOrDefault(c => c.Id == bindingModel.CountryId);
                var hasChildren = (country.Beaches.Count > 0 ||
                    country.PrimaryDivisions.Count > 0 ||
                    country.SecondaryDivisions.Count > 0 ||
                    country.TertiaryDivisions.Count > 0 ||
                    country.QuaternaryDivisions.Count > 0);

                if (hasChildren)
                {
                    throw new InvalidOperationException();
                }

                this.Data.Countries.Remove(country);
                this.Data.Countries.SaveChanges();
                this.Data.Countries.DeleteIndexEntry(country);
            }
            catch (Exception)
            {
                this.TempData["ValidationError"] = 
                    "Could not delete country. All of its children must be deleted first and beaches moved to another division.";

                if (country != null)
                {
                    this.Data.Countries.Detach(country);
                }
            }

            return this.RedirectToAction("Restructure", "Admin");
        }
    }
}