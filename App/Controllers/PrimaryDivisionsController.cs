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

    public class PrimaryDivisionsController : BasePlacesController
    {
        public PrimaryDivisionsController(IBeachRankingsData data)
            : base(data)
        {
        }

        public ActionResult Beaches(int id, int page = 0, int pageSize = 10)
        {
            var primaryDivision = this.Data.PrimaryDivisions.Find(id);
            var model = Mapper.Map<PrimaryDivision, PlaceBeachesViewModel>(primaryDivision);
            model.Beaches = model.Beaches.OrderByDescending(b => b.TotalScore).Skip(page * pageSize).Take(pageSize);

            model.Beaches.Select(b => { b.UserHasRated = base.UserHasRated(b); return b; }).ToList();

            return this.View("_PlaceBeaches", model);
        }

        public PartialViewResult Statistics(int id)
        {
            var primaryDivision = this.Data.PrimaryDivisions.All()
                .Include(pd => pd.WaterBody)
                .Include(pd => pd.SecondaryDivisions)
                .Include(pd => pd.TertiaryDivisions)
                .Include(pd => pd.QuaternaryDivisions)
                .Include(pd => pd.Beaches)
                .FirstOrDefault(pd => pd.Id == id);
            var beaches = primaryDivision.Beaches.Where(b => b.TotalScore != null).OrderByDescending(b => b.TotalScore);
            var model = new StatisticsViewModel()
            {
                Id = id,
                Controller = "PrimaryDivisions",
                Name = primaryDivision.Name,
                Rows = Mapper.Map<IEnumerable<Beach>, IEnumerable<BeachRowViewModel>>(beaches)
            };

            return this.PartialView("_StatisticsPartial", model);
        }

        public JsonResult SecondaryDivisions(int id)
        {
            var secondaryDivisions = this.Data.SecondaryDivisions.All().Where(sd => sd.PrimaryDivisionId == id).Select(sd => new SelectListItem()
            {
                Text = sd.Name,
                Value = sd.Id.ToString()
            });

            return this.Json(secondaryDivisions, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> BeachNames(int id, string term)
        {
            var beachNames = await this.Data.Beaches.All()
                .Where(b => b.PrimaryDivisionId == id && b.Name.StartsWith(term))
                .Select(b => b.Name)
                .ToListAsync();

            return this.Json(beachNames, JsonRequestBehavior.AllowGet);
        }

        public JsonResult AdminBeaches(int id)
        {
            var beaches = this.Data.Beaches.All().Where(b => b.PrimaryDivisionId == id).Select(b => new SelectListItem
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
            PrimaryDivision primaryDivision = null;

            try
            {
                var country = this.Data.Countries.Find(bindingModel.CountryId);
                primaryDivision = new PrimaryDivision()
                {
                    Name = bindingModel.PrimaryDivision,
                    ContinentId = country.ContinentId,
                    CountryId = country.Id,
                    WaterBodyId = bindingModel.WaterBodyId
                };
                var waterBodyIdHasValue = (bindingModel.WaterBodyId != null);

                if (waterBodyIdHasValue && !this.CanAddEditWaterBody(primaryDivision, true))
                {
                    throw new InvalidOperationException("Cannnot assign a water body; the water body is assigned at a different level.");
                }

                if (!waterBodyIdHasValue)
                {
                    primaryDivision.WaterBodyId = country.WaterBodyId;
                }

                this.Data.PrimaryDivisions.Add(primaryDivision);
                this.Data.PrimaryDivisions.SaveChanges();
                this.Data.PrimaryDivisions.AddUpdateIndexEntry(primaryDivision);
            }
            catch (InvalidOperationException ex)
            {
                this.TempData["ValidationError"] = ex.Message;

                this.Data.PrimaryDivisions.Detach(primaryDivision);
            }
            catch (Exception)
            {
                this.TempData["ValidationError"] = "The name of the first-level division is either a duplicate or is missing.";

                if (primaryDivision != null)
                {
                    this.Data.PrimaryDivisions.Detach(primaryDivision);
                }
            }

            return this.RedirectToAction("Restructure", "Admin");
        }

        [HttpPost]
        [RestructureAuthorize]
        public ActionResult Edit(RestructureViewModel bindingModel)
        {
            PrimaryDivision primaryDivision = null;

            try
            {
                primaryDivision = this.Data.PrimaryDivisions.All()
                    .Include(pd => pd.Beaches)
                    .Include(pd => pd.SecondaryDivisions)
                    .FirstOrDefault(pd => pd.Id == bindingModel.PrimaryDivisionId);
                primaryDivision.Name = bindingModel.PrimaryDivision;
                var waterBodyIdIsNew = (primaryDivision.WaterBodyId != bindingModel.WaterBodyId);

                if (waterBodyIdIsNew)
                {
                    if (!this.CanAddEditWaterBody(primaryDivision, false))
                    {
                        throw new InvalidOperationException("Cannnot assign a water body; the water body is assigned at a different level.");
                    }

                    var oldWaterBody = this.Data.WaterBodies.Find(primaryDivision.WaterBodyId);
                    var newWaterBody = this.Data.WaterBodies.Find(bindingModel.WaterBodyId);

                    primaryDivision.WaterBodyId = bindingModel.WaterBodyId;

                    this.AssignChildrenWaterBodyIds(primaryDivision);

                    this.Data.Beaches.SaveChanges();
                    this.Data.SecondaryDivisions.SaveChanges();

                    this.Data.WaterBodies.AddUpdateIndexEntry(oldWaterBody);
                    this.Data.WaterBodies.AddUpdateIndexEntry(newWaterBody);
                }

                this.Data.PrimaryDivisions.SaveChanges();
                this.Data.PrimaryDivisions.AddUpdateIndexEntry(primaryDivision);
            }
            catch (InvalidOperationException ex)
            {
                this.TempData["ValidationError"] = ex.Message;

                this.Data.PrimaryDivisions.Detach(primaryDivision);
            }
            catch (Exception)
            {
                this.TempData["ValidationError"] = "The name of the first-level division is either a duplicate or is missing.";

                if (primaryDivision != null)
                {
                    this.Data.PrimaryDivisions.Detach(primaryDivision);
                }
            }

            return this.RedirectToAction("Restructure", "Admin");
        }

        [HttpPost]
        [RestructureAuthorize]
        public ActionResult Delete(RestructureViewModel bindingModel)
        {
            PrimaryDivision primaryDivision = null;

            try
            {
                primaryDivision = this.Data.PrimaryDivisions.All()
                   .Include(pd => pd.Beaches)
                   .Include(pd => pd.Country)
                   .Include(pd => pd.SecondaryDivisions)
                   .FirstOrDefault(pd => pd.Id == bindingModel.PrimaryDivisionId);
                var hasChildren = (primaryDivision.Beaches.Count > 0 || primaryDivision.SecondaryDivisions.Count > 0);

                if (hasChildren)
                {
                    throw new InvalidOperationException();
                }

                this.Data.PrimaryDivisions.Remove(primaryDivision);
                this.Data.PrimaryDivisions.SaveChanges();
                this.Data.PrimaryDivisions.DeleteIndexEntry(primaryDivision);
            }
            catch (Exception)
            {
                this.TempData["ValidationError"] =
                    "Could not delete first-level division. All of its children must be deleted first and its beaches moved to another division.";

                if (primaryDivision != null)
                {
                    this.Data.PrimaryDivisions.Detach(primaryDivision);
                }
            }

            return this.RedirectToAction("Restructure", "Admin");
        }

        [HttpGet]
        public JsonResult WaterBody(int id)
        {
            var waterBodyId = this.Data.PrimaryDivisions.Find(id).WaterBodyId;

            return this.Json(waterBodyId, JsonRequestBehavior.AllowGet);
        }

        #region Helpers

        private bool CanAddEditWaterBody(PrimaryDivision primaryDivision, bool adding)
        {
            var country = adding ? this.Data.Countries.Find(primaryDivision.CountryId) : primaryDivision.Country;
            var waterBodyAssignedAtCountryLevel = (country.WaterBodyId != null);

            if (waterBodyAssignedAtCountryLevel)
            {
                return false;
            }

            var primaryDivisions = this.Data.PrimaryDivisions.All().Where(pd => pd.CountryId == country.Id).ToList();
            var secondaryDivisions = this.Data.SecondaryDivisions.All().Where(pd => pd.CountryId == country.Id).ToList();
            var justCreated = (primaryDivisions.All(pd => pd.WaterBodyId == null) && secondaryDivisions.All(sd => sd.WaterBodyId == null));
            var waterBodyAssignedAtPrimaryLevel = justCreated ? true :
                (secondaryDivisions.Count > 0) ? primaryDivisions.All(pd => pd.WaterBodyId != null) : false;

            return waterBodyAssignedAtPrimaryLevel;
        }

        private void AssignChildrenWaterBodyIds(PrimaryDivision primaryDivision)
        {
            foreach (var beach in primaryDivision.Beaches)
            {
                beach.WaterBodyId = (int)primaryDivision.WaterBodyId;
            }

            foreach (var secondaryDivision in primaryDivision.SecondaryDivisions)
            {
                secondaryDivision.WaterBodyId = primaryDivision.WaterBodyId;
            }
        }

        #endregion
    }
}