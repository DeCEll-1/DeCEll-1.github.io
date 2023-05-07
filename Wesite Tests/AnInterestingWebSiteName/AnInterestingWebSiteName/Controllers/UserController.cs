using AnInterestingWebSiteName.Classes;
using AnInterestingWebSiteName.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.Mvc;

namespace AnInterestingWebSiteName.Controllers
{
    [ValidateInput(false)]
    public class UserController : Controller
    {
        AnInterestingWebSiteName_Model db = new AnInterestingWebSiteName_Model();
        // GET: User
        public ActionResult Index(int? id)
        {
            UserViewClass uvc = new UserViewClass();

            if (id == 0 && Session["userSession"] != null)
            {
                uvc.Kullanici = (Models.Kullanici)Session["userSession"];

                uvc.Kutuphane = db.Kutuphanes.ToList().Where(s => s.Kullanici_ID == uvc.Kullanici.ID);

                bool e = true;

                foreach (var item in uvc.Kutuphane)
                {
                    if (e)
                    {
                        List<Urunler> gecicil = new List<Urunler>();

                        gecicil.Add(db.Urunlers.FirstOrDefault(s => s.ID == item.Oyun_ID));

                        uvc.Urunler = gecicil;

                        e = false;
                    }
                    else
                    {
                        List<Urunler> gecicil = (List<Urunler>)uvc.Urunler;

                        gecicil.Add(db.Urunlers.FirstOrDefault(s => s.ID == item.Oyun_ID));

                        uvc.Urunler = gecicil;
                    }
                }

                uvc.Benim = true;
            }
            else if (db.Kullanicis.Count(s => s.ID == id) > 0)
            {
                uvc.Kullanici = db.Kullanicis.Find(id);

                uvc.Kutuphane = db.Kutuphanes.ToList().Where(s => s.Kullanici_ID == uvc.Kullanici.ID);

                bool e = true;

                foreach (var item in uvc.Kutuphane)
                {
                    if (e)
                    {
                        List<Urunler> gecicil = new List<Urunler>();

                        gecicil.Add(db.Urunlers.FirstOrDefault(s => s.ID == item.Oyun_ID));

                        uvc.Urunler = gecicil;

                        e = false;
                    }
                    else
                    {
                        List<Urunler> gecicil = (List<Urunler>)uvc.Urunler;

                        gecicil.Add(db.Urunlers.FirstOrDefault(s => s.ID == item.Oyun_ID));

                        uvc.Urunler = gecicil;
                    }
                }
                if (Session["userSession"] != null)
                {
                    Kullanici k = (Kullanici)Session["userSession"];

                    if (k.ID == id)
                    {
                        uvc.Benim = true;
                    }
                }
            }
            else
            {
                return RedirectToAction("Index", "UserLogin");
            }

            return View(uvc);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Kullanici model, HttpPostedFileBase pfp)
        {
            if (pfp == null)
            {
                ViewBag.error = "Fotoğraf Giriniz";
                return View(model);
            }
            if (ModelState.IsValid)
            {
                try
                {

                    FileInfo fipfp = new FileInfo(pfp.FileName);
                    string namepfp = Guid.NewGuid().ToString() + fipfp.Extension;
                    model.ProfilResmi = namepfp;
                    pfp.SaveAs(Server.MapPath($"~/Fotograflar/KullaniciFotograflari/{namepfp}"));
                    model.Aktifmi = true;
                    db.Kullanicis.Add(model);
                    db.SaveChanges();
                    Session["userSession"] = model;
                    ViewBag.message = "Düzenleme Başarılı";
                }
                catch (Exception ex)
                {
                    ViewBag.error = "Ekleme Başarısız<br/>Hata:" + ex.Message;
                }
                return RedirectToAction("Index", "User");
            }

            return View(model);
        }

        [HttpGet]
        public ActionResult Edit()
        {
            if (Session["userSession"] == null)
            {
                return RedirectToAction("Index", "UserLogin");
            }
            Kullanici model = (Kullanici)Session["userSession"];
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(Kullanici model, HttpPostedFileBase pfp)
        {
            if (ModelState.IsValid)
            {
                try
                {

                    if (pfp != null)
                    {

                        FileInfo fidpfp = new FileInfo(Server.MapPath($"~/Fotograflar/KullaniciFotograflari/{db.Yoneticis.Find(model.ID).ProfilFotografi}"));
                        fidpfp.Delete();

                        FileInfo fipfp = new FileInfo(pfp.FileName);
                        string namepfp = Guid.NewGuid().ToString() + fipfp.Extension;
                        model.ProfilResmi = namepfp;
                        pfp.SaveAs(Server.MapPath($"~/Fotograflar/KullaniciFotograflari/{namepfp}"));
                    }
                    else
                    {
                        model.ProfilResmi = db.Kullanicis.Find(model.ID).ProfilResmi;
                    }

                    AnInterestingWebSiteName_Model dbe = new AnInterestingWebSiteName_Model();

                    model.Aktifmi = true;

                    dbe.Entry(model).State = System.Data.Entity.EntityState.Modified;
                    dbe.SaveChanges();
                    ViewBag.message = "Düzenleme Başarılı";
                    Session["userSession"] = model;
                }
                catch (Exception ex)
                {
                    ViewBag.error = "Düzenleme Başarısız<br/>Hata:" + ex.Message;
                }
            }

            return View(model);
        }

        public ActionResult Liste()
        {

            IEnumerable<Kullanici> ul = db.Kullanicis.ToList().Where(s=>s.Aktifmi==true);

            ul.OrderBy(s => s.Ad);

            return View(ul);
        }

    }
}