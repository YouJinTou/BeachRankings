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

    public class TertiaryDivisionsController : BasePlacesController
    {
        public TertiaryDivisionsController(IBeachRankingsData data)
            : base(data)
        {
        }

        public ActionResult Beaches(int id, int page = 0, int pageSize = 10)
        {
            var tertiaryDivision = this.Data.TertiaryDivisions.Find(id);
            var model = Mapper.Map<TertiaryDivision, PlaceBeachesViewModel>(tertiaryDivision);
            model.Beaches = model.Beaches.Skip(page * pageSize).Take(pageSize);

            model.Beaches.Select(b => { b.UserHasRated = base.UserHasRated(b); return b; }).ToList();

            return this.View("_PlaceBeaches", model);
        }

        public PartialViewResult Statistics(int id)
        {
            var beaches = this.Data.TertiaryDivisions.All()
                 .Include(td => td.PrimaryDivision.WaterBody)
                 .Include(td => td.QuaternaryDivisions)
                 .FirstOrDefault(td => td.Id == id)
                 .Beaches
                 .Where(b => b.TotalScore != null);
            var model = Mapper.Map<IEnumerable<Beach>, IEnumerable<TableRowViewModel>>(beaches);

            return this.PartialView("_StatisticsPartial", model);
        }

        public JsonResult QuaternaryDivisions(int id)
        {
            var quaternaryDivisions = this.Data.QuaternaryDivisions.All().Where(qd => qd.TertiaryDivisionId == id).Select(r => new SelectListItem()
            {
                Text = r.Name,
                Value = r.Id.ToString()
            });

            return this.Json(quaternaryDivisions, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> BeachNames(int id, string term)
        {
            var beachNames = await this.Data.Beaches.All()
                .Where(b => b.TertiaryDivisionId == id && b.Name.StartsWith(term))
                .Select(b => b.Name)
                .ToListAsync();

            return this.Json(beachNames, JsonRequestBehavior.AllowGet);
        }

        public JsonResult AdminBeaches(int id)
        {
            var beaches = this.Data.Beaches.All().Where(b => b.TertiaryDivisionId == id).Select(b => new SelectListItem
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
            TertiaryDivision tertiaryDivision = null;

            try
            {
                tertiaryDivision = new TertiaryDivision()
                {
                    Name = bindingModel.TertiaryDivision,
                    CountryId = (int)bindingModel.CountryId,
                    PrimaryDivisionId = (int)bindingModel.PrimaryDivisionId,
                    SecondaryDivisionId = (int)bindingModel.SecondaryDivisionId,
                };

                this.Data.TertiaryDivisions.Add(tertiaryDivision);
                this.Data.TertiaryDivisions.SaveChanges();
                this.Data.TertiaryDivisions.AddUpdateIndexEntry(tertiaryDivision);
            }
            catch (Exception)
            {
                this.TempData["ValidationError"] = "The name of the third-level division is either a duplicate or is missing.";

                if (tertiaryDivision != null)
                {
                    this.Data.TertiaryDivisions.Detach(tertiaryDivision);
                }
            }

            return this.RedirectToAction("Restructure", "Admin");
        }

        [HttpPost]
        [RestructureAuthorize]
        public ActionResult Edit(RestructureViewModel bindingModel)
        {
            TertiaryDivision tertiaryDivision = null;

            try
            {
                tertiaryDivision = this.Data.TertiaryDivisions.Find(bindingModel.TertiaryDivisionId);
                tertiaryDivision.Name = bindingModel.TertiaryDivision;

                this.Data.TertiaryDivisions.SaveChanges();
                this.Data.TertiaryDivisions.AddUpdateIndexEntry(tertiaryDivision);
            }
            catch (Exception)
            {
                this.TempData["ValidationError"] = "The name of the third-level division is either a duplicate or is missing.";

                if (tertiaryDivision != null)
                {
                    this.Data.TertiaryDivisions.Detach(tertiaryDivision);
                }
            }

            return this.RedirectToAction("Restructure", "Admin");
        }

        [HttpPost]
        [RestructureAuthorize]
        public ActionResult Delete(RestructureViewModel bindingModel)
        {
            TertiaryDivision tertiaryDivision = null;

            try
            {
                tertiaryDivision = this.Data.TertiaryDivisions.All()
                    .Include(td => td.Beaches)
                    .Include(td => td.QuaternaryDivisions)
                    .FirstOrDefault(td => td.Id == bindingModel.TertiaryDivisionId);
                var hasChildren = (tertiaryDivision.Beaches.Count > 0 || tertiaryDivision.QuaternaryDivisions.Count > 0);

                if (hasChildren)
                {
                    throw new InvalidOperationException();
                }

                this.Data.TertiaryDivisions.Remove(tertiaryDivision);
                this.Data.TertiaryDivisions.SaveChanges();
                this.Data.TertiaryDivisions.DeleteIndexEntry(tertiaryDivision);
            }
            catch (Exception)
            {
                this.TempData["ValidationError"] = 
                    "Could not delete third-level division. All of its children must be deleted first and its beaches moved to another division.";

                if (tertiaryDivision != null)
                {
                    this.Data.TertiaryDivisions.Detach(tertiaryDivision);
                }
            }

            return this.RedirectToAction("Restructure", "Admin");
        }
    }
}