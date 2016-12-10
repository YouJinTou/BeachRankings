namespace App.Controllers
{
    using App.Controllers.Enums;
    using AutoMapper;
    using BeachRankings.App.Models.ViewModels;
    using BeachRankings.App.Utils;
    using BeachRankings.App.Utils.Extensions;
    using BeachRankings.Data.UnitOfWork;
    using BeachRankings.Models;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.IO;
    using System.Linq;
    using System.Web.Mvc;

    [Authorize]
    public class UserController : BaseController
    {
        public UserController(IBeachRankingsData data)
            : base(data)
        {
        }

        public ActionResult Statistics()
        {
            var reviews = this.UserProfile.Reviews;
            var model = Mapper.Map<IEnumerable<Review>, IEnumerable<ReviewRowViewModel>>(reviews);

            return this.View(model);
        }

        public ActionResult Reviews(string authorId)
        {
            var user = this.Data.Users.Find(authorId);
            var model = Mapper.Map<User, TableUserReviewsViewModel>(user);

            return this.View(model);
        }

        public ActionResult Images(int page = 0, int pageSize = 10)
        {
            var imageGroups = this.Data.BeachImages.All().Where(i => i.AuthorId == this.UserProfile.Id).GroupBy(i => i.Beach.Name);
            var model = new List<DashboardImageViewModel>();

            foreach (var group in imageGroups)
            {
                model.Add(new DashboardImageViewModel()
                {
                    BeachName = group.Key,
                    Paths = Mapper.Map<IEnumerable<BeachImage>, IEnumerable<DashboardBeachImageThumbnailViewModel>>(group.ToList())
                });
            }

            model = model.Skip(page * pageSize).Take(pageSize).ToList();

            return this.View(model);
        }

        [HttpGet]
        public PartialViewResult ChangeAvatar()
        {
            var model = new ChangeAvatarViewModel() { AvatarPath = this.UserProfile.AvatarPath };

            return this.PartialView(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangeAvatar(ChangeAvatarViewModel bindingModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.RedirectToAction("Index", "Manage", new { Message = ActionMessage.UploadAvatarError });
            }

            this.SaveUserAvatar(bindingModel);

            return this.RedirectToAction("Index", "Manage", new { Message = ActionMessage.ChangeAvatarSuccess });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteAvatar()
        {
            if (this.DeleteOldAvatar())
            {
                this.SetDefaultAvatar();
            }

            return this.RedirectToAction("Index", "Manage", new { Message = ActionMessage.DeleteAvatarSuccess });
        }

        [HttpPost]
        public ActionResult DeleteBeachImage(int id)
        {
            var image = this.Data.BeachImages.All().Include(i => i.Beach).FirstOrDefault(i => i.Id == id);

            if (image == null)
            {
                return new HttpStatusCodeResult(404);
            }

            if (!this.User.Identity.CanDeleteBeachImage(image.AuthorId))
            {
                return new HttpStatusCodeResult(401);
            }

            ImageHelper.EraseBeachImageLocally(image.Beach.Name, image.Name);
            this.Data.BeachImages.Remove(image);
            this.Data.BeachImages.SaveChanges();

            return new HttpStatusCodeResult(200);
        }

        #region Helpers

        private void SaveUserAvatar(ChangeAvatarViewModel bindingModel)
        {
            if (bindingModel.Avatar == null)
            {
                return;
            }

            this.DeleteOldAvatar();

            var relativeAvatarsDir = UserHelper.GetUserAvatarsRelativeDir();
            var avatarsDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, relativeAvatarsDir);
            var uniqueName = Guid.NewGuid().ToString() + Path.GetFileName(bindingModel.Avatar.FileName);
            var avatarPath = Path.Combine(avatarsDir, uniqueName);
            var relativeAvatarPath = Path.Combine("\\", relativeAvatarsDir, uniqueName);
            this.UserProfile.AvatarPath = relativeAvatarPath;
            bindingModel.AvatarPath = relativeAvatarPath;

            bindingModel.Avatar.SaveAs(avatarPath);
            this.Data.Users.SaveChanges();
        }

        private bool DeleteOldAvatar()
        {
            var oldRelativePath = this.UserProfile.AvatarPath.TrimStart('\\');
            var oldAvatarPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, oldRelativePath);
            var defaultAvatarPath = UserHelper.GetUserDefaultAvatarPath();
            var userHasDefaultAvatar = oldAvatarPath.Contains(defaultAvatarPath);

            if (userHasDefaultAvatar)
            {
                return false;
            }

            if (System.IO.File.Exists(oldAvatarPath))
            {
                System.IO.File.Delete(oldAvatarPath);
            }

            return true;
        }

        private void SetDefaultAvatar()
        {
            this.UserProfile.AvatarPath = UserHelper.GetUserDefaultAvatarPath();

            this.Data.Users.SaveChanges();
        }

        #endregion
    }
}