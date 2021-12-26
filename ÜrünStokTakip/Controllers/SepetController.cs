using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ÜrünStokTakip.Models;

namespace ÜrünStokTakip.Controllers
{
    public class SepetController : Controller
    {
        // GET: Sepet
        MVC_StokTakipEntities db = new MVC_StokTakipEntities();
        public ActionResult Index(decimal?Tutar)
        {
            if(User.Identity.IsAuthenticated)
            {
                var kullanıcıadı = User.Identity.Name;
                var kullanıcı = db.Kullanıcı.FirstOrDefault(x => x.Email == kullanıcıadı);
                var model = db.Sepet.Where(x => x.KullanıcıId == kullanıcı.Id).ToList();
                var kid = db.Sepet.FirstOrDefault(x => x.KullanıcıId == kullanıcı.Id);
                if(model!=null)
                {
                    if(kid==null)
                    {
                        ViewBag.Tutar = "Sepetinizde Ürün Bulunmamaktadır";
                    }
                    else if(kid!=null)
                    {
                        Tutar = db.Sepet.Where(x => x.KullanıcıId == kid.KullanıcıId).Sum(x => x.Urun.Fiyat * x.Adet);
                        ViewBag.Tutar = "Toplam Tutar =" + Tutar + "TL";
                    }
                    return View(model);
                }
            }
            return HttpNotFound();
        }
        public ActionResult SepeteEkle(int id)
        {
            if(User.Identity.IsAuthenticated)
            {
                var kullanıcıadı = User.Identity.Name;
                var model = db.Kullanıcı.FirstOrDefault(x => x.Email == kullanıcıadı);
                var u = db.Urun.Find(id);
                var sepet = db.Sepet.FirstOrDefault(x => x.KullanıcıId == model.Id && x.UrunId == id);
                if(model!=null)
                {
                    if(sepet!=null)
                    {
                        sepet.Adet++;
                        sepet.Fiyat = u.Fiyat * sepet.Adet;
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    var s = new Sepet
                    {
                        KullanıcıId = model.Id,
                        UrunId = u.Id,
                        Adet = 1,
                        Fiyat = u.Fiyat,
                        Tarih = DateTime.Now
                    };
                    db.Sepet.Add(s);
                    db.SaveChanges();
                    return RedirectToAction("Index");

                    }
                return View();
                }
            return HttpNotFound();
            }

        public ActionResult SepetCount(int?count)
        {
            if(User.Identity.IsAuthenticated)
            {
                var model = db.Kullanıcı.FirstOrDefault(x => x.Email == User.Identity.Name);
                count = db.Sepet.Where(x => x.KullanıcıId == model.Id).Count();
                ViewBag.count = count;
                if(count==0)
                {
                    ViewBag.count = "";
                }
                return PartialView();
            }
            return HttpNotFound();
        }
        public ActionResult Sil(int id)
        {
            var sil = db.Sepet.Find(id);
            db.Sepet.Remove(sil);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult HepsiniSil()
        {
            if (User.Identity.IsAuthenticated)
            {
                var kullanıcıadı = User.Identity.Name;
                var model = db.Kullanıcı.FirstOrDefault(x => x.Email == kullanıcıadı);
                var sil= db.Sepet.Where(x => x.KullanıcıId == model.Id);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return HttpNotFound();

        }
    }
    
}