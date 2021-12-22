using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ÜrünStokTakip.Models;

namespace ÜrünStokTakip.Controllers
{
    public class UrunController : Controller
    {
        // GET: Urun
        MVC_StokTakipEntities db = new MVC_StokTakipEntities();
        [Authorize]
        public ActionResult Index()
        {
            List<Urun> list = db.Urun.ToList();
            return View(list);
        }

        public ActionResult Ekle()
        {
            return View();
        }
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
    }
}