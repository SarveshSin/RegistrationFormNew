using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using RegistrationFormNew.Models;

namespace RegistrationFormNew.Controllers
{
    public class StudentController : Controller
    {
        private JobDbEntities db = new JobDbEntities();

        // GET: Student
        public ActionResult Index()
        {
            return View(db.NewUsers.ToList());
        }

        // GET: Student/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NewUser newUser = db.NewUsers.Find(id);
            if (newUser == null)
            {
                return HttpNotFound();
            }
            return View(newUser);
        }

        // GET: Student/Create
        public ActionResult Create()
        {
            StudentData data = new StudentData();
            ViewBag.FileStatus = "";
            return View(data);
        }

        // POST: Student/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create([Bind(Include = "id,FirstName,LastName,PhoneNumber,JobExprience,JobTitle,CompanyName")] StudentData studentData, HttpPostedFileBase Resume)
        {
            if (ModelState.IsValid && Resume !=null)
            {
                String FileExt = Path.GetExtension(Resume.FileName).ToUpper();

                if (FileExt == ".PDF")
                {
                    Stream str = Resume.InputStream;
                    BinaryReader Br = new BinaryReader(str);
                    Byte[] FileDet = Br.ReadBytes((Int32)str.Length);
                    NewUser newUser = new NewUser
                    {
                    id=studentData.id,
                    FirstName=studentData.FirstName,
                    LastName=studentData.LastName,
                    PhoneNumber=studentData.PhoneNumber,
                    Resume= FileDet,
                    JobExprience=studentData.JobExprience,
                    JobTitle=studentData.JobTitle,
                    CompanyName=studentData.CompanyName
                    };
                    db.NewUsers.Add(newUser);
                    db.SaveChanges();

                }
                else
                {

                    ViewBag.FileStatus = "Invalid file format.";
                    return View(studentData);

                }
                
                return RedirectToAction("Index");
            }

            return View(studentData);
        }

        // GET: Student/Edit/5
        public ActionResult Edit(int? id)
        {
            
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NewUser newUser = db.NewUsers.Find(id);
            if (newUser == null)
            {
                return HttpNotFound();
            }
            StudentData studentData = new StudentData
            {
                id =newUser.id,
                FirstName = newUser.FirstName,
                LastName = newUser.LastName,
                 PhoneNumber = newUser.PhoneNumber,
                 Resume=newUser.Resume,
                 JobExprience=newUser.JobExprience,
                 JobTitle = newUser.JobTitle,
                 CompanyName=newUser.CompanyName
            };
            return View(studentData);
        }


        // POST: Student/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,FirstName,LastName,PhoneNumber,JobExprience,JobTitle,CompanyName")] StudentData studentData, HttpPostedFileBase Resume)
        {
            if (ModelState.IsValid)
            {
                String FileExt = Path.GetExtension(Resume.FileName).ToUpper();

                if (FileExt == ".PDF")
                {
                    Stream str = Resume.InputStream;
                    BinaryReader Br = new BinaryReader(str);
                    Byte[] FileDet = Br.ReadBytes((Int32)str.Length);
                    NewUser newUser = new NewUser
                    {
                        id = studentData.id,
                        FirstName = studentData.FirstName,
                        LastName = studentData.LastName,
                        PhoneNumber = studentData.PhoneNumber,
                        Resume = FileDet,
                        JobExprience = studentData.JobExprience,
                        JobTitle = studentData.JobTitle,
                        CompanyName = studentData.CompanyName
                    };
                    db.Entry(newUser).State = EntityState.Modified;
                    db.SaveChanges();

                }
                else
                {

                    ViewBag.FileStatus = "Invalid file format.";
                    return View(studentData);

                }
                
                return RedirectToAction("Index");
            }
            return View(studentData);
        }

        // GET: Student/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
                
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NewUser newUser = db.NewUsers.Find(id);
            if (newUser == null)
            {
                return HttpNotFound();
            }
            return View(newUser);
        }

        // POST: Student/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            NewUser newUser = db.NewUsers.Find(id);
            db.NewUsers.Remove(newUser);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public FileResult DownLoadFile(int id)
        {

            NewUser user = db.NewUsers.Where(x => x.id == id).FirstOrDefault();

            return File(user.Resume, "application/pdf", user.FirstName+"_Resume.pdf");

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
