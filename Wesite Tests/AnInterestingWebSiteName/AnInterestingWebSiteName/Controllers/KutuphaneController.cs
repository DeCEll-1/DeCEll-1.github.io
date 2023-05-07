using AnInterestingWebSiteName.Classes;
using AnInterestingWebSiteName.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AnInterestingWebSiteName.Controllers
{
    public class KutuphaneController : Controller
    {
        AnInterestingWebSiteName_Model db = new AnInterestingWebSiteName_Model();

        // GET: Kutuphane
        [HttpGet]
        public ActionResult Index(int? id)
        {
            if (Session["userSession"] == null)
            {
                return RedirectToAction("Index", "UserLogin");
            }

            Kullanici k = (Kullanici)Session["userSession"];
            KutuphaneModelClass kmc = new KutuphaneModelClass();

            if (id != 0)
            {
                if (id != null)
                {
                    if (db.Urunlers.Find(id) == null)
                    {
                        TempData["message"] = "Ürün Bulunamadı";
                        return RedirectToAction("Index");
                    }
                    else if (db.Urunlers.Find(id).Aktifmi == false)
                    {
                        TempData["message"] = "Ürün Bulunamadı";
                        return RedirectToAction("Index");
                    }

                    else if (db.Kutuphanes.Count(s => s.Kullanici_ID == k.ID && s.Oyun_ID == id) < 1)
                    {
                        TempData["message"] = "Ürün Bulunamadı";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        kmc.Urun = db.Urunlers.Find(id);

                        kmc.tagsVeUrunAraClasses = db.TagsVeUrunAraClasses.ToList().Where(s=>s.Urun_ID==kmc.Urun.ID);

                        bool e = true;

                        foreach (TagsVeUrunAraClass item in kmc.tagsVeUrunAraClasses)
                        {
                            if (e)
                            {
                                List<Tag> gecicit = new List<Tag>();

                                gecicit.Add(db.Tags.FirstOrDefault(s => s.ID == item.Tag_ID));

                                kmc.Tag = gecicit;

                                e = false;
                            }
                            else
                            {
                                List<Tag> gecicit = (List<Tag>)kmc.Tag;

                                gecicit.Add(db.Tags.FirstOrDefault(s => s.ID == item.Tag_ID));

                                kmc.Tag = gecicit;
                            }
                        }
                    }
                }
            }

            kmc.Kutuphane = db.Kutuphanes.ToList().Where(s => s.Kullanici_ID == k.ID);

            bool a = true;

            foreach (Kutuphane item in kmc.Kutuphane)
            {
                if (a)
                {
                    List<Urunler> gecicit = new List<Urunler>();

                    gecicit.Add(db.Urunlers.FirstOrDefault(s => s.ID == item.Oyun_ID));

                    kmc.Urunler = gecicit;

                    a = false;
                }
                else
                {
                    List<Urunler> gecicit = (List<Urunler>)kmc.Urunler;

                    gecicit.Add(db.Urunlers.FirstOrDefault(s => s.ID == item.Oyun_ID));

                    kmc.Urunler = gecicit;
                }
            }


            kmc.Firma = db.Firmas.ToList().Where(s => s.Aktifmi == true);

            return View((KutuphaneModelClass)kmc);
        }
    }
}