using AnInterestingWebSiteName.Classes;
using AnInterestingWebSiteName.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AnInterestingWebSiteName.Controllers
{
    public class UserFirmaController : Controller
    {
        AnInterestingWebSiteName_Model db = new AnInterestingWebSiteName_Model();

        // GET: UserFirma
        public ActionResult Index(int? id)
        {
            if (id==null||db.Firmas.Find(id)==null)
            {
                TempData["message"] = "Firma Bulunamadı";
                return RedirectToAction("Index","Liste");
            }

            FirmaIndexViewClass fivc = new FirmaIndexViewClass();

            fivc.Firma = db.Firmas.Find(id);

            fivc.Urunler = db.Urunlers.ToList().Where(s=>s.Yayinci_ID==id||s.Yapimci_ID==id);

            return View(fivc);
        }
    }
}