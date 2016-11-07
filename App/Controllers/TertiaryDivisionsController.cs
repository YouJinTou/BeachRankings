namespace App.Controllers
{
    using AutoMapper;
    using BeachRankings.App.Models.ViewModels;
    using BeachRankings.Data.UnitOfWork;
    using BeachRankings.Models;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;

    public class TertiaryDivisionsController : BaseController
    {
        public TertiaryDivisionsController(IBeachRankingsData data)
            : base(data)
        {
        }

        public ActionResult Beaches(int id)
        {
            var tertiaryDivision = this.Data.TertiaryDivisions.Find(id);
            var model = Mapper.Map<TertiaryDivision, LocationBeachesViewModel>(tertiaryDivision);

            return this.View("LocationBeaches", model);
        }

        public PartialViewResult BeachesPartial(int id)
        {
            var beaches = this.Data.TertiaryDivisions.All()
                 .Include(td => td.PrimaryDivision.WaterBody)
                 .Include(td => td.QuaternaryDivisions)
                 .FirstOrDefault(td => td.Id == id)
                 .Beaches
                 .Where(b => b.TotalScore != null);
            var model = Mapper.Map<IEnumerable<Beach>, IEnumerable<BeachTableRowViewModel>>(beaches);

            return this.PartialView(model);
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
    }
}