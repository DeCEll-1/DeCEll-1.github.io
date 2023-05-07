using AnInterestingWebSiteName.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.SessionState;

namespace AnInterestingWebSiteName.Areas.Admin.Controllers
{
    public class YetkiYetersizController : Controller
    {
        // GET: Admin/YetkiYetersiz
        public ActionResult Index()
        {
            if (Session["adminSession"]==null)
            {
                return RedirectToAction("Index", "Login");
            }

            Yonetici model = (Yonetici)Session["adminSession"];

            return View(model);
        }
    }
}