using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ÜrünStokTakip.Models;
using PagedList;
using PagedList.Mvc;

namespace ÜrünStokTakip.Controllers
{
    public class SatisController : Controller
    {
        // GET: Satis
        MVC_StokTakipEntities db = new MVC_StokTakipEntities();
        public ActionResult Index(int sayfa = 1)
        {
            if (User.Identity.IsAuthenticated)
            {
                var kullanıcıadı = User.Identity.Name;
                var kullanıcı = db.Kullanıcı.FirstOrDefault(x => x.Email == kullanıcıadı);
                var model = db.Satislar.Where(x => x.KullanıcıId == kullanıcı.Id).ToList().ToPagedList(sayfa, 5);
                return View(model);
            }
            return HttpNotFound();
        }
       
        public ActionResult SatinAl(int id)
        {
            var model = db.Sepet.FirstOrDefault(x => x.Id == id);
            return View(model);
        }
        [HttpPost]
        public ActionResult SatinAl2(int id)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    var model = db.Sepet.FirstOrDefault(x => x.Id == id);

                    var satis = new Satislar
                    {
                        KullanıcıId = model.KullanıcıId,
                        UrunId = model.UrunId,
                        Adet = model.Adet,
                        Resim = model.Resim,
                        Fiyat = model.Fiyat,
                        Tarih = model.Tarih,
                    };
                    db.Sepet.Remove(model);
                    db.Satislar.Add(satis);
                    db.SaveChanges();
                    ViewBag.islem = "Satın alma işlemi başarılı bir şekilde gerçekleşti";
                }
            }
            catch(Exception)
            {
                ViewBag.islem = "Satın alma işlemi başarısız.";
            }
            return View("islem");

        }
    }
}