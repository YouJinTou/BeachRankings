namespace App.Controllers
{
    using AutoMapper;
    using BeachRankings.App.Models.ViewModels;
    using BeachRankings.Data.UnitOfWork;
    using BeachRankings.Models;
    using System.Linq;
    using System.Web.Mvc;

    public class PrimaryDivisionsController : BaseController
    {
        public PrimaryDivisionsController(IBeachRankingsData data)
            : base(data)
        {
        }

        public ActionResult Beaches(int id)
        {
            var primaryDivision = this.Data.PrimaryDivisions.Find(id);
            var model = Mapper.Map<PrimaryDivision, LocationBeachesViewModel>(primaryDivision);

            return View(model);
        }

        public JsonResult SecondaryDivisions(int id)
        {
            var secondaryDivisions = this.Data.SecondaryDivisions.All().Where(a => a.PrimaryDivisionId == id).Select(r => new SelectListItem()
            {
                Text = r.Name,
                Value = r.Id.ToString()
            });

            return this.Json(secondaryDivisions, JsonRequestBehavior.AllowGet);
        }

        public JsonResult WaterBody(int id)
        {
            var waterBodyId = this.Data.PrimaryDivisions.Find(id).WaterBodyId;

            return this.Json(waterBodyId, JsonRequestBehavior.AllowGet);
        }
    }
}