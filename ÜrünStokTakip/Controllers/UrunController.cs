using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using ÜrünStokTakip.Models;

namespace ÜrünStokTakip.Controllers
{
    public class UrunController : Controller
    {
        // GET: Urun
        MVC_StokTakipEntities db = new MVC_StokTakipEntities();
        [Authorize]
        public ActionResult Index(string ara)
        {
            var list = db.Urun.ToList();
            if(!string.IsNullOrEmpty(ara))
            {
                list = list.Where(x => x.Ad.Contains(ara)).ToList();
            }
            return View(list);
        }
        [Authorize(Roles ="A")]
        public ActionResult Ekle()
        {
            List<SelectListItem> deger1=(from x in db.Kategori.ToList()
                                        select new SelectListItem
                                        {
                                            Text=x.Ad,
                                            Value=x.Id.ToString()  
                                        }).ToList();
            ViewBag.ktgr = deger1;
            return View();
        }
        [Authorize(Roles = "A")]
        [HttpPost]
        public ActionResult Ekle(Urun Data,HttpPostedFileBase File)
        {
            string path = Path.Combine("~/Content/images" + File.FileName);
            File.SaveAs(Server.MapPath(path));
            Data.Resim = File.FileName.ToString();
            db.Urun.Add(Data);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        [Authorize(Roles = "A")]
        public ActionResult Sil(int id)
        {
            var urun = db.Urun.Where(x => x.Id == id).FirstOrDefault();
            db.Urun.Remove(urun);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        [Authorize(Roles = "A")]
        public ActionResult Guncelle(int id)
        {
            var guncelle = db.Urun.Where(x => x.Id == id).FirstOrDefault();
            List<SelectListItem> deger1 = (from x in db.Kategori.ToList()
                                           select new SelectListItem
                                           {
                                               Text = x.Ad,
                                               Value = x.Id.ToString()
                                           }).ToList();
            ViewBag.ktgr = deger1;
            return View(guncelle);

        }
        [Authorize(Roles = "A")]
        [HttpPost]
        public ActionResult Guncelle(Urun model,HttpPostedFileBase File)
        {
            var urun = db.Urun.Find(model.Id);
            if (File==null)
            {
                urun.Ad = model.Ad;
                urun.Fiyat = model.Fiyat;
                urun.Açıklama = model.Açıklama;
                urun.Stok = model.Stok;
                urun.Populer = model.Populer;
                urun.KategoriId = model.KategoriId;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                urun.Resim = File.FileName.ToString();
                urun.Ad = model.Ad;
                urun.Fiyat = model.Fiyat;
                urun.Açıklama = model.Açıklama;
                urun.Stok = model.Stok;
                urun.Populer = model.Populer;
                urun.KategoriId = model.KategoriId;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

        }
        [Authorize(Roles = "A")]
        public ActionResult KritikStok()
        {
            var kritik = db.Urun.Where(x => x.Stok <= 50).ToList();
            return View();
        }
        public PartialViewResult StokCount()
        {
            if(User.Identity.IsAuthenticated)
            {
                var count = db.Urun.Where(x => x.Stok < 50).Count();
                ViewBag.count = count;
                var azalan = db.Urun.Where(x => x.Stok == 50).Count();
                ViewBag.azalan = azalan;
            }
            return PartialView();
        }

        public ActionResult StokGrafik()
        {
            ArrayList deger1 = new ArrayList();
            ArrayList deger2 = new ArrayList();
            var veriler = db.Urun.ToList();
            veriler.ToList().ForEach(x => deger1.Add(x.Ad));
            veriler.ToList().ForEach(x => deger2.Add(x.Stok));
            var grafik = new Chart(width: 500, height: 500).AddTitle("Ürün Stok Grafiği").AddSeries(chartType: "Column", name: "Ad", xValue: deger1, yValues: deger2);
            return File(grafik.ToWebImage().GetBytes(), "images/jpg");

        }
    }
}