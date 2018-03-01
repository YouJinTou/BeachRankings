namespace App.Controllers
{
    using App.Code.Beaches;
    using App.Code.Blogs;
    using App.Code.Images;
    using AutoMapper;
    using BeachRankings.App.CustomAttributes;
    using BeachRankings.App.Models.ViewModels;
    using BeachRankings.Data.UnitOfWork;
    using BeachRankings.Extensions;
    using BeachRankings.Models;
    using BeachRankings.Services.Aggregation;
    using Extensions;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Web.Mvc;

    public class BeachesController : BasePlacesController
    {
        private IBeachUpdater beachUpdater;
        private IBeachQueryManager beachQueryManager;
        private IBeachDisplayManager beachDisplayManager;
        private IImageManager imageManager;
        private IBlogArticleUpdater blogArticleUpdater;

        public BeachesController(
            IBeachRankingsData data, 
            IBeachUpdater beachUpdater, 
            IBeachQueryManager beachQueryManager, 
            IBeachDisplayManager beachDisplayManager,
            IImageManager imageManager,
            IBlogArticleUpdater blogArticleUpdater,
            IDataAggregationService aggregationService)
            : base(data, aggregationService)
        {
            this.beachUpdater = beachUpdater;
            this.beachQueryManager = beachQueryManager;
            this.beachDisplayManager = beachDisplayManager;
            this.imageManager = imageManager;
            this.blogArticleUpdater = blogArticleUpdater;
        }

        public ActionResult Statistics(int continentId = 0, int countryId = 0, int waterBodyId = 0, int criterionId = 0)
        {
            var beaches = this.Data.Beaches
                 .FilterByContinent(continentId)
                 .FilterByCountry(countryId)
                 .FilterByWaterBody(waterBodyId)
                 .OrderByDescending(b => b.TotalScore)
                 .OrderByCriterion(criterionId)
                 .Take(25)
                 .ToList();
            var model = new StatisticsViewModel()
            {
                Id = this.beachDisplayManager.GetPlaceId(continentId, countryId, waterBodyId),
                Controller = this.beachDisplayManager.GetControllerNameFromId(continentId, countryId, waterBodyId),
                Name = this.beachDisplayManager.GetFilteredBeachesTitle(continentId, countryId, waterBodyId, criterionId),
                Rows = Mapper.Map<IEnumerable<Beach>, IEnumerable<BeachRowViewModel>>(beaches),
                SortingCriterion = criterionId
            };

            return this.View("_StatisticsPartial", model);
        }

        public ActionResult Details(int id)
        {
            var beach = this.Data.Beaches.All()
                .Include(b => b.Reviews)
                .Include(b => b.Images)
                .Include(b => b.BlogArticles)
                .FirstOrDefault(b => b.Id == id);
            var model = Mapper.Map<Beach, DetailedBeachViewModel>(beach);
            model.Reviews = model.Reviews
                .OrderByDescending(r => r.ReviewUser.Badge)
                .ThenByDescending(r => r.Upvotes)
                .ThenByDescending(r => r.PostedOn)
                .Take(10)
                .ToList();
            model.CrossTable.Rows = Mapper.Map<ICollection<RankContainer>, IEnumerable<CrossTableRowViewModel>>(
                this.aggregationService.CalculateBeachRanks(id));

            if (this.User.Identity.IsAuthenticated)
            {
                Func<ConciseReviewViewModel, bool> userUpvoted = (r => (this.Data.UpvotedReviews.UserHasVotedForReview(this.UserProfile.Id, r.Id)));
                model.UserHasRated = this.UserProfile.Reviews.Any(r => r.BeachId == id);
                model.Reviews.Select(r => { r.ReviewHead.AlreadyUpvoted = userUpvoted(r); return r; }).ToList();
            }

            return this.View(model);
        }

        public PartialViewResult Reviews(int beachId, int page = 0, int pageSize = 10)
        {
            var beach = this.Data.Beaches.All().Include(b => b.Reviews).FirstOrDefault(b => b.Id == beachId);
            var model = Mapper.Map<IEnumerable<Review>, IEnumerable<ConciseReviewViewModel>>
                (beach.Reviews.OrderByDescending(r => r.Upvotes).ThenByDescending(r => r.PostedOn));
            model = model.Skip(page * pageSize).Take(pageSize).ToList();

            return this.PartialView("BeachReviewsPartial", model);
        }

        [Authorize]
        [HttpGet]
        public ActionResult Add()
        {
            var countries = this.Data.Countries.All().Select(c => new SelectListItem()
            {
                Text = c.Name,
                Value = c.Id.ToString()
            });
            var model = new AddBeachViewModel() { Countries = countries };

            return this.View(model);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(AddBeachViewModel bindingModel)
        {
            if (!this.ModelState.IsValid)
            {
                bindingModel.Countries = this.Data.Countries.All().Select(c => new SelectListItem()
                {
                    Text = c.Name,
                    Value = c.Id.ToString()
                });

                return this.View(bindingModel);
            }

            var beach = this.beachUpdater.SaveBeach(bindingModel, this.UserProfile.Id);

            this.imageManager.PersistBeachImages(beach, beach.Name, bindingModel.Images, this.UserProfile.Id);

            bindingModel.Id = beach.Id;

            this.blogArticleUpdater.TryAddUpdateBlogArticles(this.User.Identity.IsAdmin(), bindingModel);

            return this.RedirectToAction("Post", "Reviews", new { id = beach.Id });
        }

        [Authorize]
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var beach = this.Data.Beaches.Find(id);

            if (!this.User.Identity.CanEditBeach(beach.CreatorId, beach.Reviews.Count))
            {
                return this.RedirectToAction("Details", new { id = id });
            }

            var model = this.beachDisplayManager.GetEditBeachViewModel(beach);

            return this.View("Edit", model);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EditBeachViewModel bindingModel)
        {
            var beach = this.Data.Beaches.Find(bindingModel.Id);

            if (!this.User.Identity.CanEditBeach(beach.CreatorId, beach.Reviews.Count))
            {
                return this.RedirectToAction("Details", new { id = beach.Id });
            }

            if (!this.ModelState.IsValid)
            {
                var model = this.beachDisplayManager.GetEditBeachViewModel(beach);
                bindingModel.Countries = model.Countries;
                bindingModel.PrimaryDivisions = model.PrimaryDivisions;
                bindingModel.SecondaryDivisions = model.SecondaryDivisions;
                bindingModel.TertiaryDivisions = model.TertiaryDivisions;
                bindingModel.QuaternaryDivisions = model.QuaternaryDivisions;

                return this.View(bindingModel);
            }

            var oldName = beach.Name;

            this.beachUpdater.UpdateBeach(beach, bindingModel, this.User.Identity.IsAdmin());

            this.imageManager.PersistBeachImages(beach, oldName, bindingModel.Images, this.UserProfile.Id);

            return this.RedirectToAction("Details", new { id = beach.Id });
        }

        [Authorize]
        [HttpPost]
        public ActionResult Delete(int id)
        {
            var beach = this.Data.Beaches.Find(id);

            if (!this.User.Identity.CanEditBeach(beach.CreatorId, beach.Reviews.Count))
            {
                return this.RedirectToAction("Details", new { id = id });
            }

            var oldBeach = new Beach();

            Mapper.Map(beach, oldBeach);

            this.beachUpdater.RemoveChildReferences(beach);
            this.Data.Beaches.Remove(beach);
            this.Data.Beaches.SaveChanges();
            this.beachUpdater.UpdateIndexEntries(oldBeach);
            this.Data.Beaches.DeleteIndexEntry(oldBeach);

            return this.RedirectToAction("Index", "Home");
        }

        public JsonResult ExportHtml(int id)
        {
            var beach = this.Data.Beaches.Find(id);
            var model = Mapper.Map<Beach, ExportScoresAsHtmlViewModel>(beach);
            var horizontalHtmlResult = this.PartialView(
                @"~\Views\Shared\_ExportScoresHorizontalHtml.cshtml", 
                new List<ExportScoresAsHtmlViewModel> { model }).RenderPartialViewAsString();
            var verticalHtmlResult = this.PartialView(
                @"~\Views\Shared\_ExportScoresVerticalHtml.cshtml", model).RenderPartialViewAsString();
            var htmlResult = horizontalHtmlResult + "@@@" + verticalHtmlResult;

            return this.Json(htmlResult, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [RestructureAuthorized]
        public ActionResult MoveBeaches(RestructureViewModel bindingModel)
        {
            var beachIds = bindingModel.BeachIdsToMove.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var beachId in beachIds)
            {
                var beach = this.Data.Beaches.Find(int.Parse(beachId));
                var oldBeach = new Beach();

                Mapper.Map(beach, oldBeach);

                beach.ContinentId = bindingModel.ContinentId;
                beach.CountryId = (int)bindingModel.CountryId;
                beach.PrimaryDivisionId = bindingModel.PrimaryDivisionId;
                beach.SecondaryDivisionId = bindingModel.SecondaryDivisionId;
                beach.TertiaryDivisionId = bindingModel.TertiaryDivisionId;
                beach.QuaternaryDivisionId = bindingModel.QuaternaryDivisionId;
                beach.WaterBodyId = this.beachQueryManager
                    .GetBeachWaterBodyId((int)bindingModel.CountryId, bindingModel.PrimaryDivisionId, bindingModel.SecondaryDivisionId);

                this.Data.Beaches.SaveChanges();

                beach.SetBeachData();

                this.Data.Beaches.SaveChanges();

                this.beachUpdater.UpdateIndexEntries(beach, oldBeach);
            }

            return this.RedirectToAction("Restructure", "Admin");
        }
    }
}