namespace App.Controllers
{
    using AutoMapper;
    using BeachRankings.App.Models.ViewModels;
    using BeachRankings.Data.UnitOfWork;
    using BeachRankings.Models;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;

    public class SecondaryDivisionsController : BaseController
    {
        public SecondaryDivisionsController(IBeachRankingsData data)
            : base(data)
        {
        }

        public ActionResult Beaches(int id)
        {
            var secondaryDivision = this.Data.SecondaryDivisions.Find(id);
            var model = Mapper.Map<SecondaryDivision, LocationBeachesViewModel>(secondaryDivision);

            return View(model);
        }

        public async Task<JsonResult> BeachNames(int id, string term)
        {
            var beachNames = await this.Data.Beaches.All()
                .Where(a => a.SecondaryDivisionId == id && a.Name.StartsWith(term))
                .Select(a => a.Name)
                .ToListAsync();

            return this.Json(beachNames, JsonRequestBehavior.AllowGet);
        }
    }
}