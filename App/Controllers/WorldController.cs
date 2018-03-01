namespace App.Controllers
{
    using AutoMapper;
    using BeachRankings.App.Models.ViewModels;
    using BeachRankings.Data.UnitOfWork;
    using BeachRankings.Models;
    using BeachRankings.Models.Enums;
    using BeachRankings.Services.Aggregation;
    using BeachRankings.Extensions;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Web.Mvc;

    public class WorldController : BasePlacesController
    {
        public WorldController(IBeachRankingsData data, IDataAggregationService aggregationService)
            : base(data, aggregationService)
        {
        }

        public ActionResult Beaches(int id = 1, int page = 0, int pageSize = 10)
        {
            var continents = this.Data.Continents.All().Include(c => c.Beaches).ToList();
            var beaches = continents
                .SelectMany(c => c.Beaches)
                .OrderByDescending(b => b.TotalScore)
                .Skip(page * pageSize)
                .Take(pageSize)
                .ToList();
            var model = new PlaceBeachesViewModel
            {
                Id = id,
                Controller = "World",
                Beaches = Mapper.Map<IEnumerable<Beach>, IEnumerable<ConciseBeachViewModel>>(beaches),
                Name = "World",
                TotalBeachesCount = beaches.Count
            };

            model.Beaches.Select(b => { b.UserHasRated = base.UserHasRated(b); return b; }).ToList();

            return this.View("_PlaceBeaches", model);
        }

        public ActionResult Statistics()
        {
            var beaches = this.Data.Continents.All()
                .Include(c => c.Countries)
                .Include(c => c.PrimaryDivisions.Select(pd => pd.WaterBody))
                .Include(c => c.SecondaryDivisions)
                .Include(c => c.TertiaryDivisions)
                .Include(c => c.QuaternaryDivisions)
                .Include(c => c.Beaches)
                .SelectMany(c => c.Beaches)
                .Where(b => b.TotalScore != null)
                .OrderByDescending(b => b.TotalScore)
                .ToList();
            var model = new StatisticsViewModel()
            {
                Id = 1,
                Controller = "World",
                Name = "World",
                Rows = Mapper.Map<IEnumerable<Beach>, IEnumerable<BeachRowViewModel>>(beaches),
                TotalBeachesCount = beaches.Count(),
            };

            return this.View("_StatisticsPartial", model);
        }

        public ActionResult FilteredStatistics(int id, string filterType)
        {
            var beaches = this.Data.Continents.All()
                .Include(c => c.Countries)
                .Include(c => c.PrimaryDivisions.Select(pd => pd.WaterBody))
                .Include(c => c.SecondaryDivisions)
                .Include(c => c.TertiaryDivisions)
                .Include(c => c.QuaternaryDivisions)
                .Include(c => c.Beaches)
                .SelectMany(c => c.Beaches)
                .Where(b => b.TotalScore != null)
                .OrderByDescending(b => b.TotalScore)
                .ToList();
            var model = new StatisticsViewModel()
            {
                Id = id,
                Controller = "World",
                Name = ((BeachFilterType)Enum.Parse(typeof(BeachFilterType), filterType)).GetDescription() + " World",
                Rows = Mapper.Map<IEnumerable<Beach>, IEnumerable<BeachRowViewModel>>(beaches),
                TotalBeachesCount = beaches.Count,
                FilterType = filterType
            };

            if (this.Request.IsAjaxRequest())
            {
                return this.Json(this.PartialView("_StatisticsPartial", model)
                    .RenderPartialViewAsString(), JsonRequestBehavior.AllowGet);
            }

            return this.View("_StatisticsPartial", model);
        }
    }
}