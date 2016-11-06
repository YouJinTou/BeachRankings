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

    public class PrimaryDivisionsController : BaseController
    {
        public PrimaryDivisionsController(IBeachRankingsData data)
            : base(data)
        {
        }

        public PartialViewResult Beaches(int id)
        {
            var beaches = this.Data.PrimaryDivisions.All()
                .Include(pd => pd.WaterBody)
                .Include(pd => pd.SecondaryDivisions)
                .Include(pd => pd.TertiaryDivisions)
                .Include(pd => pd.QuaternaryDivisions)
                .FirstOrDefault(pd => pd.Id == id)
                .Beaches
                .Where(b => b.Reviews.Count > 0);
            var model = Mapper.Map<IEnumerable<Beach>, IEnumerable<BeachTableRowViewModel>>(beaches);

            return this.PartialView(model);
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
                .Where(pd => pd.PrimaryDivisionId == id && pd.Name.StartsWith(term))
                .Select(pd => pd.Name)
                .ToListAsync();

            return this.Json(beachNames, JsonRequestBehavior.AllowGet);
        }
    }
}