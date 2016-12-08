namespace App.Controllers
{
    using AutoMapper;
    using BeachRankings.App.CustomAttributes;
    using BeachRankings.Data.Extensions;
    using BeachRankings.Data.UnitOfWork;
    using BeachRankings.Models;
    using BeachRankings.App.Models.ViewModels;
    using BeachRankings.App.Utils;
    using BeachRankings.App.Utils.Extensions;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Web.Mvc;

    public class BeachesController : BasePlacesController
    {
        public BeachesController(IBeachRankingsData data)
            : base(data)
        {
        }

        public ActionResult Best()
        {
            var model = new BestBeachesViewModel()
            {
                CaribbeanId = 40,
                EuropeId = 4,
                FranceId = 53,
                GreeceId = 61,
                ItalyId = 78,
                MediterraneanId = 108,
                SpainId = 147,
                TurkeyId = 160,
                CampingId = 13,
                LongStayId = 15,
                SnorkelingId = 11
            };

            return this.View(model);
        }

        public ActionResult Top(int continentId = 0, int countryId = 0, int waterBodyId = 0, int criterionId = 0)
        {
            var beaches = this.Data.Beaches
                 .FilterByContinent(continentId)
                 .FilterByCountry(countryId)
                 .FilterByWaterBody(waterBodyId)
                 .OrderByDescending(b => b.TotalScore)
                 .OrderByCriterion(criterionId)
                 .Take(25)
                 .ToList();
            var beachesModel = Mapper.Map<IEnumerable<Beach>, IEnumerable<ConciseBeachViewModel>>(beaches);
            var title = BeachHelper.GetFilteredBeachesTitle(continentId, countryId, waterBodyId, criterionId);
            var model = new PlaceBeachesViewModel() { Name = title, Beaches = beachesModel };

            model.Beaches.Select(b => { b.UserHasRated = base.UserHasRated(b); return b; }).ToList();

            return this.View("_PlaceBeaches", model);
        }

        public ActionResult Details(int id)
        {
            var beach = this.Data.Beaches.All()
                .Include(b => b.Reviews)
                .Include(b => b.Images)
                .Include(b => b.BlogArticles)
                .FirstOrDefault(b => b.Id == id);
            var model = Mapper.Map<Beach, DetailedBeachViewModel>(beach);
            model.Reviews = model.Reviews.OrderByDescending(r => r.Upvotes).ThenByDescending(r => r.PostedOn).ToList();

            if (this.User.Identity.IsAuthenticated)
            {
                Func<ConciseReviewViewModel, bool> userUpvoted = (r => (this.UserProfile.UpvotedReviews.Any(ur => ur.Id == r.Id)));
                model.UserHasRated = this.UserProfile.Reviews.Any(r => r.BeachId == id);
                model.Reviews.Select(r => { r.AlreadyUpvoted = userUpvoted(r); return r; }).ToList();
            }

            return this.View(model);
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
            if (!this.AddEditModelValid(bindingModel))
            {
                bindingModel.Countries = this.Data.Countries.All().Select(c => new SelectListItem()
                {
                    Text = c.Name,
                    Value = c.Id.ToString()
                });

                return this.View(bindingModel);
            }

            var beach = this.SaveBeach(bindingModel);
            var images = ImageHelper.PersistBeachImages(beach, bindingModel.Images, this.UserProfile.Id);

            this.Data.BeachImages.AddMany(images);
            this.Data.BeachImages.SaveChanges();

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

            var model = this.GetEditBeachViewModel(beach);

            return this.View("Edit", model);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EditBeachViewModel bindingModel)
        {
            var beach = this.Data.Beaches.Find(bindingModel.Id);

            if (this.User.Identity.CanEditBeach(beach.CreatorId, beach.Reviews.Count))
            {
                if (!this.AddEditModelValid(bindingModel))
                {
                    var model = this.GetEditBeachViewModel(beach);
                    bindingModel.Countries = model.Countries;
                    bindingModel.PrimaryDivisions = model.PrimaryDivisions;
                    bindingModel.SecondaryDivisions = model.SecondaryDivisions;
                    bindingModel.TertiaryDivisions = model.TertiaryDivisions;
                    bindingModel.QuaternaryDivisions = model.QuaternaryDivisions;

                    return this.View(bindingModel);
                }

                this.UpdateBeach(beach, bindingModel);
            }

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

            this.RemoveChildReferences(beach);
            this.Data.Beaches.Remove(beach);
            this.Data.Beaches.SaveChanges();
            this.UpdateIndexEntries(oldBeach);
            this.Data.Beaches.DeleteIndexEntry(oldBeach);

            return this.RedirectToAction("Index", "Home");
        }

        public JsonResult ExportHtml(int id)
        {
            var beach = this.Data.Beaches.Find(id);
            var model = Mapper.Map<Beach, ExportScoresAsHtmlViewModel>(beach);
            var htmlResult = this.PartialView(@"~\Views\Shared\_ExportScoresHtml.cshtml", model).RenderPartialViewAsString();

            return this.Json(htmlResult, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [RestructureAuthorize]
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
                beach.WaterBodyId = this.GetWaterBodyId((int)bindingModel.CountryId, bindingModel.PrimaryDivisionId, bindingModel.SecondaryDivisionId);

                this.Data.Beaches.SaveChanges();

                beach.SetBeachData();

                this.Data.Beaches.SaveChanges();

                this.UpdateIndexEntries(beach, oldBeach);
            }

            return this.RedirectToAction("Restructure", "Admin");
        }

        #region Action Helpers

        #region Common

        private bool AddEditModelValid(IAddEditBeachModel model)
        {
            var beachNameUnique = !this.Data.Beaches.All().Any(
                b => b.PrimaryDivisionId == model.PrimaryDivisionId &&
                b.SecondaryDivisionId == model.SecondaryDivisionId &&
                b.Name.ToLower() == model.Name.ToLower());
            var englishAlphabetUsed = Regex.IsMatch(model.Name, @"^[A-Za-z0-9\s-]+$");
            var primaryDivision = this.Data.PrimaryDivisions.All()
                .Include(pd => pd.SecondaryDivisions)
                .Include(pd => pd.TertiaryDivisions)
                .Include(pd => pd.QuaternaryDivisions)
                .FirstOrDefault(pd => pd.Id == model.PrimaryDivisionId);
            var secondaryDivisionsExist = (primaryDivision.SecondaryDivisions.Count > 0);
            var tertiaryDivisionsExist = (primaryDivision.TertiaryDivisions.Count > 0);
            var quaternaryDivisionsExist = (primaryDivision.QuaternaryDivisions.Count > 0);
            var secondaryIdMissing = (secondaryDivisionsExist && model.SecondaryDivisionId == null);
            var tertiaryIdMissing = (tertiaryDivisionsExist && model.TertiaryDivisionId == null);
            var quaternaryIdMissing = (quaternaryDivisionsExist && model.QuaternaryDivisionId == null);

            if (secondaryIdMissing || tertiaryIdMissing || quaternaryIdMissing)
            {
                this.ModelState.AddModelError(string.Empty, "All location fields are required.");

                return false;
            }

            if (!beachNameUnique)
            {
                this.ModelState.AddModelError(string.Empty, "A beach with this name already exists.");

                return false;
            }

            return true;
        }

        #endregion

        #region Add        

        private Beach SaveBeach(AddBeachViewModel model)
        {
            this.Data.Countries.All()
                .Include(c => c.PrimaryDivisions)
                .Include(c => c.SecondaryDivisions)
                .FirstOrDefault(c => c.Id == model.CountryId);

            var beach = Mapper.Map<AddBeachViewModel, Beach>(model);
            beach.CreatorId = this.UserProfile.Id;
            beach.WaterBodyId = this.GetWaterBodyId(model.CountryId, model.PrimaryDivisionId, model.SecondaryDivisionId);

            this.Data.Beaches.Add(beach);
            beach.SetBeachData();
            this.Data.Beaches.SaveChanges();

            this.UpdateIndexEntries(beach);

            return beach;
        }

        private int GetWaterBodyId(int countryId, int? primaryDivisionid, int? secondaryDivisionId)
        {
            var secondaryDivision = this.Data.SecondaryDivisions.All().Include(sd => sd.WaterBody).FirstOrDefault(sd => sd.Id == secondaryDivisionId);
            var secondaryDivisionHasWaterBody = (secondaryDivision != null && secondaryDivision.WaterBodyId != null);

            if (secondaryDivisionHasWaterBody)
            {
                return (int)secondaryDivision.WaterBodyId;
            }

            var primaryDivision = this.Data.PrimaryDivisions.All().Include(pd => pd.WaterBody).FirstOrDefault(pd => pd.Id == primaryDivisionid);
            var primaryDivisionHasWaterBody = (primaryDivision != null && primaryDivision.WaterBodyId != null);

            if (primaryDivisionHasWaterBody)
            {
                return (int)primaryDivision.WaterBodyId;
            }

            var waterBodyId = this.Data.Countries.All().Include(c => c.WaterBody).FirstOrDefault(c => c.Id == countryId).WaterBodyId;

            return (int)waterBodyId;
        }

        #endregion

        #region Edit

        private EditBeachViewModel GetEditBeachViewModel(Beach beach)
        {
            var model = Mapper.Map<Beach, EditBeachViewModel>(beach);
            model.Countries = this.Data.Countries.All().Select(c => new SelectListItem()
            {
                Text = c.Name,
                Value = c.Id.ToString(),
                Selected = (c.Id == model.CountryId)
            });
            model.PrimaryDivisions = this.Data.PrimaryDivisions.All()
                .Where(pd => pd.CountryId == beach.CountryId)
                .Select(c => new SelectListItem()
                {
                    Text = c.Name,
                    Value = c.Id.ToString(),
                    Selected = (c.Id == model.PrimaryDivisionId)
                });
            model.SecondaryDivisions = this.Data.SecondaryDivisions.All()
                .Where(sd => sd.PrimaryDivisionId == beach.PrimaryDivisionId)
                .Select(c => new SelectListItem()
                {
                    Text = c.Name,
                    Value = c.Id.ToString(),
                    Selected = (c.Id == model.SecondaryDivisionId)
                });
            model.TertiaryDivisions = this.Data.TertiaryDivisions.All()
                .Where(td => td.SecondaryDivisionId == beach.SecondaryDivisionId)
                .Select(c => new SelectListItem()
                {
                    Text = c.Name,
                    Value = c.Id.ToString(),
                    Selected = (c.Id == model.TertiaryDivisionId)
                });
            model.QuaternaryDivisions = this.Data.QuaternaryDivisions.All()
                .Where(qd => qd.TertiaryDivisionId == beach.TertiaryDivisionId)
                .Select(c => new SelectListItem()
                {
                    Text = c.Name,
                    Value = c.Id.ToString(),
                    Selected = (c.Id == model.QuaternaryDivisionId)
                });

            return model;
        }

        private void UpdateBeach(Beach beach, EditBeachViewModel model)
        {
            var oldBeach = new Beach();

            Mapper.Map(beach, oldBeach);
            Mapper.Map(model, beach);

            var primaryDivision = this.Data.PrimaryDivisions.All().Include(pd => pd.WaterBody).FirstOrDefault(pd => pd.Id == model.PrimaryDivisionId);
            beach.WaterBodyId = this.GetWaterBodyId(model.CountryId, model.PrimaryDivisionId, model.SecondaryDivisionId);

            this.Data.Beaches.SaveChanges();

            beach.SetBeachData();

            this.Data.Beaches.SaveChanges();

            this.UpdateIndexEntries(beach, oldBeach);
        }

        private void UpdateIndexEntries(Beach beach, Beach oldBeach = null)
        {
            if (oldBeach != null)
            {
                var oldContinent = this.Data.Continents.Find(oldBeach.ContinentId);
                var oldCountry = this.Data.Countries.Find(oldBeach.CountryId);
                var oldPrimary = this.Data.PrimaryDivisions.Find(oldBeach.PrimaryDivisionId);
                var oldSecondary = this.Data.SecondaryDivisions.Find(oldBeach.SecondaryDivisionId);
                var oldTertiary = (oldBeach.TertiaryDivisionId == null) ? null : this.Data.TertiaryDivisions.Find(oldBeach.TertiaryDivisionId);
                var oldQuaternary = (oldBeach.QuaternaryDivisionId == null) ? null : this.Data.QuaternaryDivisions.Find(oldBeach.QuaternaryDivisionId);
                var oldWaterBody = this.Data.WaterBodies.Find(oldBeach.WaterBodyId);

                this.Data.Beaches.AddUpdateIndexEntry(oldBeach);
                this.Data.Continents.AddUpdateIndexEntry(oldContinent);
                this.Data.Countries.AddUpdateIndexEntry(oldCountry);
                this.Data.PrimaryDivisions.AddUpdateIndexEntry(oldPrimary);
                this.Data.SecondaryDivisions.AddUpdateIndexEntry(oldSecondary);
                this.Data.TertiaryDivisions.AddUpdateIndexEntry(oldTertiary);
                this.Data.QuaternaryDivisions.AddUpdateIndexEntry(oldQuaternary);
                this.Data.WaterBodies.AddUpdateIndexEntry(oldWaterBody);
            }

            var continent = this.Data.Continents.Find(beach.ContinentId);
            var country = this.Data.Countries.Find(beach.CountryId);
            var primary = this.Data.PrimaryDivisions.Find(beach.PrimaryDivisionId);
            var secondary = this.Data.SecondaryDivisions.Find(beach.SecondaryDivisionId);
            var tertiary = (beach.TertiaryDivisionId == null) ? null : this.Data.TertiaryDivisions.Find(beach.TertiaryDivisionId);
            var quaternary = (beach.QuaternaryDivisionId == null) ? null : this.Data.QuaternaryDivisions.Find(beach.QuaternaryDivisionId);
            var waterBodyId = (secondary == null) ?
                (primary == null) ? country.WaterBodyId : primary.WaterBodyId
                : secondary.WaterBodyId;
            var waterBody = this.Data.WaterBodies.Find(waterBodyId);

            this.Data.Beaches.AddUpdateIndexEntry(beach);
            this.Data.Continents.AddUpdateIndexEntry(continent);
            this.Data.Countries.AddUpdateIndexEntry(country);
            this.Data.PrimaryDivisions.AddUpdateIndexEntry(primary);
            this.Data.SecondaryDivisions.AddUpdateIndexEntry(secondary);
            this.Data.TertiaryDivisions.AddUpdateIndexEntry(tertiary);
            this.Data.QuaternaryDivisions.AddUpdateIndexEntry(quaternary);
            this.Data.WaterBodies.AddUpdateIndexEntry(waterBody);
        }

        #endregion

        #region Delete

        private void RemoveChildReferences(Beach beach)
        {
            var reviews = beach.Reviews.ToList();
            var images = beach.Images.ToList();

            this.Data.Reviews.RemoveMany(reviews);
            this.Data.BeachImages.RemoveMany(images);
            ImageHelper.EraseImagesLocally(beach.Name);

            this.Data.Reviews.SaveChanges();
            this.Data.BeachImages.SaveChanges();
        }

        #endregion

        #endregion
    }
}