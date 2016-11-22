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
                .Where(qd => qd.QuaternaryDivisionId == id && qd.Name.StartsWith(term))
                .Select(qd => qd.Name)
                .ToListAsync();

            return this.Json(beachNames, JsonRequestBehavior.AllowGet);
        }
    }
}