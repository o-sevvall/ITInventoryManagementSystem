using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ITInventoryProject.Models;

namespace ITInventoryProject.Controllers
{
    public class PersonelsController : Controller
    {
        private OGMDatabase1Entities1 db = new OGMDatabase1Entities1();

        // GET: Personels Personel listesi görüntülendi.
        public ActionResult Index()
        {
            var personel = db.Personel.Include(p => p.Bolge).Include(p => p.Gorev).Include(p => p.Mudurluk).Include(p => p.Seflik);
            return View(personel.ToList());
        }

        //Belirli bir personelin detayları görüntülendi.
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Personel personel = db.Personel.Find(id);
            if (personel == null)
            {
                return HttpNotFound();
            }
            return View(personel);
        }

        //  Yeni bir personel oluşturma metodu
        public ActionResult Create()
        {
            ViewBag.BolgeID = new SelectList(db.Bolge, "BolgeID", "BolgeAd");
            ViewBag.GorevID = new SelectList(db.Gorev, "GorevID", "GorevAdi");
            ViewBag.MudurlukID = new SelectList(db.Mudurluk, "MudurlukID", "MudurlukAd");
            ViewBag.SeflikID = new SelectList(db.Seflik, "SeflikID", "SeflikAd");
            return View();
        }

       
        // Yeni bir personeli oluşturmak için gönderilen veriyi işleyen metod
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PersonelID,GorevID,BolgeID,MudurlukID,SeflikID,Ad,Soyad,AktifMi")] Personel personel)
        {
            if (ModelState.IsValid)
            {
                db.Personel.Add(personel);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.BolgeID = new SelectList(db.Bolge, "BolgeID", "BolgeAd", personel.BolgeID);
            ViewBag.GorevID = new SelectList(db.Gorev, "GorevID", "GorevAdi", personel.GorevID);
            ViewBag.MudurlukID = new SelectList(db.Mudurluk, "MudurlukID", "MudurlukAd", personel.MudurlukID);
            ViewBag.SeflikID = new SelectList(db.Seflik, "SeflikID", "SeflikAd", personel.SeflikID);
            return View(personel);
        }

        // Belirli bir personeli düzenleme metodu
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Personel personel = db.Personel.Find(id);
            if (personel == null)
            {
                return HttpNotFound();
            }
            ViewBag.BolgeID = new SelectList(db.Bolge, "BolgeID", "BolgeAd", personel.BolgeID);
            ViewBag.GorevID = new SelectList(db.Gorev, "GorevID", "GorevAdi", personel.GorevID);
            ViewBag.MudurlukID = new SelectList(db.Mudurluk, "MudurlukID", "MudurlukAd", personel.MudurlukID);
            ViewBag.SeflikID = new SelectList(db.Seflik, "SeflikID", "SeflikAd", personel.SeflikID);
            return View(personel);
        }

        // Bir personelin düzenlenmesi için gönderilen veriyi işleyen metod
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PersonelID,GorevID,BolgeID,MudurlukID,SeflikID,Ad,Soyad,AktifMi")] Personel personel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(personel).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.BolgeID = new SelectList(db.Bolge, "BolgeID", "BolgeAd", personel.BolgeID);
            ViewBag.GorevID = new SelectList(db.Gorev, "GorevID", "GorevAdi", personel.GorevID);
            ViewBag.MudurlukID = new SelectList(db.Mudurluk, "MudurlukID", "MudurlukAd", personel.MudurlukID);
            ViewBag.SeflikID = new SelectList(db.Seflik, "SeflikID", "SeflikAd", personel.SeflikID);
            return View(personel);
        }

        // Belirli bir personeli silme metodu
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Personel personel = db.Personel.Find(id);
            if (personel == null)
            {
                return HttpNotFound();
            }
            return View(personel);
        }

        //Bir personeli silme işlemini onaylayan metod
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Personel personel = db.Personel.Find(id);
            db.Personel.Remove(personel);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
