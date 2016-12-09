namespace App.Controllers
{
    using BeachRankings.Data.UnitOfWork;
    using BeachRankings.Models;
    using Microsoft.AspNet.Identity.Owin;
    using Microsoft.Owin.Security;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;

    public class BaseController : Controller
    {
        protected ApplicationSignInManager signInManager;
        protected ApplicationUserManager userManager;

        protected BaseController()
        {
        }

        protected BaseController(IBeachRankingsData data)
        {
            this.Data = data;
        }

        protected BaseController(IBeachRankingsData data, User userProfile)
            : this(data)
        {
            this.UserProfile = userProfile;
        }

        protected BaseController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            this.UserManager = userManager;
            this.SignInManager = signInManager;
        }

        protected IBeachRankingsData Data { get; private set; }

        protected User UserProfile { get; private set; }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            protected set
            {
                signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            protected set
            {
                userManager = value;
            }
        }

        protected override IAsyncResult BeginExecute(RequestContext requestContext, AsyncCallback callback, object state)
        {
            if (requestContext.HttpContext.User.Identity.IsAuthenticated)
            {
                var userName = requestContext.HttpContext.User.Identity.Name;
                var user = this.Data.Users.All().FirstOrDefault(u => u.UserName == userName);
                this.UserProfile = user;
            }

            return base.BeginExecute(requestContext, callback, state);
        }

        protected ICollection<string> GetModelStateErrors()
        {
            var errors = new List<string>();

            foreach (var modelState in this.ViewData.ModelState.Values)
            {
                foreach (var error in modelState.Errors)
                {
                    errors.Add(error.ErrorMessage);
                }
            }

            return errors;
        }

        protected void AddModelStateErrors(ICollection<string> errors)
        {
            foreach (var error in errors)
            {
                this.ModelState.AddModelError(string.Empty, error);
            }
        }
    }
}