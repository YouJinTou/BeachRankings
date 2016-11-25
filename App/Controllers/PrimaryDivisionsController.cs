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

    public class PrimaryDivisionsController : BaseLocationsController
    {
        public PrimaryDivisionsController(IBeachRankingsData data)
            : base(data)
        {
        }

        public ActionResult Beaches(int id, int page = 0, int pageSize = 10)
        {
            var primaryDivision = this.Data.PrimaryDivisions.Find(id);
            var model = Mapper.Map<PrimaryDivision, LocationBeachesViewModel>(primaryDivision);
            model.Beaches = model.Beaches.Skip(page * pageSize).Take(pageSize);

            model.Beaches.Select(b => { b.UserHasRated = base.UserHasRated(b); return b; }).ToList();

            return this.View("_LocationBeaches", model);
        }

        public PartialViewResult Statistics(int id)
        {
            var beaches = this.Data.PrimaryDivisions.All()
                .Include(pd => pd.WaterBody)
                .Include(pd => pd.SecondaryDivisions)
                .Include(pd => pd.TertiaryDivisions)
                .Include(pd => pd.QuaternaryDivisions)
                .FirstOrDefault(pd => pd.Id == id)
                .Beaches
                .Where(b => b.TotalScore != null);
            var model = Mapper.Map<IEnumerable<Beach>, IEnumerable<TableRowViewModel>>(beaches);

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
            try
            {
                var primaryDivision = new PrimaryDivision() { Name = bindingModel.PrimaryDivision, CountryId = (int)bindingModel.CountryId };

                this.Data.PrimaryDivisions.Add(primaryDivision);
                this.Data.PrimaryDivisions.SaveChanges();
                this.Data.PrimaryDivisions.AddUpdateIndexEntry(primaryDivision);
            }
            catch (Exception)
            {
                this.TempData["ValidationError"] = "The name of the first-level division is either a duplicate or is missing.";
            }

            return this.RedirectToAction("Restructure", "Admin");
        }

        [HttpPost]
        [RestructureAuthorize]
        public ActionResult Edit(RestructureViewModel bindingModel)
        {
            try
            {
                var primaryDivision = this.Data.PrimaryDivisions.Find(bindingModel.PrimaryDivisionId);
                primaryDivision.Name = bindingModel.PrimaryDivision;

                this.Data.PrimaryDivisions.SaveChanges();
                this.Data.PrimaryDivisions.AddUpdateIndexEntry(primaryDivision);
            }
            catch (Exception)
            {
                this.TempData["ValidationError"] = "The name of the first-level division is either a duplicate or is missing.";
            }

            return this.RedirectToAction("Restructure", "Admin");
        }
    }
}