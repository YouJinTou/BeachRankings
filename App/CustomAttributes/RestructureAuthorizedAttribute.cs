namespace BeachRankings.App.CustomAttributes
{
    using BeachRankings.Extensions;
    using Microsoft.AspNet.Identity;
    using System.Web.Mvc;
    using System.Web.Routing;

    public class RestructureAuthorizedAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var httpContext = filterContext.HttpContext;
            var userId = httpContext.User.Identity.GetUserId();

            if (!httpContext.User.Identity.CanRestructure(userId))
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Home", action = "Index" }));

                filterContext.Result.ExecuteResult(filterContext.Controller.ControllerContext);
            }
            else
            {
                base.OnActionExecuting(filterContext);
            }
        }
    }
}