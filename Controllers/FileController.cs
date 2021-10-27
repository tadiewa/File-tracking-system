using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using File_tracking_system;

namespace File_tracking_system.Controllers
{
    public class FileController : Controller
    {
        private file_trackingsystemEntities db = new file_trackingsystemEntities();

        // GET: /File/
        public ActionResult Index()
        {
            return View(db.Files.ToList());
        }

        // GET: /File/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            File file = db.Files.Find(id);
            if (file == null)
            {
                return HttpNotFound();
            }
            return View(file);
        }
        public ActionResult Filesearch()
        {

            return View();
        }

        [HttpPost]
        public ActionResult Filesearch(string Filename, string referencenumber)
        {

            // var px = db.mark_out.ToList();

            var floe = db.Files.Where(u => u.Title == Filename && u.Reference_number == referencenumber.Trim()).ToList();
            if (floe == null)
            {
                return View();
            }
            var k = floe;
            return View(k);
        }
        // GET: /mark_out/Create
       
        // GET: /File/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /File/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        
        public ActionResult Create (string Reference_number,string Title,string Volume_number, string File_status)
            
           
        {

            File file = new File();
            file.Reference_number = Reference_number;
            file.Title = Title;
            file.Volume_number = Volume_number;
            //file.Name_organisation = Name_organisation;
            file.Date_opened = DateTime.Now.Date.ToString("yyyy-MM-dd"); 
            file.Date_closed = "---";
            file.File_status = File_status; 
             
                db.Files.Add(file);
                db.SaveChanges();
                return RedirectToAction("Index");
            

            return View(file);
        }

        // GET: /File/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            File file = db.Files.Find(id);
            if (file == null)
            {
                return HttpNotFound();
            }
            return View(file);
        }

        // POST: /File/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
       
        public ActionResult Edit(int id ,string Reference_number,string Title,string Volume_number, string Date_opened,string File_status)
            
           
        {

            File file = new File();
            file.id = id;
            file.Reference_number = Reference_number;
            file.Title = Title;
            file.Volume_number = Volume_number;
            //file.Name_organisation = Name_organisation;
            file.Date_opened = Date_opened;
            file.Date_closed = "---";
            file.File_status = File_status;
            if (ModelState.IsValid)
            {
                db.Entry(file).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
                return View(file);
        }

        // GET: /File/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            File file = db.Files.Find(id);
            if (file == null)
            {
                return HttpNotFound();
            }
            return View(file);
        }

        // POST: /File/Delete/5
        [HttpPost, ActionName("Delete")]
        
        public ActionResult DeleteConfirmed(int id)
        {
            File file = db.Files.Find(id);
            db.Files.Remove(file);
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
