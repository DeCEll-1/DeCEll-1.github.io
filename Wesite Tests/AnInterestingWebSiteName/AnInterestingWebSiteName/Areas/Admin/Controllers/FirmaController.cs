using AnInterestingWebSiteName.Areas.Admin.Filtre;
using AnInterestingWebSiteName.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Migrations;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AnInterestingWebSiteName.Areas.Admin.Controllers
{
    [ValidateInput(false)]
    [ModeratorAuthenticationFilter]
    public class FirmaController : Controller
    {
        AnInterestingWebSiteName_Model db = new AnInterestingWebSiteName_Model();
        // GET: Admin/Firma
        public ActionResult Index()
        {
            return View(db.Firmas.ToList().Where(s => s.Aktifmi == true));
        }

        [AdminAuthenticationFilter]
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [AdminAuthenticationFilter]
        [HttpPost]
        public ActionResult Create(Firma model, string cke_FirmaHakkinda, HttpPostedFileBase firmaResmi, HttpPostedFileBase firmaArkaPlanResmi)
        {
            if (firmaResmi == null || firmaArkaPlanResmi == null)
            {
                ViewBag.message = "Lütfen Fotoğraf Giriniz";
                return View();
            }
            ModelState.Remove("FirmaArkaPlanResmi");
            ModelState.Remove("FirmaResmi");
            if (ModelState.IsValid)
            {
                try
                {

                    if (db.Firmas.Count(s => s.Ad == model.Ad || s.Mail == model.Mail) > 0)
                    {
                        ViewBag.message = "Firma Zaten Var";
                        return View(model);
                    }


                    FileInfo fii = new FileInfo(firmaResmi.FileName);
                    string namei = Guid.NewGuid().ToString() + fii.Extension;
                    model.FirmaResmi = namei;
                    firmaResmi.SaveAs(Server.MapPath($"~/Fotograflar/FirmaFotograflari/{namei}"));

                    FileInfo fifi = new FileInfo(firmaArkaPlanResmi.FileName);
                    string namefi = Guid.NewGuid().ToString() + fifi.Extension;
                    model.FirmaArkaPlanResmi = namefi;
                    firmaArkaPlanResmi.SaveAs(Server.MapPath($"~/Fotograflar/FirmaFotograflari/{namefi}"));

                    model.Aktifmi = true;

                    db.Firmas.Add(model);
                    db.SaveChanges();
                    ViewBag.message = "Firma Ekleme Başarılı";
                }
                catch (Exception ex)
                {
                    ViewBag.message = "Firma Eklenemedi\nHata:" + ex.Message;
                }
            }

            return View(model);
        }

        [AdminAuthenticationFilter]
        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id == null || db.Firmas.Find(id) == null)
            {
                ViewBag.message = "Yanlış Bir ID Girdiniz";
                return RedirectToAction("Index");
            }

            return View(db.Firmas.Find(id));
        }

        [AdminAuthenticationFilter]
        [HttpPost]
        public ActionResult Edit(Firma model, HttpPostedFileBase firmaResmi, HttpPostedFileBase firmaArkaPlanResmi)
        {


            ModelState.Remove("FirmaArkaPlanResmi");
            ModelState.Remove("FirmaResmi");
            if (ModelState.IsValid)
            {
                try
                {

                    if (firmaResmi != null)
                    {
                        FileInfo fifr = new FileInfo(Server.MapPath($"~/Fotograflar/FirmaFotograflari/{db.Firmas.FirstOrDefault(s => s.ID == model.ID).FirmaResmi}"));
                        fifr.Delete();

                        FileInfo fii = new FileInfo(firmaResmi.FileName);
                        string namei = Guid.NewGuid().ToString() + fii.Extension;
                        model.FirmaResmi = namei;
                        firmaResmi.SaveAs(Server.MapPath($"~/Fotograflar/FirmaFotograflari/{namei}"));
                    }
                    else
                    {
                        model.FirmaResmi = db.Firmas.FirstOrDefault(s => s.ID == model.ID).FirmaResmi;
                    }


                    if (firmaArkaPlanResmi != null)
                    {
                        FileInfo fidfi = new FileInfo(Server.MapPath($"~/Fotograflar/FirmaFotograflari/{db.Firmas.FirstOrDefault(s => s.ID == model.ID).FirmaArkaPlanResmi}"));
                        fidfi.Delete();

                        FileInfo fifi = new FileInfo(firmaArkaPlanResmi.FileName);
                        string namefi = Guid.NewGuid().ToString() + fifi.Extension;
                        model.FirmaArkaPlanResmi = namefi;
                        firmaArkaPlanResmi.SaveAs(Server.MapPath($"~/Fotograflar/FirmaFotograflari/{namefi}"));
                    }
                    else
                    {
                        model.FirmaArkaPlanResmi = db.Firmas.FirstOrDefault(s => s.ID == model.ID).FirmaArkaPlanResmi;
                    }

                    db.Dispose();

                    AnInterestingWebSiteName_Model dbf = new AnInterestingWebSiteName_Model();

                    model.Aktifmi = true;

                    dbf.Entry(model).State = EntityState.Modified;
                    dbf.SaveChanges();
                    ViewBag.message = "Ürün Düzenleme Başarılı";
                }
                catch (Exception ex)
                {
                    ViewBag.message = "Ürün Düzenleme Başarısız\nHata Kodu" + ex.Message;
                }
            }
            return View(model);
        }

        [AdminAuthenticationFilter]
        [HttpGet]
        public ActionResult Delete(int? id)
        {

            if (id == null || db.Firmas.Find(id) == null)
            {
                ViewBag.message = "Yanlış Bir ID Girdiniz";
                return RedirectToAction("Index");
            }

            Firma f = db.Firmas.Find(id);

            foreach (var item in db.Urunlers.ToList().Where(s => s.Yayinci_ID == id))
            {
                AnInterestingWebSiteName_Model dbe = new AnInterestingWebSiteName_Model();
                item.Aktifmi = false;
                item.Yayinci_ID = 0;
                dbe.Entry(item).State = EntityState.Modified;
                dbe.SaveChanges();
            }

            foreach (var item in db.Urunlers.ToList().Where(s => s.Yapimci_ID == id))
            {
                AnInterestingWebSiteName_Model dbe = new AnInterestingWebSiteName_Model();
                item.Aktifmi = false;
                item.Yayinci_ID = 0;
                dbe.Entry(item).State = EntityState.Modified;
                dbe.SaveChanges();
            }

            FileInfo fidfr = new FileInfo(Server.MapPath($"~/Fotograflar/UrunFotograflari/{f.FirmaResmi}"));
            fidfr.Delete();

            FileInfo fidfapr = new FileInfo(Server.MapPath($"~/Fotograflar/UrunFotograflari/{f.FirmaArkaPlanResmi}"));
            fidfapr.Delete();

            db.Firmas.Remove(f);

            db.SaveChanges();

            ViewBag.message = "Firma Silindi (Ve Firmanın Yaptığı/Yayınladığı Oyunlar Aktifsizleştirildi)";

            return RedirectToAction("Index");
        }
    }
}