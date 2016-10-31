﻿namespace App.Controllers
{
    using AutoMapper;
    using BeachRankings.Data.UnitOfWork;
    using BeachRankings.Models;
    using BeachRankings.App.Models.ViewModels;
    using BeachRankings.App.Utils.Extensions;
    using System;
    using System.Data.Entity;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Web.Mvc;

    public class BeachesController : BaseController
    {
        public BeachesController(IBeachRankingsData data)
            : base(data)
        {
        }

        public ActionResult Details(int id)
        {
            var beach = this.Data.Beaches.All().FirstOrDefault(b => b.Id == id);
            var model = Mapper.Map<Beach, DetailedBeachViewModel>(beach);
            model.UserHasRated = this.UserProfile.Reviews.Any(r => r.BeachId == id);

            model.Reviews.OrderByDescending(r => r.PostedOn);

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
            var model = new AddBeachViewModel()
            {
                Countries = countries
            };

            return this.View(model);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(AddBeachViewModel bindingModel)
        {
            this.ValidateBindingModel(bindingModel);

            if (!this.ModelState.IsValid)
            {
                bindingModel.Countries = this.Data.Countries.All().Select(c => new SelectListItem()
                {
                    Text = c.Name,
                    Value = c.Id.ToString()
                });

                return this.View(bindingModel);
            }

            var beach = this.SaveBeach(bindingModel);

            this.SaveBeachImages(beach, bindingModel);

            return this.RedirectToAction("Post", "Reviews", new { id = beach.Id });
        }

        [Authorize]
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var beach = this.Data.Beaches.Find(id);
            var currentUserId = this.UserProfile.Id;

            if (!this.User.Identity.CanEditBeach(beach.CreatorId, beach.Reviews.Count))
            {
                return this.RedirectToAction("Details", new { id = id });
            }

            var model = Mapper.Map<Beach, EditBeachPristineViewModel>(beach);
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

            return this.View("EditPristine", model);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditPristine(EditBeachPristineViewModel model)
        {
            var beach = this.Data.Beaches.Find(model.Id);
            var currentUserId = this.UserProfile.Id;

            if (this.User.Identity.CanEditBeach(beach.CreatorId, beach.Reviews.Count))
            {
                this.UpdateBeach(model);
            }

            return this.RedirectToAction("Details", new { id = beach.Id });
        }

        [Authorize]
        [HttpPost]
        public ActionResult Delete(int id)
        {
            var beach = this.Data.Beaches.Find(id);
            var currentUserId = this.UserProfile.Id;

            if (!this.User.Identity.CanEditBeach(beach.CreatorId, beach.Reviews.Count))
            {
                return this.RedirectToAction("Details", new { id = id });
            }

            this.Data.Beaches.Remove(beach);
            this.Data.Beaches.SaveChanges();
            this.UpdateIndexEntries(beach);
            this.Data.Beaches.DeleteIndexEntry(beach);

            return this.RedirectToAction("Index", "Home");
        }

        #region Action Helpers

        #region Add

        private void ValidateBindingModel(AddBeachViewModel model)
        {
            var beachNameUnique = !this.Data.Beaches.All().Any(
                b => b.PrimaryDivisionId == model.PrimaryDivisionId &&
                b.SecondaryDivisionId == model.SecondaryDivisionId &&
                b.Name.ToLower() == model.Name.ToLower());
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
            }

            if (!beachNameUnique)
            {
                this.ModelState.AddModelError(string.Empty, "A beach with this name already exists.");
            }
        }

        private Beach SaveBeach(AddBeachViewModel model)
        {
            this.Data.Countries.All()
                .Include(c => c.PrimaryDivisions)
                .Include(c => c.SecondaryDivisions)
                .FirstOrDefault(c => c.Id == model.CountryId);

            var creatorId = this.UserProfile.Id;
            var primaryDivision = this.Data.PrimaryDivisions.Find(model.PrimaryDivisionId);
            var waterBody = this.Data.WaterBodies.Find(primaryDivision.WaterBodyId);
            var beach = Mapper.Map<AddBeachViewModel, Beach>(model);
            beach.CreatorId = creatorId;
            beach.WaterBodyId = primaryDivision.WaterBodyId;

            this.Data.Beaches.Add(beach);
            beach.SetBeachData();
            this.Data.Beaches.SaveChanges();

            this.UpdateIndexEntries(beach);

            return beach;
        }

        private void SaveBeachImages(Beach beach, AddBeachViewModel bindingModel)
        {
            if (bindingModel.Images == null || bindingModel.Images.ElementAt(0) == null)
            {
                return;
            }

            var formattedBeachName = Regex.Replace(beach.Name, @"[^A-Za-z]", string.Empty);
            var relativeBeachDir = Path.Combine("Uploads", "Images", "Beaches", formattedBeachName);
            var beachDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, relativeBeachDir);

            Directory.CreateDirectory(beachDir);

            foreach (var image in bindingModel.Images)
            {
                var uniqueName = Guid.NewGuid().ToString() + Path.GetFileName(image.FileName);
                var imagePath = Path.Combine(beachDir, uniqueName);
                var relativeImagePath = Path.Combine("\\", relativeBeachDir, uniqueName);
                var beachPhoto = new BeachImage()
                {
                    AuthorId = this.UserProfile.Id,
                    BeachId = beach.Id,
                    Name = uniqueName,
                    Path = relativeImagePath
                };

                image.SaveAs(imagePath);
                this.Data.BeachImages.Add(beachPhoto);
            }

            this.Data.BeachImages.SaveChanges();
        }

        #endregion

        #region Update

        private void UpdateBeach(EditBeachPristineViewModel model)
        {
            this.Data.Countries.All()
                .Include(c => c.PrimaryDivisions)
                .Include(c => c.SecondaryDivisions)
                .FirstOrDefault(c => c.Id == model.CountryId);

            var beach = this.Data.Beaches.Find(model.Id);
            var oldBeach = new Beach();

            Mapper.Map(beach, oldBeach);
            Mapper.Map(model, beach);

            var primaryDivision = this.Data.PrimaryDivisions.Find(model.PrimaryDivisionId);
            var waterBody = this.Data.WaterBodies.Find(primaryDivision.WaterBodyId);
            beach.WaterBodyId = primaryDivision.WaterBodyId;

            beach.SetBeachData();

            this.Data.Beaches.SaveChanges();

            this.UpdateIndexEntries(beach, oldBeach);
        }

        private void UpdateIndexEntries(Beach beach, Beach oldBeach = null)
        {
            if (oldBeach != null)
            {
                var oldCountry = this.Data.Countries.Find(oldBeach.CountryId);
                var oldPrimary = this.Data.PrimaryDivisions.Find(oldBeach.PrimaryDivisionId);
                var oldSecondary = this.Data.SecondaryDivisions.Find(oldBeach.SecondaryDivisionId);
                var oldTertiary = (oldBeach.TertiaryDivisionId == null) ? null : this.Data.TertiaryDivisions.Find(oldBeach.TertiaryDivisionId);
                var oldQuaternary = (oldBeach.QuaternaryDivisionId == null) ? null : this.Data.QuaternaryDivisions.Find(oldBeach.QuaternaryDivisionId);
                var oldWaterBody = this.Data.WaterBodies.Find(oldBeach.WaterBodyId);

                this.Data.Beaches.AddUpdateIndexEntry(oldBeach);
                this.Data.Countries.AddUpdateIndexEntry(oldCountry);
                this.Data.PrimaryDivisions.AddUpdateIndexEntry(oldPrimary);
                this.Data.SecondaryDivisions.AddUpdateIndexEntry(oldSecondary);
                this.Data.TertiaryDivisions.AddUpdateIndexEntry(oldTertiary);
                this.Data.QuaternaryDivisions.AddUpdateIndexEntry(oldQuaternary);
                this.Data.WaterBodies.AddUpdateIndexEntry(oldWaterBody);
            }

            var country = this.Data.Countries.Find(beach.CountryId);
            var primary = this.Data.PrimaryDivisions.Find(beach.PrimaryDivisionId);
            var secondary = this.Data.SecondaryDivisions.Find(beach.SecondaryDivisionId);
            var tertiary = (beach.TertiaryDivisionId == null) ? null : this.Data.TertiaryDivisions.Find(beach.TertiaryDivisionId);
            var quaternary = (beach.QuaternaryDivisionId == null) ? null : this.Data.QuaternaryDivisions.Find(beach.QuaternaryDivisionId);
            var waterBody = this.Data.WaterBodies.Find(primary.WaterBodyId);

            this.Data.Beaches.AddUpdateIndexEntry(beach);
            this.Data.Countries.AddUpdateIndexEntry(country);
            this.Data.PrimaryDivisions.AddUpdateIndexEntry(primary);
            this.Data.SecondaryDivisions.AddUpdateIndexEntry(secondary);
            this.Data.TertiaryDivisions.AddUpdateIndexEntry(tertiary);
            this.Data.QuaternaryDivisions.AddUpdateIndexEntry(quaternary);
            this.Data.WaterBodies.AddUpdateIndexEntry(waterBody);
        }

        #endregion

        #endregion
    }
}