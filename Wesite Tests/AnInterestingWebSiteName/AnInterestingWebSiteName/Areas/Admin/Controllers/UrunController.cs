using AnInterestingWebSiteName.Areas.Admin.Filtre;
using AnInterestingWebSiteName.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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
    public class UrunController : Controller
    {
        AnInterestingWebSiteName_Model db = new AnInterestingWebSiteName_Model();

        public ActionResult Index(int? a)
        {
            if (a == 0)
            {
                return View(db.Urunlers.ToList().Where(s => s.Aktifmi == true).OrderBy(s => s.Ad));
            }
            else if (a == 1)
            {
                return View(db.Urunlers.ToList().Where(s => s.Aktifmi == true).OrderBy(s => s.ID));
            }
            return View(db.Urunlers.ToList().Where(s => s.Aktifmi == true).OrderBy(s => s.Ad));

        }

        [AdminAuthenticationFilter]
        [HttpGet]
        public ActionResult Create()
        {

            ViewBag.Yayinci_ID = new SelectList(db.Firmas.ToList().Where(s=>s.Aktifmi==true), "ID", "Ad");

            ViewBag.Yapimci_ID = new SelectList(db.Firmas.ToList().Where(s => s.Aktifmi == true), "ID", "Ad");

            return View();
        }

        [AdminAuthenticationFilter]
        [HttpPost]
        public ActionResult Create(Urunler model, HttpPostedFileBase icon, HttpPostedFileBase fullImage, HttpPostedFileBase arkaPlanResmi)
        {
            ViewBag.Yayinci_ID = new SelectList(db.Firmas.ToList().Where(s => s.Aktifmi == true), "ID", "Ad");

            ViewBag.Yapimci_ID = new SelectList(db.Firmas.ToList().Where(s => s.Aktifmi == true), "ID", "Ad");
            if (icon == null || fullImage == null)
            {
                ViewBag.message = "Lütfen Fotoğraf Giriniz";
                return View();
            }
            ModelState.Remove("ArkaPlanResmi");
            ModelState.Remove("YayinTarihi");
            ModelState.Remove("IkonYolu");
            ModelState.Remove("TamResimYolu");
            if (ModelState.IsValid)
            {
                try
                {

                    FileInfo fiap = new FileInfo(arkaPlanResmi.FileName);
                    string nameap = Guid.NewGuid().ToString() + fiap.Extension;
                    model.ArkaPlanResmi = nameap;
                    arkaPlanResmi.SaveAs(Server.MapPath($"~/Fotograflar/UrunFotograflari/{nameap}"));

                    FileInfo fimr = new FileInfo(icon.FileName);
                    string namemr = Guid.NewGuid().ToString() + fimr.Extension;
                    model.IkonYolu = namemr;
                    icon.SaveAs(Server.MapPath($"~/Fotograflar/UrunFotograflari/{namemr}"));


                    FileInfo fifi = new FileInfo(fullImage.FileName);
                    string namefi = Guid.NewGuid().ToString() + fifi.Extension;
                    model.TamResimYolu = namefi;
                    fullImage.SaveAs(Server.MapPath($"~/Fotograflar/UrunFotograflari/{namefi}"));

                    if (model.Indirim == null)
                    {
                        model.Indirim = 0;
                    }

                    model.Aktifmi = true;

                    model.IndirimliFiyat = model.Fiyat - ((model.Fiyat * model.Indirim) / 100);

                    model.YayinTarihi = DateTime.Now.Date;

                    db.Urunlers.Add(model);
                    db.SaveChanges();
                    ViewBag.message = "Ürün Ekleme Başarılı";
                }
                catch (Exception ex)
                {
                    ViewBag.message = "Ürün Ekleme Başarısız\nHata =" + ex.Message;
                }
            }


            

            return View(model);
        }


        [AdminAuthenticationFilter]
        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id == null || db.Urunlers.Find(id) == null)
            {
                ViewBag.message = "Yanlış Bir ID Girdiniz";
                return RedirectToAction("Index");
            }

            List<SelectListItem> yapimci = new List<SelectListItem>();
            yapimci.Add(new SelectListItem { Text = "Yayıncıyı Değiştirme", Value = "0", Selected = true });
            foreach (Tag item in db.Tags.ToList())
            {
                yapimci.Add(new SelectListItem { Text = item.Ad, Value = item.ID.ToString(), Selected = false });
            }
            ViewBag.Yapimci = yapimci;

            List<SelectListItem> yayıncı = new List<SelectListItem>();
            yayıncı.Add(new SelectListItem { Text = "Yayıncıyı Değiştirme", Value = "0", Selected = true });
            foreach (Tag item in db.Tags.ToList())
            {
                yayıncı.Add(new SelectListItem { Text = item.Ad, Value = item.ID.ToString(), Selected = false });
            }
            ViewBag.Yayıncı = yayıncı;

            return View(db.Urunlers.Find(id));
        }

        [AdminAuthenticationFilter]
        [HttpPost]
        public ActionResult Edit(Urunler model, HttpPostedFileBase icon, HttpPostedFileBase fullImage,HttpPostedFileBase arkaPlanResmi)
        {
            List<SelectListItem> yapimci = new List<SelectListItem>();
            yapimci.Add(new SelectListItem { Text = "Yapımcı Değiştirme", Value = "0", Selected = true });
            foreach (Tag item in db.Tags.ToList())
            {
                yapimci.Add(new SelectListItem { Text = item.Ad, Value = item.ID.ToString(), Selected = false });
            }
            ViewBag.Yapimci = yapimci;

            List<SelectListItem> yayıncı = new List<SelectListItem>();
            yayıncı.Add(new SelectListItem { Text = "Yayıncıyı Değiştirme", Value = "0", Selected = true });
            foreach (Tag item in db.Tags.ToList())
            {
                yayıncı.Add(new SelectListItem { Text = item.Ad, Value = item.ID.ToString(), Selected = false });
            }
            ViewBag.Yayıncı = yayıncı;

            ModelState.Remove("IkonYolu");
            ModelState.Remove("YayinTarihi");
            ModelState.Remove("ArkaPlanResmi");
            ModelState.Remove("TamResimYolu");
            if (ModelState.IsValid)
            {
                try
                {
                    if (arkaPlanResmi != null)
                    {
                        FileInfo fidap = new FileInfo(Server.MapPath($"~/Fotograflar/UrunFotograflari/{db.Urunlers.FirstOrDefault(s => s.ID == model.ID).ArkaPlanResmi}"));
                        fidap.Delete();

                        FileInfo fiap = new FileInfo(arkaPlanResmi.FileName);
                        string nameap = Guid.NewGuid().ToString() + fiap.Extension;
                        model.ArkaPlanResmi = nameap;
                        arkaPlanResmi.SaveAs(Server.MapPath($"~/Fotograflar/UrunFotograflari/{nameap}"));
                    }
                    else
                    {
                        model.ArkaPlanResmi = db.Urunlers.FirstOrDefault(s => s.ID == model.ID).ArkaPlanResmi;
                    }

                    if (icon != null)
                    {
                        FileInfo fidi = new FileInfo(Server.MapPath($"~/Fotograflar/UrunFotograflari/{db.Urunlers.FirstOrDefault(s => s.ID == model.ID).IkonYolu}"));
                        fidi.Delete();

                        FileInfo fii = new FileInfo(icon.FileName);
                        string namei = Guid.NewGuid().ToString() + fii.Extension;
                        model.IkonYolu = namei;
                        icon.SaveAs(Server.MapPath($"~/Fotograflar/UrunFotograflari/{namei}"));
                    }
                    else
                    {
                        model.IkonYolu = db.Urunlers.FirstOrDefault(s => s.ID == model.ID).IkonYolu;
                    }

                    if (fullImage != null)
                    {
                        FileInfo fidfi = new FileInfo(Server.MapPath($"~/Fotograflar/UrunFotograflari/{db.Urunlers.FirstOrDefault(s => s.ID == model.ID).TamResimYolu}"));
                        fidfi.Delete();

                        FileInfo fifi = new FileInfo(fullImage.FileName);
                        string namefi = Guid.NewGuid().ToString() + fifi.Extension;
                        model.TamResimYolu = namefi;
                        fullImage.SaveAs(Server.MapPath($"~/Fotograflar/UrunFotograflari/{namefi}"));
                    }
                    else
                    {
                        model.TamResimYolu = db.Urunlers.FirstOrDefault(s => s.ID == model.ID).TamResimYolu;
                    }

                    model.YayinTarihi = db.Urunlers.FirstOrDefault(s => s.ID == model.ID).YayinTarihi.Date;

                    if (model.Yayinci_ID == 0)
                    {
                        model.Yayinci_ID = db.Urunlers.Find(model.ID).Yayinci_ID;
                    }

                    if (model.Yapimci_ID == 0)
                    {
                        model.Yapimci_ID = db.Urunlers.Find(model.ID).Yapimci_ID;
                    }

                    db.Dispose();

                    AnInterestingWebSiteName_Model dbu = new AnInterestingWebSiteName_Model();

                    model.Aktifmi = true;

                    model.IndirimliFiyat = model.Fiyat - ((model.Fiyat * model.Indirim) / 100);

                    dbu.Entry(model).State = EntityState.Modified;
                    dbu.SaveChanges();
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
            if (id != null || db.Urunlers.Find(id) == null)
            {
                Urunler u = db.Urunlers.Find(id);

                FileInfo fii = new FileInfo(Server.MapPath($"~/Fotograflar/UrunFotograflari/{u.IkonYolu}"));
                fii.Delete();

                FileInfo fifi = new FileInfo(Server.MapPath($"~/Fotograflar/UrunFotograflari/{u.TamResimYolu}"));
                fifi.Delete();

                foreach (var item in db.OyunResimleris.ToList().Where(s => s.Oyun_ID == u.ID))
                {
                    FileInfo fiord = new FileInfo(Server.MapPath($"~/Fotograflar/UrunFotograflari/{item.Resim}"));
                    fiord.Delete();
                    db.OyunResimleris.Remove(item);
                }

                db.Urunlers.Remove(u);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }


    }
}