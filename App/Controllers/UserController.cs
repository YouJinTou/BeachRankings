namespace App.Controllers
{
    using App.Controllers.Enums;
    using AutoMapper;
    using BeachRankings.App.Models.ViewModels;
    using BeachRankings.App.Utils;
    using BeachRankings.Data.UnitOfWork;
    using BeachRankings.Models;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Web.Mvc;

    public class UserController : BaseController
    {
        public UserController(IBeachRankingsData data)
            : base(data)
        {
        }

        [Authorize]
        public PartialViewResult Statistics()
        {
            var reviews = this.Data.Reviews.All().Where(r => r.AuthorId == this.UserProfile.Id);
            var model = Mapper.Map<IEnumerable<Review>, IEnumerable<TableRowViewModel>>(reviews);

            return this.PartialView("_StatisticsPartial", model);
        }
        
        public ActionResult Reviews(string authorId)
        {
            var user = this.Data.Users.Find(authorId);
            var model = Mapper.Map<User, TableUserReviewsViewModel>(user);

            return this.View("_Statistics", model);
        }

        [Authorize]
        public PartialViewResult Images(int page = 0, int pageSize = 10)
        {
            var imageGroups = this.Data.BeachImages.All().Where(i => i.AuthorId == this.UserProfile.Id).GroupBy(i => i.Beach.Name);
            var model = new List<DashboardImageViewModel>();

            foreach (var group in imageGroups)
            {
                model.Add(new DashboardImageViewModel()
                {
                    BeachName = group.Key,
                    Paths = Mapper.Map<IEnumerable<BeachImage>, IEnumerable<BeachImageThumbnailViewModel>>(group.ToList())
                });
            }

            model = model.Skip(page * pageSize).Take(pageSize).ToList();

            return this.PartialView(model);
        }

        [Authorize]
        public PartialViewResult ChangeAvatar()
        {
            var model = new ChangeAvatarViewModel() { AvatarPath = this.UserProfile.AvatarPath };

            return this.PartialView(model);
        }

        [Authorize]
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

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteAvatar()
        {
            this.DeleteOldAvatar();
            this.SetDefaultAvatar();

            return this.RedirectToAction("Index", "Manage", new { Message = ActionMessage.DeleteAvatarSuccess });
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

        private void DeleteOldAvatar()
        {
            var oldRelativePath = this.UserProfile.AvatarPath.TrimStart('\\');
            var oldAvatarPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, oldRelativePath);

            if (System.IO.File.Exists(oldAvatarPath))
            {
                System.IO.File.Delete(oldAvatarPath);
            }
        }

        private void SetDefaultAvatar()
        {
            this.UserProfile.AvatarPath = UserHelper.GetUserDefaultAvatarPath();

            this.Data.Users.SaveChanges();
        }

        #endregion
    }
}