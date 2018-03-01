namespace App.Controllers
{
    using AutoMapper;
    using BeachRankings.App.Models.ViewModels;
    using BeachRankings.Data.UnitOfWork;
    using BeachRankings.Extensions;
    using BeachRankings.Models;
    using Microsoft.AspNet.Identity;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Net;
    using System.Web.Mvc;

    public class WatchlistsController : BaseController
    {
        public WatchlistsController(IBeachRankingsData data)
            : base(data)
        {
        }

        [HttpGet]
        public ActionResult GetForUser(string userId)
        {
            var user = this.Data.Users.All()
                .Include(u => u.Watchlists)
                .FirstOrDefault(u => u.Id == userId);
            var model = Mapper.Map<User, WatchlistsTableViewModel>(user);

            return this.View("UserWatchlists", model);
        }

        [HttpGet]
        public ActionResult GetWatchlist(int id)
        {
            var watchlist = this.Data.Watchlists.Find(id);
            var beaches = watchlist.Beaches;
            var model = new WatchlistStatisticsViewModel
            {
                Id = id,
                Controller = "Watchlists",
                Name = $"Watchlist {watchlist.Name}",
                Rows = Mapper.Map<IEnumerable<Beach>, IEnumerable<BeachRowViewModel>>(beaches),
                TotalBeachesCount = beaches.Count
            };

            return this.View("_StatisticsPartial", model);
        }

        [HttpGet]
        public ActionResult GetWatchlistCriteriaTables(int id)
        {
            var watchlist = this.Data.Watchlists.All()
                .Include(w => w.Beaches)
                .FirstOrDefault(w => w.Id == id);
            var beaches = watchlist.Beaches.OrderByDescending(b => b.TotalScore).ToList();
            var model = new WatchlistCriteriaTablesViewModel
            {
                Id = id,
                Name = $"Watchlist {watchlist.Name}",
                CriteriaTableRows = Mapper.Map<ICollection<Beach>, ICollection<HorizontalCriteriaViewModel>>(beaches)
            };

            return this.View("WatchlistCriteriaTables", model);
        }

        public JsonResult ExportHtml(int id)
        {
            var watchlist = this.Data.Watchlists.All()
                .Include(w => w.Beaches)
                .FirstOrDefault(w => w.Id == id);
            var beaches = watchlist.Beaches.OrderByDescending(b => b.TotalScore).ToList();
            var model = Mapper.Map<ICollection<Beach>, ICollection<ExportScoresAsHtmlViewModel>> (beaches);
            var horizontalHtmlResult = this.PartialView(
                @"~\Views\Shared\_ExportScoresHorizontalHtml.cshtml", model).RenderPartialViewAsString();

            return this.Json(horizontalHtmlResult, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        [HttpGet]
        public ActionResult UserWatchlistsByBeachId(int beachId)
        {
            var userWatchlists = this.Data.Watchlists.All().Where(w => w.OwnerId == this.UserProfile.Id).ToList();
            var model = new ConciseWatchlistViewModel
            {
                BeachId = beachId,
                Watchlists = Mapper.Map<ICollection<Watchlist>, ICollection<BeachWatchlistViewModel>>(
                    userWatchlists).ToList()
            };
            model.Watchlists = model.Watchlists.Select(m =>
            {
                var current = userWatchlists.First(uw => uw.Id == m.Id);
                m.BeachPresentInWatchlist = current.Beaches.Any(b => b.Id == beachId);
                m.BeachId = beachId;

                return m;
            }).ToList();

            return this.PartialView("_ConciseWatchlistsPartial", model);
        }

        [Authorize]
        [HttpPost]
        public ActionResult AdjustWatchlists(ConciseWatchlistViewModel viewModel)
        {
            var adjustedWatchlists = viewModel.Watchlists;
            var adjustedWatchlistIds = adjustedWatchlists.Select(aw => aw.Id).ToList();
            var watchlistsToAdjust = this.Data.Watchlists.All()
                .Include(w => w.Beaches)
                .Where(w => adjustedWatchlistIds.Contains(w.Id))
                .ToList();

            foreach (var oldList in watchlistsToAdjust)
            {
                var newList = adjustedWatchlists.First(aw => aw.Id == oldList.Id);

                if (newList.BeachPresentInWatchlist && oldList.Beaches.All(b => b.Id != newList.BeachId))
                {
                    var beach = this.Data.Beaches.Find(newList.BeachId);

                    if (beach != null)
                    {
                        oldList.Beaches.Add(beach);
                    }
                }
                else if (!newList.BeachPresentInWatchlist && oldList.Beaches.Any(b => b.Id == newList.BeachId))
                {
                    oldList.Beaches.Remove(this.Data.Beaches.Find(newList.BeachId));
                }
            }

            this.Data.Watchlists.SaveChanges();

            return Json("Watchlist updated.", JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        [HttpPost]
        public ActionResult Add(AddEditWatchlistViewModel viewModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.Json(this.ModelState.Values.Select(v => v.Errors.Select(e => e.ErrorMessage)));
            }

            var watchlist = Mapper.Map<AddEditWatchlistViewModel, Watchlist>(viewModel);
            watchlist.OwnerId = this.User.Identity.GetUserId();

            this.Data.Watchlists.Add(watchlist);
            this.Data.Watchlists.SaveChanges();

            var model = Mapper.Map<Watchlist, BeachWatchlistViewModel>(watchlist);
            var watchlistRowAsHtml = this.PartialView(
                @"~\Views\Shared\DisplayTemplates\BeachWatchlistViewModel.cshtml", model)
                .RenderPartialViewAsString();

            return this.Json(watchlistRowAsHtml, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        [HttpPost]
        public ActionResult Edit(AddEditWatchlistViewModel viewModel)
        {
            if (!this.ModelState.IsValid)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var watchlist = this.Data.Watchlists.Find(viewModel.Id);

            if (!this.User.Identity.CanEditWatchlist(watchlist.OwnerId))
            {
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            }

            watchlist.Name = viewModel.Name;

            this.Data.Watchlists.SaveChanges();

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        [Authorize]
        [HttpPost]
        public ActionResult Delete(int id)
        {
            var watchlist = this.Data.Watchlists.Find(id);

            if (!this.User.Identity.CanEditWatchlist(watchlist.OwnerId))
            {
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            }

            this.Data.Watchlists.Remove(watchlist);
            this.Data.Watchlists.SaveChanges();

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }
    }
}