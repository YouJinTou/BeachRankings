namespace App.Controllers
{
    using App.Code.Blogs;
    using App.Controllers.Enums;
    using BeachRankings.App.Models.ViewModels;
    using BeachRankings.App.Utils;
    using BeachRankings.Data;
    using BeachRankings.Data.UnitOfWork;
    using BeachRankings.Extensions;
    using BeachRankings.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.Owin.Security;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web;
    using System.Web.Mvc;

    [Authorize]
    public class ManageController : BaseController
    {
        private IBlogValidator blogValidator;

        public ManageController(IBeachRankingsData data, IBlogValidator blogValidator)
            : base(data)
        {
            this.blogValidator = blogValidator;
        }

        public async Task<ActionResult> Index(string message)
        {
            this.ViewBag.StatusMessage = message;

            var beachesDbContext = new BeachRankingsDbContext();
            var userId = User.Identity.GetUserId();
            var user = beachesDbContext.Users.Include(u => u.Blog).FirstOrDefault(u => u.Id == userId);
            var model = new ManageIndexViewModel
            {
                HasPassword = HasPassword(),
                PhoneNumber = await UserManager.GetPhoneNumberAsync(userId),
                TwoFactor = await UserManager.GetTwoFactorEnabledAsync(userId),
                Logins = await UserManager.GetLoginsAsync(userId),
                BrowserRemembered = await AuthenticationManager.TwoFactorBrowserRememberedAsync(userId),
                IsBlogger = user.IsBlogger,
                BlogUrl = user.IsBlogger ? user.Blog.Url : string.Empty
            };

            return this.View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Blog(ManageIndexViewModel model)
        {
            this.AddModelStateErrors(this.blogValidator.ValidateBlogger(model.IsBlogger, model.BlogUrl));

            if (!this.ModelState.IsValid)
            {
                return this.RedirectToAction("Index", new { Message = string.Join(", ", this.GetModelStateErrors()) });
            }

            var blog = this.UserProfile.Blog;
            this.UserProfile.IsBlogger = model.IsBlogger;
                        
            if (model.IsBlogger)
            {
                if (blog == null)
                {
                    blog = new Blog() { Id = this.UserProfile.Id };

                    this.Data.Blogs.Add(blog);
                }

                blog.Url = GenericHelper.GetUriHostName(model.BlogUrl);
            }
            else
            {
                if (blog != null)
                {
                    var articles = this.Data.BlogArticles.All().Where(ba => ba.BlogId == blog.Id).ToList();

                    this.Data.BlogArticles.RemoveMany(articles);
                    this.Data.Blogs.Remove(blog);
                }
            }

            this.Data.SaveChanges();

            return this.RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemoveLogin(string loginProvider, string providerKey)
        {
            string message;
            var result = await UserManager.RemoveLoginAsync(User.Identity.GetUserId(), new UserLoginInfo(loginProvider, providerKey));
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                message = ActionMessage.RemoveLoginSuccess.GetDescription();
            }
            else
            {
                message = ActionMessage.Error.GetDescription();
            }
            return this.RedirectToAction("ManageLogins", new { Message = message });
        }

        public ActionResult AddPhoneNumber()
        {
            return this.View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddPhoneNumber(AddPhoneNumberViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return this.View(model);
            }
            // Generate the token and send it
            var code = await UserManager.GenerateChangePhoneNumberTokenAsync(User.Identity.GetUserId(), model.Number);
            if (UserManager.SmsService != null)
            {
                var message = new IdentityMessage
                {
                    Destination = model.Number,
                    Body = "Your security code is: " + code
                };
                await UserManager.SmsService.SendAsync(message);
            }
            return this.RedirectToAction("VerifyPhoneNumber", new { PhoneNumber = model.Number });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EnableTwoFactorAuthentication()
        {
            await UserManager.SetTwoFactorEnabledAsync(User.Identity.GetUserId(), true);
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user != null)
            {
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }
            return this.RedirectToAction("Index", "Manage");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DisableTwoFactorAuthentication()
        {
            await UserManager.SetTwoFactorEnabledAsync(User.Identity.GetUserId(), false);
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user != null)
            {
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }
            return this.RedirectToAction("Index", "Manage");
        }

        public async Task<ActionResult> VerifyPhoneNumber(string phoneNumber)
        {
            var code = await UserManager.GenerateChangePhoneNumberTokenAsync(User.Identity.GetUserId(), phoneNumber);
            // Send an SMS through the SMS provider to verify the phone number
            return phoneNumber == null ? View("Error") : View(new VerifyPhoneNumberViewModel { PhoneNumber = phoneNumber });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyPhoneNumber(VerifyPhoneNumberViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return this.View(model);
            }
            var result = await UserManager.ChangePhoneNumberAsync(User.Identity.GetUserId(), model.PhoneNumber, model.Code);
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                return this.RedirectToAction("Index", new { Message = ActionMessage.AddPhoneSuccess.GetDescription() });
            }

            ModelState.AddModelError("", "Failed to verify phone");

            return this.View(model);
        }

        public async Task<ActionResult> RemovePhoneNumber()
        {
            var result = await UserManager.SetPhoneNumberAsync(User.Identity.GetUserId(), null);
            if (!result.Succeeded)
            {
                return this.RedirectToAction("Index", new { Message = ActionMessage.Error.GetDescription() });
            }
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user != null)
            {
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }
            return this.RedirectToAction("Index", new { Message = ActionMessage.RemovePhoneSuccess.GetDescription() });
        }

        public ActionResult ChangePassword()
        {
            return this.View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return this.View(model);
            }
            var result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                return this.RedirectToAction("Index", new { Message = ActionMessage.ChangePasswordSuccess.GetDescription() });
            }
            AddErrors(result);
            return this.View(model);
        }

        public ActionResult SetPassword()
        {
            return this.View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SetPassword(SetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await UserManager.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);
                if (result.Succeeded)
                {
                    var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                    if (user != null)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                    }
                    return this.RedirectToAction("Index", new { Message = ActionMessage.SetPasswordSuccess.GetDescription() });
                }
                AddErrors(result);
            }

            return this.View(model); // Error
        }

        public async Task<ActionResult> ManageLogins(ActionMessage? message)
        {
            ViewBag.StatusMessage =
                message == ActionMessage.RemoveLoginSuccess ? "The external login was removed."
                : message == ActionMessage.Error ? "An error has occurred."
                : "";
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user == null)
            {
                return this.View("Error");
            }
            var userLogins = await UserManager.GetLoginsAsync(User.Identity.GetUserId());
            var otherLogins = AuthenticationManager.GetExternalAuthenticationTypes().Where(auth => userLogins.All(ul => auth.AuthenticationType != ul.LoginProvider)).ToList();
            ViewBag.ShowRemoveButton = user.PasswordHash != null || userLogins.Count > 1;
            return this.View(new ManageLoginsViewModel
            {
                CurrentLogins = userLogins,
                OtherLogins = otherLogins
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LinkLogin(string provider)
        {
            // Request a redirect to the external login provider to link a login for the current user
            return new AccountController.ChallengeResult(provider, Url.Action("LinkLoginCallback", "Manage"), User.Identity.GetUserId());
        }

        public async Task<ActionResult> LinkLoginCallback()
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync(XsrfKey, User.Identity.GetUserId());
            if (loginInfo == null)
            {
                return this.RedirectToAction("ManageLogins", new { Message = ActionMessage.Error.GetDescription() });
            }
            var result = await UserManager.AddLoginAsync(User.Identity.GetUserId(), loginInfo.Login);
            return result.Succeeded ? RedirectToAction("ManageLogins") : RedirectToAction("ManageLogins", new { Message = ActionMessage.Error.GetDescription() });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && this.userManager != null)
            {
                this.userManager.Dispose();
                this.userManager = null;
            }

            base.Dispose(disposing);
        }

        #region Helpers

        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private bool HasPassword()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PasswordHash != null;
            }
            return false;
        }

        private bool HasPhoneNumber()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PhoneNumber != null;
            }
            return false;
        }

        #endregion
    }
}