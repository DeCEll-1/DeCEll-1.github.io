using AnInterestingWebSiteName.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AnInterestingWebSiteName.Areas.Admin.Controllers
{
    public class LoginController : Controller
    {
        AnInterestingWebSiteName_Model db = new AnInterestingWebSiteName_Model();
        // GET: Admin/Login
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(string Mail,string Sifre)
        {

            if (string.IsNullOrEmpty(Mail)&&string.IsNullOrEmpty(Sifre))
            {
                ViewBag.message = "";
                return View();
            }

            if (ModelState.IsValid)
            {
                if (db.Yoneticis.Count(s => s.Mail == Mail && s.Sifre == Sifre) >0)
                {
                    if (db.Yoneticis.First(s => s.Mail == Mail && s.Sifre == Sifre).Aktifmi)
                    {
                        Session["adminSession"] = db.Yoneticis.First(s => s.Mail == Mail && s.Sifre == Sifre);
                        return RedirectToAction("Index","AnaMenu");
                    }
                    else
                    {
                        ViewBag.message = "Kullanıcı Banlı";
                    }
                }
                else
                {
                    ViewBag.message = "Kullanıcı Bulunamadı";
                }
            }

            return View();
        }

        [HttpGet]
        public ActionResult LogOut()
        {
            Session["adminSession"] = null;

            return RedirectToAction("Index","Login");
        }

    }
}