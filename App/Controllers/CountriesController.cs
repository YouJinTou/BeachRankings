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

    public class CountriesController : BasePlacesController
    {
        public CountriesController(IBeachRankingsData data)
            : base(data)
        {
        }

        public ActionResult Beaches(int id, int page = 0, int pageSize = 10)
        {
            var country = this.Data.Countries.Find(id);
            var model = Mapper.Map<Country, PlaceBeachesViewModel>(country);
            model.Beaches = model.Beaches.OrderByDescending(b => b.TotalScore).Skip(page * pageSize).Take(pageSize);

            model.Beaches.Select(b => { b.UserHasRated = base.UserHasRated(b); return b; }).ToList();

            return this.View("_PlaceBeaches", model);
        }

        public PartialViewResult Statistics(int id)
        {
            var country = this.Data.Countries.All()
                .Include(c => c.PrimaryDivisions.Select(pd => pd.WaterBody))
                .Include(c => c.SecondaryDivisions)
                .Include(c => c.TertiaryDivisions)
                .Include(c => c.QuaternaryDivisions)
                .Include(c => c.Beaches)
                .FirstOrDefault(c => c.Id == id);
            var beaches = country.Beaches.Where(b => b.TotalScore != null).OrderByDescending(b => b.TotalScore);
            var model = new StatisticsViewModel()
            {
                Id = id,
                Controller = "Countries",
                Name = country.Name,
                Rows = Mapper.Map<IEnumerable<Beach>, IEnumerable<BeachRowViewModel>>(beaches)
            };

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
            var country = new Country()
            {
                Name = bindingModel.Country,
                ContinentId = bindingModel.ContinentId,
                WaterBodyId = bindingModel.WaterBodyId
            };

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
            Country country = null;

            try
            {
                country = this.Data.Countries.All()
                    .Include(c => c.Beaches)
                    .Include(c => c.PrimaryDivisions)
                    .Include(c => c.SecondaryDivisions)
                    .FirstOrDefault(c => c.Id == bindingModel.CountryId);
                country.Name = bindingModel.Country;
                var waterBodyIdIsNew = (country.WaterBodyId != bindingModel.WaterBodyId);
                var continentIsNew = (country.ContinentId != bindingModel.ContinentId);

                if (waterBodyIdIsNew)
                {
                    if (!this.CanEditWaterBody(country))
                    {
                        throw new InvalidOperationException("Cannnot assign a water body; the water body is assigned at a different level.");
                    }

                    var oldWaterBody = this.Data.WaterBodies.Find(country.WaterBodyId);
                    var newWaterBody = this.Data.WaterBodies.Find(bindingModel.WaterBodyId);

                    country.WaterBodyId = bindingModel.WaterBodyId;

                    this.AssignChildrenWaterBodyIds(country);

                    this.Data.WaterBodies.AddUpdateIndexEntry(oldWaterBody);
                    this.Data.WaterBodies.AddUpdateIndexEntry(newWaterBody);
                }

                if (continentIsNew)
                {
                    var oldContinent = this.Data.Continents.Find(country.ContinentId);
                    var newContinent = this.Data.Continents.Find(bindingModel.ContinentId);

                    country.ContinentId = bindingModel.ContinentId;

                    this.AssignChildrenContinentIds(country);

                    this.Data.Continents.AddUpdateIndexEntry(oldContinent);
                    this.Data.Continents.AddUpdateIndexEntry(newContinent);
                }

                this.Data.SaveChanges();
                this.Data.Countries.AddUpdateIndexEntry(country);
            }
            catch (InvalidOperationException ex)
            {
                this.TempData["ValidationError"] = ex.Message;

                this.Data.Countries.Detach(country);
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
                    .FirstOrDefault(c => c.Id == bindingModel.CountryId);
                var hasChildren = (country.Beaches.Count > 0 || country.PrimaryDivisions.Count > 0);

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

        [HttpGet]
        [RestructureAuthorize]
        public JsonResult Continent(int id)
        {
            var continentId = this.Data.Countries.Find(id).ContinentId;

            return this.Json(continentId, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [RestructureAuthorize]
        public JsonResult WaterBody(int id)
        {
            var waterBodyId = this.Data.Countries.Find(id).WaterBodyId;

            return this.Json(waterBodyId, JsonRequestBehavior.AllowGet);
        }

        #region Helpers

        private bool CanEditWaterBody(Country country)
        {
            var noChildren = (country.PrimaryDivisions.Count == 0);

            if (noChildren)
            {
                return true;
            }

            var firstWaterBodyId = country.WaterBodyId;
            var waterBodyAssignedAtPrimaryLevel = country.PrimaryDivisions.Any(pd => pd.WaterBodyId != firstWaterBodyId);

            if (waterBodyAssignedAtPrimaryLevel)
            {
                return false;
            }

            var waterBodyAssignedAtSecondaryLevel = country.SecondaryDivisions.Any(pd => pd.WaterBodyId != firstWaterBodyId);

            return waterBodyAssignedAtSecondaryLevel ? false : true;
        }

        private void AssignChildrenWaterBodyIds(Country country)
        {
            foreach (var beach in country.Beaches)
            {
                beach.WaterBodyId = (int)country.WaterBodyId;
            }

            foreach (var primaryDivision in country.PrimaryDivisions)
            {
                primaryDivision.WaterBodyId = country.WaterBodyId;
            }

            foreach (var secondaryDivision in country.SecondaryDivisions)
            {
                secondaryDivision.WaterBodyId = country.WaterBodyId;
            }
        }

        private void AssignChildrenContinentIds(Country country)
        {
            foreach (var beach in country.Beaches)
            {
                beach.ContinentId = (int)country.ContinentId;
            }

            foreach (var primaryDivision in country.PrimaryDivisions)
            {
                primaryDivision.ContinentId = country.ContinentId;
            }

            foreach (var secondaryDivision in country.SecondaryDivisions)
            {
                secondaryDivision.ContinentId = country.ContinentId;
            }

            foreach (var tertiaryDivision in country.TertiaryDivisions)
            {
                tertiaryDivision.ContinentId = country.ContinentId;
            }

            foreach (var quaternaryDivision in country.QuaternaryDivisions)
            {
                quaternaryDivision.ContinentId = country.ContinentId;
            }
        }

        #endregion
    }
}