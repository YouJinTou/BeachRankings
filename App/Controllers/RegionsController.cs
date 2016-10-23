namespace App.Controllers
{
    using AutoMapper;
    using BeachRankings.App.Models.ViewModels;
    using BeachRankings.Data.UnitOfWork;
    using BeachRankings.Models;
    using System.Linq;
    using System.Web.Mvc;

    public class RegionsController : BaseController
    {
        public RegionsController(IBeachRankingsData data)
            : base(data)
        {
        }

        public ActionResult Beaches(int id)
        {
            var region = this.Data.Regions.Find(id);
            var model = Mapper.Map<Region, RegionBeachesViewModel>(region);

            return View(model);
        }

        public JsonResult Areas(int id)
        {
            var areas = this.Data.Areas.All().Where(a => a.RegionId == id).Select(r => new SelectListItem()
            {
                Text = r.Name,
                Value = r.Id.ToString()
            });

            return this.Json(areas, JsonRequestBehavior.AllowGet);
        }

        public JsonResult WaterBody(int id)
        {
            var waterBodyId = this.Data.Regions.Find(id).WaterBodyId;

            return this.Json(waterBodyId, JsonRequestBehavior.AllowGet);
        }
    }
}