using AnInterestingWebSiteName.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AnInterestingWebSiteName.Controllers
{
    public class UserLoginController : Controller
    {
        AnInterestingWebSiteName_Model db = new AnInterestingWebSiteName_Model();
        // GET: UserLogin
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(Kullanici model)
        {

            if (Session["userSession"] != null)
            {
                return RedirectToAction("Index", "Liste");
            }

            ModelState.Remove("Ad");
            ModelState.Remove("Sifre");
            ModelState.Remove("Mail");
            if (db.Kullanicis.Count(s => s.Mail == model.Mail && s.Sifre == model.Sifre) > 0)
            {
                Session["userSession"] = db.Kullanicis.Find(db.Kullanicis.FirstOrDefault(s => s.Mail == model.Mail && s.Sifre == model.Sifre).ID);
                TempData["message"] = "Giriş Başarılı";
                Session.Timeout = 120;
                return RedirectToAction("Index", "Liste");
            }
            else
            {
                ViewBag.message = "Kullanıcı Bulunamadı";
                return View(model);
            }

        }

        [HttpGet]
        public ActionResult LogOut()
        {
            Session["userSession"] = null;

            TempData["message"] = "Çıkış Başarılı";

            return RedirectToAction("Index", "Liste");
        }
    }
}