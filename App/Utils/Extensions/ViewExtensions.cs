﻿namespace BeachRankings.App.Utils.Extensions
{
    using System;
    using System.IO;
    using System.Text;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.UI;

    public static class ViewExtensions
    {
        public static string RenderPartialViewAsString(this PartialViewResult partialView)
        {
            var httpContext = HttpContext.Current;

            if (httpContext == null)
            {
                throw new NotSupportedException("An HTTP context is required to render the partial view as a string.");
            }

            var controllerName = httpContext.Request.RequestContext.RouteData.Values["controller"].ToString();
            var controller = (ControllerBase)ControllerBuilder.Current.GetControllerFactory().CreateController(httpContext.Request.RequestContext, controllerName);
            var controllerContext = new ControllerContext(httpContext.Request.RequestContext, controller);
            var view = ViewEngines.Engines.FindPartialView(controllerContext, partialView.ViewName).View;
            var sb = new StringBuilder();

            using (var sw = new StringWriter(sb))
            {
                using (var tw = new HtmlTextWriter(sw))
                {
                    view.Render(new ViewContext(controllerContext, view, partialView.ViewData, partialView.TempData, tw), tw);
                }
            }

            return sb.ToString();
        }
    }
}