namespace App.Controllers
{
    using App.Code.Beaches;
    using App.Code.Users;
    using App.Code.Web;
    using AutoMapper;
    using BeachRankings.App.Models.ViewModels;
    using BeachRankings.Data.UnitOfWork;
    using BeachRankings.Models;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    [Authorize(Roles = "Admin")]
    public class AdminController : BaseController
    {
        private IBeachUpdater beachUpdater;
        private IUserLevelCalculator userLevelCalculator;
        private ISitemapGenerator sitemapGenerator;

        public AdminController(
            IBeachRankingsData data, 
            IBeachUpdater beachUpdater,
            IUserLevelCalculator userLevelCalculator,
            ISitemapGenerator sitemapGenerator)
            : base(data)
        {
            this.beachUpdater = beachUpdater;
            this.userLevelCalculator = userLevelCalculator;
            this.sitemapGenerator = sitemapGenerator;
        }

        [HttpGet]
        public ActionResult Restructure()
        {
            var continents = this.Data.Continents.All().Select(c => new SelectListItem()
            {
                Text = c.Name,
                Value = c.Id.ToString()
            });
            var countries = this.Data.Countries.All().Select(c => new SelectListItem()
            {
                Text = c.Name,
                Value = c.Id.ToString()
            });
            var waterBodies = this.Data.WaterBodies.All().Select(wb => new SelectListItem()
            {
                Text = wb.Name,
                Value = wb.Id.ToString()
            });
            var model = new RestructureViewModel()
            {
                Continents = continents,
                Countries = countries,
                WaterBodies = waterBodies
            };

            if (this.TempData["ValidationError"] != null)
            {
                this.ModelState.AddModelError(string.Empty, this.TempData["ValidationError"].ToString());
            }

            return this.View(model);
        }

        [HttpGet]
        public ActionResult Weights()
        {
            var viewModel = Mapper.Map<ICollection<ScoreWeight>, ICollection<WeightViewModel>>
                (this.Data.ScoreWeights.All().ToList());

            return this.View(viewModel);
        }

        [HttpPost]
        public ActionResult Weights(ICollection<WeightViewModel> bindingModel)
        {
            var weights = this.Data.ScoreWeights.All().ToList();
            var changesMade = false;

            for (int w = 0; w < weights.Count; w++)
            {
                var bindingWeight = bindingModel.First(bm => bm.Id == weights[w].Id);
                var oldWeight = Mapper.Map<ScoreWeight, ScoreWeight>(weights[w]);

                Mapper.Map(bindingWeight, weights[w]);

                if (oldWeight.Value != weights[w].Value)
                {
                    changesMade = true;
                }
            }

            if (changesMade)
            {
                this.Data.ScoreWeights.SaveChanges();

                this.userLevelCalculator.RecalculateUserLevels();

                this.beachUpdater.UpdateBeachScores();
            }

            return this.RedirectToAction("Weights");
        }

        [HttpGet]
        public ActionResult ControlPanel()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RecalculateUserLevelsAndBeachScores()
        {
            this.userLevelCalculator.RecalculateUserLevels();

            this.beachUpdater.UpdateBeachScores();

            return this.RedirectToAction("ControlPanel");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RegenerateSitemap()
        {
            this.sitemapGenerator.GenerateSitemap();

            return this.RedirectToAction("ControlPanel");
        }
    }
}