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

    public class QuaternaryDivisionsController : BaseController
    {
        public QuaternaryDivisionsController(IBeachRankingsData data)
            : base(data)
        {
        }

        public ActionResult Beaches(int id)
        {
            var quaternaryDivision = this.Data.QuaternaryDivisions.Find(id);
            var model = Mapper.Map<QuaternaryDivision, LocationBeachesViewModel>(quaternaryDivision);

            return this.View("LocationBeaches", model);
        }

        public PartialViewResult BeachesPartial(int id)
        {
            var beaches = this.Data.QuaternaryDivisions.All()
                 .Include(qd => qd.PrimaryDivision.WaterBody)
                 .FirstOrDefault(td => td.Id == id)
                 .Beaches
                 .Where(b => b.TotalScore != null);
            var model = Mapper.Map<IEnumerable<Beach>, IEnumerable<BeachTableRowViewModel>>(beaches);

            return this.PartialView(model);
        }

        public async Task<JsonResult> BeachNames(int id, string term)
        {
            var beachNames = await this.Data.Beaches.All()
                .Where(qd => qd.QuaternaryDivisionId == id && qd.Name.StartsWith(term))
                .Select(qd => qd.Name)
                .ToListAsync();

            return this.Json(beachNames, JsonRequestBehavior.AllowGet);
        }
    }
}