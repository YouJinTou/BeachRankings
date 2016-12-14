namespace App.Controllers
{
    using AutoMapper;
    using BeachRankings.App.Models.ViewModels;
    using BeachRankings.Data.UnitOfWork;
    using BeachRankings.Models;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Web.Mvc;

    public class ContinentsController : BasePlacesController
    {
        public ContinentsController(IBeachRankingsData data)
            : base(data)
        {
        }

        public ActionResult Beaches(int id, int page = 0, int pageSize = 10)
        {
            var continent = this.Data.Continents.Find(id);
            var model = Mapper.Map<Continent, PlaceBeachesViewModel>(continent);
            model.Beaches = model.Beaches.OrderByDescending(b => b.TotalScore).Skip(page * pageSize).Take(pageSize);

            model.Beaches.Select(b => { b.UserHasRated = base.UserHasRated(b); return b; }).ToList();

            return this.View("_PlaceBeaches", model);
        }

        public PartialViewResult Statistics(int id)
        {
            var continent = this.Data.Continents.All()
                .Include(c => c.Countries)
                .Include(c => c.PrimaryDivisions.Select(pd => pd.WaterBody))
                .Include(c => c.SecondaryDivisions)
                .Include(c => c.TertiaryDivisions)
                .Include(c => c.QuaternaryDivisions)
                .Include(c => c.Beaches)
                .FirstOrDefault(c => c.Id == id);
            var beaches = continent.Beaches.Where(b => b.TotalScore != null).OrderByDescending(b => b.TotalScore);
            var model = new StatisticsViewModel()
            {
                Id = id,
                Controller = "Continents",
                Name = continent.Name,
                Rows = Mapper.Map<IEnumerable<Beach>, IEnumerable<BeachRowViewModel>>(beaches)
            };

            return this.PartialView("_StatisticsPartial", model);
        }
    }
}