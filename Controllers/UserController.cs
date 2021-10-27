using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Data.SqlClient;
using System.Web.Security;
using System.Web.Mvc;
using File_tracking_system;
using File_tracking_system.Models;
using Newtonsoft.Json;
using System.Security.Cryptography;
using System.IO;

namespace File_tracking_system.Controllers
{
    public class UserController : Controller
    {
        private file_trackingsystemEntities db = new file_trackingsystemEntities();



        // login 
        public ActionResult Login()
        {

            return View();

        }

        [HttpPost]
        public ActionResult Login(string Password, string UserID)
        {
            var password = "";
            bool IsExist = false;
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = @"data source=DESKTOP-4D697IJ;initial catalog=file_trackingsystem;user id=sa;password=Password123();Integrated Security=True";

            conn.Open();

            SqlCommand td = new SqlCommand(); // sqlcommand  make it possible to use sql queries in c# code
            SqlCommand px = new SqlCommand();

            td.CommandText = "select * from [User] where UserID ='" + UserID + "'";
            px.CommandText = "select * from [User] where UserID ='" + UserID + "'";
            string username = "";
            td.Connection = conn;
            SqlDataReader x = td.ExecuteReader();
             
            if (x.Read())
            {
                username = x.GetString(0);
                FormsAuthentication.SetAuthCookie(username, true); // set cookie username 
                password = x.GetString(1);  //get the user password from db if the user name is exist in that.  
                Session["Name"] = x.GetString(3);
                
                

                var tx = password;
                IsExist = true;
            }
            conn.Close();
            if (IsExist)  //if record exis in db , it will return true, otherwise it will return false  
            {
                if (Crypto.Decrypt(password).Equals(Password))
                {
                    return RedirectToAction("dashboard");
                }
                else
                {
                    TempData["alert"] = "wrong credentials , try again";
                    return View("Login");
                }

            }

            else
            {
                TempData["alert2"] = "user does not exist";
                return View("Login");
            }





            return View();
        }


        public ActionResult dashboard(){


            var px = db.mark_out.ToList();
            mark_out zx = new mark_out();
          //  List<px> K = new List<px>();
           
            var k = px.Count;
           

             

                    var x = JsonConvert.SerializeObject(k);
                    ViewBag.x = x;
                
               
            
                
              return View("dashboard");
        }
        public ActionResult Index()
        {
            return View(db.Users.ToList());
        }
           
        // GET: /User/
      

        // GET: /User/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: /User/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /User/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]

        public ActionResult Create(string Name, string Password, string Position, string UserID)
        {
            User user = new User();
            string x = Crypto.Encrypt(Password.ToString());
            if (ModelState.IsValid)
            {
                user.Name = Name;
                user.Password = x;
                user.Department = "records";
                user.Position = Position;
                user.UserID = UserID;

                db.Users.Add(user);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(user);
        }

        // GET: /User/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: /User/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        
        public ActionResult Edit(string Name,string Department,string Position,string UserID)
        {
           
                 User user  = new User();
                 user.Name = Name;
                 
                 user.Department = Department;
                 user.Position = Position;
                 user.UserID = UserID;


                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            
           return View(user);
        }


        // CREate new password
        public ActionResult pass()
        {
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = @"data source=DESKTOP-4D697IJ;initial catalog=file_trackingsystem;user id=sa;password=Password123();Integrated Security=True";
            conn.Open();

            SqlCommand td = new SqlCommand();
            td.CommandText = "select * from [User] where Name ='" + User.Identity.Name + "'";
            td.Connection = conn;

            SqlDataReader x = td.ExecuteReader();
            x.Read();
            string px = x.GetString(1);
            var tj = Crypto.Decrypt(px);
            string tad = x.GetString(4);
            string id = "";
            TempData["id"] = tad;
            TempData["pass"] = tj;
            return View();
        }
        [HttpGet]
        public ActionResult EditPas(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: /User/EditPa/
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]


        public ActionResult EditPas(string Password, string UserID, string Department, string Name, string Position)
        {

            User user = new User();
            if (Password != "")
            {
                string x = Crypto.Encrypt(Password.ToString());

                if (ModelState.IsValid)
                {

                    user.Password = x;
                    user.UserID = UserID;
                    user.Department = Department;
                    user.Name = Name;
                    user.Position = Position;


                    db.Entry(user).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            else
            {
                TempData["alertMessage"] = "ENter new password";
                return View("EditPas");
            }

            return View();
        }
        // GET: /User/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: /User/Delete/5
        [HttpPost, ActionName("Delete")]
        
        public ActionResult DeleteConfirmed(string id)
        {
            User user = db.Users.Find(id);
            db.Users.Remove(user);
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

        // security code to encrpty and decrpty
        public static class Crypto
        {
            public static string Encrypt(string encryptString)
            {
                string EncryptionKey = "0ram@1234xxxxxxxxxxtttttuuuuuiiiiio";  //we can change the code converstion key as per our requirement    
                byte[] clearBytes = Encoding.Unicode.GetBytes(encryptString);
                using (Aes encryptor = Aes.Create())
                {
                    Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] {      
            0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76      
        });
                    encryptor.Key = pdb.GetBytes(32);
                    encryptor.IV = pdb.GetBytes(16);
                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                        {
                            cs.Write(clearBytes, 0, clearBytes.Length);
                            cs.Close();
                        }
                        encryptString = Convert.ToBase64String(ms.ToArray());
                    }
                }
                return encryptString;
            }
            public static string Decrypt(string cipherText)
            {
                string EncryptionKey = "0ram@1234xxxxxxxxxxtttttuuuuuiiiiio";  //we can change the code converstion key as per our requirement, but the decryption key should be same as encryption key    
                cipherText = cipherText.Replace(" ", "+");
                byte[] cipherBytes = Convert.FromBase64String(cipherText);
                using (Aes encryptor = Aes.Create())
                {
                    Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] {      
            0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76      
        });
                    encryptor.Key = pdb.GetBytes(32);
                    encryptor.IV = pdb.GetBytes(16);
                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                        {
                            cs.Write(cipherBytes, 0, cipherBytes.Length);
                            cs.Close();
                        }
                        cipherText = Encoding.Unicode.GetString(ms.ToArray());
                    }
                }
                return cipherText;

            }

        }
    }
}
