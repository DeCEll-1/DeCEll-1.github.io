using AnInterestingWebSiteName.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Filters;
using System.Web.Mvc;

namespace AnInterestingWebSiteName.Areas.Admin.Filtre
{
    public class AdminAuthenticationFilterAttribute : System.Web.Mvc.ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (filterContext.RequestContext.HttpContext.Session["adminSession"]==null)
            {
                filterContext.RequestContext.HttpContext.Response.Redirect("~/Admin/Login/Index");
            }

            Yonetici y = (Yonetici)filterContext.RequestContext.HttpContext.Session["adminSession"];

            if (y.YoneticiTur_ID !=1)
            {
                filterContext.RequestContext.HttpContext.Response.Redirect("~/Admin/YetkiYetersiz/Index");
            }

            base.OnActionExecuted(filterContext);
        }

    }
}