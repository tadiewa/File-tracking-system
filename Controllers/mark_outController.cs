using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using File_tracking_system;
using File_tracking_system.Models;
using System.Data.SqlClient;
using System.Web.Security;



namespace File_tracking_system.Controllers
{
    public class mark_outController : Controller
    {
        private file_trackingsystemEntities db = new file_trackingsystemEntities();

        // GET: /mark_out/
        mark_out tj = new mark_out();
        public ActionResult Index()
        {
            var x = db.mark_out.ToList();

            mark_out fj = new mark_out();
            IList<mark_out> tx = new List<mark_out>();

            foreach (var cx in x)
            {
                var expire = cx.Date_in;
                var aj = DateTime.Parse(expire).ToString("MM/dd/yyyy");
                var dout = cx.Date_out;
               
               
                if (DateTime.Now > DateTime.Parse(aj) && cx.Markout_File_status == "out")
                {
                    if (ModelState.IsValid)
                    {
                        cx.duration_status = "overdue";
                        fj = cx;
                      
                        db.Entry(fj).State = EntityState.Modified;
                        db.SaveChanges();

                        
                    }
                }
            }
            return View(db.mark_out.ToList());
        }

        // GET: /mark_out/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            mark_out mark_out = db.mark_out.Find(id);
            if (mark_out == null)
            {
                return HttpNotFound();
            }
            return View(mark_out);
        }
        // markout search

        public ActionResult search()
        {

            return View();
        }

        [HttpPost]
        public ActionResult search( string Filename ,string referencenumber)
        {

           // var px = db.mark_out.ToList();
            
            var floe = db.mark_out.Where(u => u.File_title == Filename && u.Reference_number == referencenumber.Trim()).ToList();
            if (floe == null)
            {
                return View();
            }
            var k = floe;
            return View(k);
        }

        public ActionResult charts() {
            var px = db.mark_out.ToList();
            var x = px.Count;
            var datez = DateTime.Now;
            return View();
        }

        // MARKOUT REPORTS

        public ActionResult reportbyuser()
        {
            return View();
        }

        [HttpPost]
        public ActionResult reportbyuser(string Dispatch_officer)
        {
            var px = db.mark_out.Where(u => u.Dispatch_officer == Dispatch_officer).ToList();

            var k = Dispatch_officer;
            Session["p"] = k;
            if (px!=null){

                
                return View("reportusermarkout", px);
            }
            else
            {
                return View();
            }
           
        }

        //report booked files

        public ActionResult bookedrepo()
        {
            var bookedlist = db.mark_out.Where(u=>u.duration_status=="booked");
            return View(bookedlist);
        }

        // report overdue
        public ActionResult overduerepo()
        {
            var bookedlist = db.mark_out.Where(u => u.duration_status == "overdue");
            return View(bookedlist);
        }

        //report  all out files
        public ActionResult outrepo()
        {
            var bookedlist = db.mark_out.Where(u => u.Markout_File_status == "out");
            return View(bookedlist);
        }

       // report all returned files
        public ActionResult inrepo()
        {
            var bookedlist = db.mark_out.Where(u => u.Markout_File_status == "in");
            return View(bookedlist);
        }


       // file status reports
        public ActionResult current()
        {
            var bookedlist = db.mark_out.Where(u => u.File_status == "current");
            return View(bookedlist);
        }

        public ActionResult sem_current()
        {
            var bookedlist = db.mark_out.Where(u => u.File_status == "sem_current");
            return View(bookedlist);
        }

        public ActionResult non_current()
        {
            var bookedlist = db.mark_out.Where(u => u.File_status == "non_current");
            return View(bookedlist);
        }
        // GET: /mark_out/Create
       
        public ActionResult Create()
        {
            return View();
        }

        // POST: /mark_out/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]

        public ActionResult Create(string Reference_number, string File_title,string Date_in, string Volume_number, string Requesting_officer, string File_status)
        {

            File px = new File();
            mark_out mk = new mark_out();
            // adding data to the object
           
           // mark.Request_number = Request_number;
            mk.Reference_number = Reference_number;
            mk.File_title = File_title;
            mk.Date_out = DateTime.Now.Date.ToString("yyyy-MM-dd");
            mk.Date_in = DateTime.Now.Date.AddDays(3).ToString("yyyy-MM-dd");
            mk.Volume_number = Volume_number;
            mk.Dispatch_officer = User.Identity.Name;
            mk.Requesting_officer = Requesting_officer;
            mk.Markout_File_status = "out";
            mk.duration_status ="booked";
            mk.File_status = File_status;
             //List <File> file = db.Files.ToList();
           /*var mark = db.mark_out.ToList();
            var  flox = from e in file 
                        join d in mark on e.Title   equals d.File_title into table1
                        from  d in table1.ToList()
                        select new mark_file {
                             
                            file = e,
                            mark =d
                        };*/
            
       
            // code to check from file table if the file exsits
            string k = "out";
                SqlConnection conn = new SqlConnection();
                conn.ConnectionString = @"data source=DESKTOP-4D697IJ;initial catalog=file_trackingsystem;user id=sa;password=vision2030;Integrated Security=True";

                conn.Open();

                SqlCommand cd = new SqlCommand();
                SqlCommand td = new SqlCommand();
                // code to check from file table if the file exsits using sql command queries
                cd.CommandText = "select * from [File] where Title ='" + File_title + "' and Volume_number = '" + Volume_number + "'";
            // code to check if the file is not booked
                td.CommandText = "select * from [mark_out] where File_title ='" + File_title + "' and Markout_File_status ='" + k +"' and Volume_number = '" + Volume_number + "'";
                cd.Connection = conn;
                td.Connection = conn;
                SqlDataReader x = cd.ExecuteReader();
                

                if (x.Read()==true  ){
                    // checking if the file is marked_out
                    x.Close();
                    SqlDataReader t = td.ExecuteReader();
                    if (t.Read() ==false)
                    {


                        db.mark_out.Add(mk);


                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        TempData["alert"] = "the file is booked";
                        return View("Create");
                    }

          }
            else
          {
              TempData["alert"] = "File does not exist";
              return View("Create");
          }

                return View();
        }

        
        
        // GET: /mark_out/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            mark_out mark_out = db.mark_out.Find(id);
            if (mark_out == null)
            {
                return HttpNotFound();
            }
            return View(mark_out);
        }

        // POST: /mark_out/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]

        public ActionResult Edit(int Request_number, string Reference_number, string File_title, string Date_in, string Date_out, string Volume_number, string Requesting_officer, string File_status, string Markout_File_status,string duration_status)
        {
            mark_out px = new mark_out();
            px.Request_number= Request_number;
            px.Date_in = Date_in;
            px.Date_out = Date_out;
            px.Reference_number = Reference_number;
            px.File_status = File_status;
            px.File_title = File_title;;
            px.Volume_number = Volume_number;
            px.Markout_File_status = Markout_File_status;
            px.Dispatch_officer =User.Identity.Name;
            px.Requesting_officer = Requesting_officer;
            px.duration_status = "--";
            if (ModelState.IsValid)
            {
                db.Entry(px).State = EntityState.Modified;
                db.SaveChanges ();
                return RedirectToAction("Index");
            }
            return View(px);
        }

        // Mark_out report 

   
        public ActionResult Report()
        {
            var expire = tj.Date_in;
            if (DateTime.Now >= DateTime.Parse(expire))
            {
                foreach (var x in tj.duration_status)
                {
                    tj.duration_status = "overdue";
                }
            }


            return View(db.mark_out.ToList());
        }

        // code to calculate expire time
        public ActionResult timerr()
        {

            var expire = tj.Date_in;
            if (DateTime.Now >= DateTime.Parse(expire))
            {
                foreach(var x in tj.duration_status)
                {
                    tj.duration_status = "overdue";
                }
            }
            return View();
        }

        // GET: /mark_out/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            mark_out mark_out = db.mark_out.Find(id);
            if (mark_out == null)
            {
                return HttpNotFound();
            }
            return View(mark_out);
        }

        // POST: /mark_out/Delete/5
        [HttpPost, ActionName("Delete")]
        
        public ActionResult DeleteConfirmed(int id)
        {
            mark_out mark_out = db.mark_out.Find(id);
            db.mark_out.Remove(mark_out);
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
