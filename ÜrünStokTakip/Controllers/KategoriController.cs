using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ÜrünStokTakip.Models;

namespace ÜrünStokTakip.Controllers
{
    public class KategoriController : Controller
    {
        MVC_StokTakipEntities db = new MVC_StokTakipEntities();
        [Authorize(Roles = "A")]
        // GET: Kategori
        public ActionResult Index()
        {
            return View(db.Kategori.Where(x=>x.Durum==true).ToList());
        }
        [Authorize(Roles = "A")]
        public ActionResult Ekle()
        {
            return View();
        }
        [HttpPost]
        [Authorize(Roles = "A")]
        public ActionResult Ekle(Kategori data)
        {
            db.Kategori.Add(data);
            data.Durum = true;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        [Authorize(Roles = "A")]
        public ActionResult Sil(int id)
        {
            var kategori = db.Kategori.Where(x => x.Id == id).FirstOrDefault();
            db.Kategori.Remove(kategori);
            kategori.Durum = false;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult Guncelle()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Guncelle(Kategori data)
        {
            var guncelle = db.Kategori.Where(x => x.Id == data.Id).FirstOrDefault();
            guncelle.Açıklama = data.Açıklama;
            guncelle.Ad = data.Ad;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}