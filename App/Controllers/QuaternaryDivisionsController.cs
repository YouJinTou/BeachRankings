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

    public class QuaternaryDivisionsController : BaseLocationsController
    {
        public QuaternaryDivisionsController(IBeachRankingsData data)
            : base(data)
        {
        }

        public ActionResult Beaches(int id, int page = 0, int pageSize = 10)
        {
            var quaternaryDivision = this.Data.QuaternaryDivisions.Find(id);
            var model = Mapper.Map<QuaternaryDivision, LocationBeachesViewModel>(quaternaryDivision);
            model.Beaches = model.Beaches.Skip(page * pageSize).Take(pageSize);

            model.Beaches.Select(b => { b.UserHasRated = base.UserHasRated(b); return b; }).ToList();

            return this.View("_LocationBeaches", model);
        }

        public PartialViewResult Statistics(int id)
        {
            var beaches = this.Data.QuaternaryDivisions.All()
                 .Include(qd => qd.PrimaryDivision.WaterBody)
                 .FirstOrDefault(td => td.Id == id)
                 .Beaches
                 .Where(b => b.TotalScore != null);
            var model = Mapper.Map<IEnumerable<Beach>, IEnumerable<TableRowViewModel>>(beaches);

            return this.PartialView("_StatisticsPartial", model);
        }

        public async Task<JsonResult> BeachNames(int id, string term)
        {
            var beachNames = await this.Data.Beaches.All()
                .Where(b => b.QuaternaryDivisionId == id && b.Name.StartsWith(term))
                .Select(b => b.Name)
                .ToListAsync();

            return this.Json(beachNames, JsonRequestBehavior.AllowGet);
        }

        public JsonResult AdminBeaches(int id)
        {
            var beaches = this.Data.Beaches.All().Where(b => b.QuaternaryDivisionId == id).Select(b => new SelectListItem
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
                var quaternaryDivision = new QuaternaryDivision()
                {
                    Name = bindingModel.QuaternaryDivision,
                    CountryId = (int)bindingModel.CountryId,
                    PrimaryDivisionId = (int)bindingModel.PrimaryDivisionId,
                    SecondaryDivisionId = (int)bindingModel.SecondaryDivisionId,
                    TertiaryDivisionId = (int)bindingModel.TertiaryDivisionId
                };

                this.Data.QuaternaryDivisions.Add(quaternaryDivision);
                this.Data.QuaternaryDivisions.SaveChanges();
                this.Data.QuaternaryDivisions.AddUpdateIndexEntry(quaternaryDivision);
            }
            catch (Exception)
            {
                this.TempData["ValidationError"] = "The name of the fourth-level division is either a duplicate or is missing.";
            }

            return this.RedirectToAction("Restructure", "Admin");
        }

        [HttpPost]
        [RestructureAuthorize]
        public ActionResult Edit(RestructureViewModel bindingModel)
        {
            try
            {
                var quaternaryDivision = this.Data.Countries.Find(bindingModel.QuaternaryDivisionId);
                quaternaryDivision.Name = bindingModel.QuaternaryDivision;

                this.Data.QuaternaryDivisions.SaveChanges();
                this.Data.QuaternaryDivisions.AddUpdateIndexEntry(quaternaryDivision);
            }
            catch (Exception)
            {
                this.TempData["ValidationError"] = "The name of the fourth-level division is either a duplicate or is missing.";
            }

            return this.RedirectToAction("Restructure", "Admin");
        }
    }
}