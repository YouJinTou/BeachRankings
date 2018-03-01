namespace App.Controllers
{
    using App.Code.Blogs;
    using App.Code.Web;
    using BeachRankings.App.Models.ViewModels;
    using BeachRankings.App.Utils;
    using BeachRankings.Data;
    using BeachRankings.Data.UnitOfWork;
    using BeachRankings.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Microsoft.AspNet.Identity.Owin;
    using Microsoft.Owin.Security;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web;
    using System.Web.Mvc;

    [Authorize]
    public class AccountController : BaseController
    {
        private IBlogValidator blogValidator;
        private IWebNameParser webParser;

        public AccountController(
            IBeachRankingsData data, 
            IBlogValidator blogValidator,
            IWebNameParser webParser)
            : base(data)
        {
            this.blogValidator = blogValidator;
            this.webParser = webParser;
        }

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return this.View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return this.View(model);
            }

            var user = this.Data.Users.All()
                .FirstOrDefault(u => u.Email == model.UsernameEmail || u.UserName == model.UsernameEmail);

            if (user != null && !await this.UserManager.IsEmailConfirmedAsync(user.Id))
            {
                var callbackUrl = await this.SendEmailConfirmationTokenAsync(user.Id, "Confirm your account");
                this.ViewBag.Message = "You have to confirm your email before signing in. "
                              + "We've resent the confirmation token to your email. " +
                              "It might take a few minutes for it to arrive.";

                return this.View("Info");
            }

            var result = await SignInManager.PasswordSignInAsync(
                user?.UserName ?? string.Empty, model.Password, model.RememberMe, shouldLockout: true);

            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    this.ViewBag.Message = $"Your account has been locked out and " +
                        $"will be unlocked in {SignInManager.UserManager.DefaultAccountLockoutTimeSpan.Minutes} minutes.";

                    return this.View("Info");
                case SignInStatus.RequiresVerification:
                    return this.RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return this.View(model);
            }
        }

        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
        {
            // Require that the user has already logged in via username/password or external login
            if (!await SignInManager.HasBeenVerifiedAsync())
            {
                return this.View("Error");
            }
            return this.View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return this.View(model);
            }

            // The following code protects for brute force attacks against the two factor codes. 
            // If a user enters incorrect codes for a specified amount of time then the user account 
            // will be locked out for a specified amount of time. 
            // You can configure the account lockout settings in IdentityConfig
            var result = await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent: model.RememberMe, rememberBrowser: model.RememberBrowser);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(model.ReturnUrl);
                case SignInStatus.LockedOut:
                    return this.View("Lockout");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid code.");
                    return this.View(model);
            }
        }

        [AllowAnonymous]
        public ActionResult Register()
        {
            return this.View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            this.AddModelStateErrors(this.blogValidator.ValidateBlogger(model.IsBlogger, model.BlogUrl));
            
            if (this.ModelState.IsValid)
            {
                var user = new User
                {
                    UserName = model.UserName,
                    Email = model.Email,
                    IsBlogger = model.IsBlogger,
                    AvatarPath = UserHelper.GetUserDefaultAvatarPath()
                };
                var result = await this.UserManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await this.UserManager.AddToRoleAsync(user.Id, "User");

                    if (model.IsBlogger)
                    {
                        var blog = new Blog() { Id = user.Id, Url = this.webParser.GetUriHostName(model.BlogUrl) };

                        this.Data.Blogs.Add(blog);
                        this.Data.Blogs.SaveChanges();
                    }

                    var callbackUrl = await SendEmailConfirmationTokenAsync(user.Id, "Confirm your account");
                    ViewBag.Message = $"We've sent a verification link to {model.Email}. " +
                        $"It might take a few minutes for it to arrive. You must click it before signing in.";

                    return this.View("Info");
                }

                this.AddErrors(result);
            }

            return this.View(model); // Error
        }

        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return this.View("Error");
            }

            var result = await this.UserManager.ConfirmEmailAsync(userId, code);

            return this.View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return this.View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByEmailAsync(model.Email);

                if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
                {
                    return this.View("ForgotPasswordConfirmation");
                }

                var code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                var callbackUrl = Url.Action(
                    "ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);

                await UserManager.SendEmailAsync(
                    user.Id, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>. " +
                    "<br /><br />If you didn't request this, you can ignore it.");

                return this.RedirectToAction("ForgotPasswordConfirmation", "Account");
            }

            return this.View(model);
        }

        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return this.View();
        }

        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return this.View(model);
            }

            var user = await UserManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                return this.RedirectToAction("ResetPasswordConfirmation", "Account");
            }

            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);

            if (result.Succeeded)
            {
                return this.RedirectToAction("ResetPasswordConfirmation", "Account");
            }

            AddErrors(result);

            return this.View();
        }

        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return this.View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
        {
            var userId = await SignInManager.GetVerifiedUserIdAsync();
            if (userId == null)
            {
                return this.View("Error");
            }
            var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
            var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            return this.View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return this.View();
            }

            // Generate the token and send it
            if (!await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
            {
                return this.View("Error");
            }
            return this.RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
        }

        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return this.RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return this.View("Lockout");
                case SignInStatus.RequiresVerification:
                    return this.RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
                case SignInStatus.Failure:
                default:
                    // If the user does not have an account, then prompt the user to create an account
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                    return this.View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return this.RedirectToAction("Index", "Manage");
            }

            this.AddModelStateErrors(this.blogValidator.ValidateBlogger(model.IsBlogger, model.BlogUrl));

            if (ModelState.IsValid)
            {
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();

                if (info == null)
                {
                    return this.View("ExternalLoginFailure");
                }

                var user = new User {
                    UserName = model.UserName,
                    Email = model.Email,
                    IsBlogger = model.IsBlogger,
                    AvatarPath = UserHelper.GetUserDefaultAvatarPath()
                };
                var result = await UserManager.CreateAsync(user);

                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);

                    await UserManager.AddToRoleAsync(user.Id, "User");

                    if (model.IsBlogger)
                    {
                        var blog = new Blog() { Id = user.Id, Url = this.webParser.GetUriHostName(model.BlogUrl) };

                        this.Data.Blogs.Add(blog);
                        this.Data.SaveChanges();
                    }

                    if (result.Succeeded)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

                        return this.RedirectToLocal(returnUrl);
                    }
                }

                this.AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return this.View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return this.RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return this.View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.userManager != null)
                {
                    this.userManager.Dispose();
                    this.userManager = null;
                }

                if (this.signInManager != null)
                {
                    this.signInManager.Dispose();
                    this.signInManager = null;
                }
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

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return this.RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }

        private async Task<string> SendEmailConfirmationTokenAsync(string userID, string subject)
        {
            string code = await UserManager.GenerateEmailConfirmationTokenAsync(userID);
            var callbackUrl = Url.Action("ConfirmEmail", "Account",
               new { userId = userID, code = code }, protocol: Request.Url.Scheme);
            await UserManager.SendEmailAsync(userID, subject,
               "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>.");

            return callbackUrl;
        }

        #endregion
    }
}