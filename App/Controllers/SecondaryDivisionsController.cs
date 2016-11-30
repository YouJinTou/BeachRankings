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
            model.Beaches = model.Beaches.Skip(page * pageSize).Take(pageSize);

            model.Beaches.Select(b => { b.UserHasRated = base.UserHasRated(b); return b; }).ToList();

            return this.View("_PlaceBeaches", model);
        }

        public PartialViewResult Statistics(int id)
        {
            var beaches = this.Data.SecondaryDivisions.All()
                .Include(sd => sd.PrimaryDivision.WaterBody)
                .Include(sd => sd.TertiaryDivisions)
                .Include(sd => sd.QuaternaryDivisions)
                .FirstOrDefault(sd => sd.Id == id)
                .Beaches
                .Where(b => b.TotalScore != null);
            var model = Mapper.Map<IEnumerable<Beach>, IEnumerable<BeachRowViewModel>>(beaches);

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
                secondaryDivision = new SecondaryDivision()
                {
                    Name = bindingModel.SecondaryDivision,
                    CountryId = (int)bindingModel.CountryId,
                    PrimaryDivisionId = (int)bindingModel.PrimaryDivisionId,
                    WaterBodyId = bindingModel.WaterBodyId
                };

                this.Data.SecondaryDivisions.Add(secondaryDivision);
                this.Data.SecondaryDivisions.SaveChanges();
                this.Data.SecondaryDivisions.AddUpdateIndexEntry(secondaryDivision);
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
                secondaryDivision = this.Data.SecondaryDivisions.Find(bindingModel.SecondaryDivisionId);
                secondaryDivision.Name = bindingModel.SecondaryDivision;
                secondaryDivision.WaterBodyId = bindingModel.WaterBodyId;

                this.Data.SecondaryDivisions.SaveChanges();
                this.Data.SecondaryDivisions.AddUpdateIndexEntry(secondaryDivision);
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
                    .Include(sd => sd.QuaternaryDivisions)
                    .FirstOrDefault(sd => sd.Id == bindingModel.SecondaryDivisionId);
                var hasChildren = (secondaryDivision.Beaches.Count > 0 || 
                    secondaryDivision.TertiaryDivisions.Count > 0 || 
                    secondaryDivision.QuaternaryDivisions.Count > 0);

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
    }
}