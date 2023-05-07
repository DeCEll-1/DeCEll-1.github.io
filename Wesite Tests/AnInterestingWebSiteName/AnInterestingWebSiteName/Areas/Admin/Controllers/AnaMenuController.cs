using AnInterestingWebSiteName.Areas.Admin.Filtre;
using AnInterestingWebSiteName.Classes;
using AnInterestingWebSiteName.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AnInterestingWebSiteName.Areas.Admin.Controllers
{
    [ModeratorAuthenticationFilter]
    public class AnaMenuController : Controller
    {

        SerilisationStuff ss = new SerilisationStuff();

        AnInterestingWebSiteName_Model db = new AnInterestingWebSiteName_Model();
        // GET: Admin/AnaMenu
        [HttpGet]
        public ActionResult Index()
        {
            BaseClass bc = new BaseClass();

            bc.WebAdress = System.Web.HttpContext.Current.Request.Url.GetLeftPart(System.UriPartial.Authority) + System.Web.VirtualPathUtility.ToAbsolute("~/");
            bc.Firma = db.Firmas.ToList();
            bc.Kullanici = db.Kullanicis.ToList();
            bc.Kutuphane = db.Kutuphanes.ToList();
            bc.OyunResimleri = db.OyunResimleris.ToList();
            bc.Tag = db.Tags.ToList();
            bc.TagsVeUrunAraClass = db.TagsVeUrunAraClasses.ToList();
            bc.Urunler = db.Urunlers.ToList();
            bc.Yonetici = db.Yoneticis.ToList();
            bc.YoneticiTur = db.YoneticiTurs.ToList();

            //if (true)
            //{
            //    FileInfo fi = new FileInfo(Server.MapPath($"~/Data.txt"));

            //    if (!fi.Exists)
            //    {
            //        fi.Create();
            //    }
            //    else
            //    {
            //        fi.Delete();
            //        fi.Create();
            //    }
            //}

            new StreamWriter(Server.MapPath($"~/Data.txt")).Close();

            StreamWriter sw = new StreamWriter(Server.MapPath($"~/Data.txt"), false);

            sw.Flush();
            sw.Write(ss.XMLSerialize(bc));
            sw.Close();

            return View();
        }

        [HttpPost]
        public ActionResult Index(HttpPostedFileBase dbu)
        {

            try
            {

                dbu.SaveAs(Server.MapPath($"~/Data.txt"));

                StreamReader sr = new StreamReader(dbu.InputStream);

                BaseClass bc = ss.XMLDeseriliazier(sr.ReadToEnd());

                foreach (var item in bc.Firma.ToList()) { db.Firmas.AddOrUpdate(item); }
                foreach (var item in bc.Urunler.ToList()) { db.Urunlers.AddOrUpdate(item); }
                foreach (var item in bc.OyunResimleri.ToList())
                {
                    if (item.Aktifmi == false)
                    {
                        OyunResimleri or = db.OyunResimleris.Find(item.ID);

                        FileInfo fi = new FileInfo(Server.MapPath($"~/Fotograflar/UrunFotograflari/{or.Resim}"));
                        fi.Delete();
                        db.OyunResimleris.Remove(or);
                        db.SaveChanges();
                    }
                    else
                    {
                        db.OyunResimleris.AddOrUpdate(item);
                    }
                }
                foreach (var item in bc.Kullanici.ToList()) { db.Kullanicis.AddOrUpdate(item); }
                foreach (var item in bc.Kutuphane.ToList()) { db.Kutuphanes.AddOrUpdate(item); }
                foreach (var item in bc.Tag.ToList()) { db.Tags.AddOrUpdate(item); }
                foreach (var item in bc.TagsVeUrunAraClass.ToList()) { db.TagsVeUrunAraClasses.AddOrUpdate(item); }
                foreach (var item in bc.YoneticiTur.ToList()) { db.YoneticiTurs.AddOrUpdate(item); }
                foreach (var item in bc.Yonetici.ToList()) { db.Yoneticis.AddOrUpdate(item); }

                db.SaveChanges();

                ViewBag.message = "Güncellendi";
            }
            catch (Exception ex)
            {
                ViewBag.message = "Hata: " + ex.Message;
            }


            return View();
        }

        [HttpGet]
        public ActionResult Edit()
        {


            if (Session["adminSession"] == null)
            {
                ViewBag.message = "Bir hata oluştu lütfen tekrar giriş yapıp tekrar deneyiniz";

                return RedirectToAction("Index", "Login");
            }
            Yonetici model = (Yonetici)Session["adminSession"];
            model.YoneticiTur = db.YoneticiTurs.Find(model.YoneticiTur_ID);

            ViewBag.YoneticiTur_ID = new SelectList(db.YoneticiTurs.ToList(), "ID", "Ad");

            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(Yonetici model, HttpPostedFileBase pfp, string pfpcb)
        {
            ViewBag.YoneticiTur_ID = new SelectList(db.YoneticiTurs.ToList(), "ID", "Ad");

            try
            {

                if (pfpcb != null)
                {
                    FileInfo fidpfp = new FileInfo(Server.MapPath($"~/Fotograflar/KullaniciFotograflari/{db.Yoneticis.Find(model.ID).ProfilFotografi}"));
                    fidpfp.Delete();
                    model.ProfilFotografi = "Smile.png";
                }
                else if (pfp != null)
                {
                    if (!(db.Yoneticis.Find(model.ID).ProfilFotografi == "Smile.png"))
                    {
                        FileInfo fidpfp = new FileInfo(Server.MapPath($"~/Fotograflar/KullaniciFotograflari/{db.Yoneticis.Find(model.ID).ProfilFotografi}"));
                        fidpfp.Delete();
                    }


                    FileInfo fipfp = new FileInfo(pfp.FileName);
                    string namepfp = Guid.NewGuid().ToString() + fipfp.Extension;
                    model.ProfilFotografi = namepfp;
                    pfp.SaveAs(Server.MapPath($"~/Fotograflar/KullaniciFotograflari/{namepfp}"));
                }
                else
                {
                    model.ProfilFotografi = db.Yoneticis.Find(model.ID).ProfilFotografi;
                }

                AnInterestingWebSiteName_Model dbe = new AnInterestingWebSiteName_Model();

                model.Aktifmi = true;

                dbe.Entry(model).State = System.Data.Entity.EntityState.Modified;
                dbe.SaveChanges();
                ViewBag.message = "Düzenleme Başarılı";
            }
            catch (Exception ex)
            {

                ViewBag.message = "Düzenleme Başarısız\nHata:" + ex.Message;

            }

            model.YoneticiTur = db.YoneticiTurs.FirstOrDefault(s => s.ID == model.YoneticiTur_ID);

            Session["adminSession"] = model;

            return View(model);
        }
    }
}