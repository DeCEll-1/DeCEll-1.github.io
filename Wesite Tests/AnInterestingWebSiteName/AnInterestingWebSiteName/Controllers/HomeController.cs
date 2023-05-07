using AnInterestingWebSiteName.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AnInterestingWebSiteName.Controllers
{
    public class HomeController : Controller
    {
        AnInterestingWebSiteName_Model db = new AnInterestingWebSiteName_Model();
        // GET: Home
        public ActionResult Index()
        {
            return View(db.Urunlers.ToList().Where(s=>s.Aktifmi==true));
        }
    }
}