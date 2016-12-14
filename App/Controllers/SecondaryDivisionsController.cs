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

    public class SecondaryDivisionsController : BasePlacesController
    {
        public SecondaryDivisionsController(IBeachRankingsData data)
            : base(data)
        {
        }

        public ActionResult Beaches(int id, int page = 0, int pageSize = 10)
        {
            var secondaryDivision = this.Data.SecondaryDivisions.Find(id);
            var model = Mapper.Map<SecondaryDivision, PlaceBeachesViewModel>(secondaryDivision);
            model.Beaches = model.Beaches.OrderByDescending(b => b.TotalScore).Skip(page * pageSize).Take(pageSize);

            model.Beaches.Select(b => { b.UserHasRated = base.UserHasRated(b); return b; }).ToList();

            return this.View("_PlaceBeaches", model);
        }

        public PartialViewResult Statistics(int id)
        {
            var secondaryDivisions = this.Data.SecondaryDivisions.All()
                .Include(sd => sd.PrimaryDivision.WaterBody)
                .Include(sd => sd.TertiaryDivisions)
                .Include(sd => sd.QuaternaryDivisions)
                .Include(sd => sd.Beaches)
                .FirstOrDefault(sd => sd.Id == id);
            var beaches = secondaryDivisions.Beaches.Where(b => b.TotalScore != null).OrderByDescending(b => b.TotalScore);
            var model = new StatisticsViewModel()
            {
                Id = id,
                Controller = "SecondaryDivisions",
                Name = secondaryDivisions.Name,
                Rows = Mapper.Map<IEnumerable<Beach>, IEnumerable<BeachRowViewModel>>(beaches)
            };

            return this.PartialView("_StatisticsPartial", model);
        }

        public JsonResult TertiaryDivisions(int id)
        {
            var tertiaryDivisions = this.Data.TertiaryDivisions.All().Where(td => td.SecondaryDivisionId == id).Select(td => new SelectListItem()
            {
                Text = td.Name,
                Value = td.Id.ToString()
            });

            return this.Json(tertiaryDivisions, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> BeachNames(int id, string term)
        {
            var beachNames = await this.Data.Beaches.All()
                .Where(b => b.SecondaryDivisionId == id && b.Name.StartsWith(term))
                .Select(b => b.Name)
                .ToListAsync();

            return this.Json(beachNames, JsonRequestBehavior.AllowGet);
        }

        public JsonResult AdminBeaches(int id)
        {
            var beaches = this.Data.Beaches.All().Where(b => b.SecondaryDivisionId == id).Select(b => new SelectListItem
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
            SecondaryDivision secondaryDivision = null;

            try
            {
                var country = this.Data.Countries.Find(bindingModel.CountryId);
                secondaryDivision = new SecondaryDivision()
                {
                    Name = bindingModel.SecondaryDivision,
                    ContinentId = country.ContinentId,
                    CountryId = country.Id,
                    PrimaryDivisionId = (int)bindingModel.PrimaryDivisionId,
                    WaterBodyId = bindingModel.WaterBodyId
                };

                if (!this.CanAddEditWaterBody(secondaryDivision, true))
                {
                    throw new InvalidOperationException("Cannnot assign a water body; the water body is assigned at a different level.");
                }

                var missingWaterBody = (bindingModel.WaterBodyId == null && !this.TryAddWaterBody(secondaryDivision));

                if (missingWaterBody)
                {
                    throw new InvalidOperationException("You must assign a water body when adding a second-level division.");
                }

                this.Data.SecondaryDivisions.Add(secondaryDivision);
                this.Data.SecondaryDivisions.SaveChanges();
                this.Data.SecondaryDivisions.AddUpdateIndexEntry(secondaryDivision);
            }
            catch (InvalidOperationException ex)
            {
                this.TempData["ValidationError"] = ex.Message;

                this.Data.SecondaryDivisions.Detach(secondaryDivision);
            }
            catch (Exception)
            {
                this.TempData["ValidationError"] = "The name of the second-level division is either a duplicate or is missing.";

                if (secondaryDivision != null)
                {
                    this.Data.SecondaryDivisions.Detach(secondaryDivision);
                }
            }

            return this.RedirectToAction("Restructure", "Admin");
        }

        [HttpPost]
        [RestructureAuthorize]
        public ActionResult Edit(RestructureViewModel bindingModel)
        {
            SecondaryDivision secondaryDivision = null;

            try
            {
                secondaryDivision = this.Data.SecondaryDivisions.All()
                    .Include(sd => sd.Beaches)
                    .Include(sd => sd.Country)
                    .Include(sd => sd.PrimaryDivision)
                    .FirstOrDefault(sd => sd.Id == bindingModel.SecondaryDivisionId);
                secondaryDivision.Name = bindingModel.SecondaryDivision;
                var waterBodyIdIsNew = (secondaryDivision.WaterBodyId != bindingModel.WaterBodyId);

                if (waterBodyIdIsNew)
                {
                    if (!this.CanAddEditWaterBody(secondaryDivision, false))
                    {
                        throw new InvalidOperationException("Cannnot assign a water body; the water body is assigned at a different level.");
                    }

                    var oldWaterBody = this.Data.WaterBodies.Find(secondaryDivision.WaterBodyId);
                    var newWaterBody = this.Data.WaterBodies.Find(bindingModel.WaterBodyId);

                    secondaryDivision.WaterBodyId = bindingModel.WaterBodyId;

                    this.AssignChildrenWaterBodyIds(secondaryDivision);

                    this.Data.Beaches.SaveChanges();

                    this.Data.WaterBodies.AddUpdateIndexEntry(oldWaterBody);
                    this.Data.WaterBodies.AddUpdateIndexEntry(newWaterBody);
                }

                this.Data.SecondaryDivisions.SaveChanges();
                this.Data.SecondaryDivisions.AddUpdateIndexEntry(secondaryDivision);
            }
            catch (InvalidOperationException ex)
            {
                this.TempData["ValidationError"] = ex.Message;

                this.Data.SecondaryDivisions.Detach(secondaryDivision);
            }
            catch (Exception)
            {
                this.TempData["ValidationError"] = "The name of the second-level division is either a duplicate or is missing.";

                if (secondaryDivision != null)
                {
                    this.Data.SecondaryDivisions.Detach(secondaryDivision);
                }
            }

            return this.RedirectToAction("Restructure", "Admin");
        }

        [HttpPost]
        [RestructureAuthorize]
        public ActionResult Delete(RestructureViewModel bindingModel)
        {
            SecondaryDivision secondaryDivision = null;

            try
            {                
                secondaryDivision = this.Data.SecondaryDivisions.All()
                    .Include(sd => sd.Beaches)
                    .Include(sd => sd.TertiaryDivisions)
                    .FirstOrDefault(sd => sd.Id == bindingModel.SecondaryDivisionId);
                var hasChildren = (secondaryDivision.Beaches.Count > 0 || secondaryDivision.TertiaryDivisions.Count > 0);

                if (hasChildren)
                {
                    throw new InvalidOperationException();
                }

                this.Data.SecondaryDivisions.Remove(secondaryDivision);
                this.Data.SecondaryDivisions.SaveChanges();
                this.Data.SecondaryDivisions.DeleteIndexEntry(secondaryDivision);
            }
            catch (Exception)
            {
                this.TempData["ValidationError"] =
                    "Could not delete second-level division. All of its children must be deleted first and its beaches moved to another division.";

                if (secondaryDivision != null)
                {
                    this.Data.SecondaryDivisions.Detach(secondaryDivision);
                }
            }

            return this.RedirectToAction("Restructure", "Admin");
        }

        [HttpGet]
        public JsonResult WaterBody(int id)
        {
            var waterBodyId = this.Data.SecondaryDivisions.Find(id).WaterBodyId;

            return this.Json(waterBodyId, JsonRequestBehavior.AllowGet);
        }

        #region Helpers

        private bool CanAddEditWaterBody(SecondaryDivision secondaryDivision, bool adding)
        {
            var country = adding ? this.Data.Countries.Find(secondaryDivision.CountryId) : secondaryDivision.Country;
            var primaryDivision = adding ? this.Data.PrimaryDivisions.Find(secondaryDivision.PrimaryDivisionId) : secondaryDivision.PrimaryDivision;
            var waterBodyAssignedAtHigherLevel = (country.WaterBodyId != null || primaryDivision.WaterBodyId != null);

            if (adding)
            {
                var overriding = (secondaryDivision.WaterBodyId != null);

                return waterBodyAssignedAtHigherLevel ? 
                    overriding ? false : true :
                    true;
            }

            return waterBodyAssignedAtHigherLevel ? false : true;
        }

        private bool TryAddWaterBody(SecondaryDivision secondaryDivision)
        {
            var country = this.Data.Countries.Find(secondaryDivision.CountryId);
            var primaryDivision = this.Data.PrimaryDivisions.Find(secondaryDivision.PrimaryDivisionId);
            var waterBodyId = (country.WaterBodyId != null) ? country.WaterBodyId : primaryDivision.WaterBodyId;

            if (waterBodyId == null)
            {
                return false;
            }

            secondaryDivision.WaterBodyId = waterBodyId;

            return true;
        }

        private void AssignChildrenWaterBodyIds(SecondaryDivision secondaryDivision)
        {
            foreach (var beach in secondaryDivision.Beaches)
            {
                beach.WaterBodyId = (int)secondaryDivision.WaterBodyId;
            }
        }

        #endregion
    }
}