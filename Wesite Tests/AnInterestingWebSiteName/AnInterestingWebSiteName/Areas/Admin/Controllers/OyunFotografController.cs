using AnInterestingWebSiteName.Areas.Admin.Filtre;
using AnInterestingWebSiteName.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;

namespace AnInterestingWebSiteName.Areas.Admin.Controllers
{
    [ModeratorAuthenticationFilter]
    public class OyunFotografController : Controller
    {
        AnInterestingWebSiteName_Model db = new AnInterestingWebSiteName_Model();
        // GET: Admin/OyunFotograf
        public ActionResult Index(int? id)
        {

            if (id != null || db.Urunlers.Find(id) != null)
            {
                ViewBag.urun = db.Urunlers.Find(id);
                return View(db.OyunResimleris.ToList().Where(s => s.Oyun_ID == id&&s.Aktifmi==true));
            }
            return RedirectToAction("Index", "Urun");

        }


        [HttpGet]
        public ActionResult Create(int? id)
        {

            if (id == null)
            {
                ViewBag.Oyun_ID = new SelectList(db.Urunlers.ToList(), "ID", "Ad");
            }
            else
            {
                ViewBag.urun = db.Urunlers.Find(id);
            }
            return View();
        }

        [HttpPost]
        public ActionResult Create(OyunResimleri model, HttpPostedFileBase image, int? id)
        {

            if (image == null)
            {
                ViewBag.message = "Lütfen Bir Fotoğraf Yükleyiniz";
                ViewBag.Urun_ID = new SelectList(db.Urunlers.ToList(), "ID", "Ad");
                return View();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (id != null)
                    {
                        model.Oyun_ID = (int)id;
                    }

                    model.Urunler = db.Urunlers.Find(model.Oyun_ID);
                    FileInfo fi = new FileInfo(image.FileName);
                    string name = Guid.NewGuid().ToString() + fi.Extension;
                    model.Resim = name;
                    image.SaveAs(Server.MapPath($"~/Fotograflar/UrunFotograflari/{name}"));

                    model.Aktifmi = true;

                    db.OyunResimleris.Add(model);
                    db.SaveChanges();
                    //çalış
                    ViewBag.message = "Fotoğraf Yükleme Başarılı";
                }
                catch (Exception ex)
                {
                    ViewBag.message = "Fotoğraf Yükleme Başarısız\nHata:" + ex.Message;
                }

            }

            if (ViewBag.urun != null)
            {
                ViewBag.urun = db.Urunlers.Find(model.Oyun_ID);
            }
            ViewBag.Oyun_ID = new SelectList(db.Urunlers.ToList(), "ID", "Ad");
            return View(model);
        }

        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id != null || db.OyunResimleris.Find(id) != null)
            {
                ViewBag.urun = db.Urunlers.Find(db.OyunResimleris.Find(id).Oyun_ID);
                ViewBag.urunFoto = db.OyunResimleris.Find(id);
                return View(db.OyunResimleris.Find(id));
            }
            ViewBag.message = "Hatalı Bir ID Girildi";
            return RedirectToAction("Index", "Urun");
        }

        [HttpPost]
        public ActionResult Edit(OyunResimleri model, HttpPostedFileBase image)
        {

            if (image == null)
            {
                ViewBag.message = "Lütfen Bir Fotoğraf Giriniz";
                return View();
            }

            if (ModelState.IsValid)
            {
                try
                {

                    model = db.OyunResimleris.First(s => s.ID == model.ID);


                    ViewBag.urun = db.Urunlers.Find(model.Oyun_ID);
                    ViewBag.urunFoto = db.OyunResimleris.Find(model.ID);

                    FileInfo fid = new FileInfo(Server.MapPath($"~/Fotograflar/UrunFotograflari/{model.Resim}"));
                    fid.Delete();

                    model.Urunler = db.Urunlers.Find(model.Oyun_ID);

                    FileInfo fi = new FileInfo(image.FileName);
                    string name = Guid.NewGuid().ToString() + fi.Extension;
                    model.Resim = name;
                    image.SaveAs(Server.MapPath($"~/Fotograflar/UrunFotograflari/{name}"));


                    db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                    db.OyunResimleris.AddOrUpdate(model);
                    db.SaveChanges();
                    //çalış
                    ViewBag.message = "Fotoğraf Düzenleme Başarılı";


                }
                catch (Exception ex)
                {
                    ViewBag.message = "Fotoğraf Düzenlenemedi \nHata:" + ex.Message;
                }
            }
            return View(model);
        }

        public ActionResult Delete(int? id)
        {

            if (id != null || db.OyunResimleris.Find(id) != null)
            {
                OyunResimleri or = db.OyunResimleris.Find(id);
                int? a = or.Urunler.ID;

                FileInfo fi = new FileInfo(Server.MapPath($"~/Fotograflar/UrunFotograflari/{or.Resim}"));
                fi.Delete();
                db.OyunResimleris.Remove(or);
                db.SaveChanges();

                id = a;

                return RedirectToAction("Index", "OyunFotograf", new { id = id });
            }
            else
            {

                return RedirectToAction("Index", "Urun");
            }

        }




    }
}