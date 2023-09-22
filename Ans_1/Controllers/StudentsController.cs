using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Ans_1.Models;
using Ans_1.Models.ViewModels;

namespace Ans_1.Controllers
{
    public class StudentsController : Controller
    {
        private readonly EnrollDbContext db = new EnrollDbContext();
        public ActionResult Index()
        {
            var students = db.Students.Include(s => s.Enrolls.Select(e => e.Course)).OrderByDescending(x => x.StudentId).ToList();
            return View(students);
        }
        public ActionResult AddNewCourse(int? id)
        {
            ViewBag.courses = new SelectList(db.Courses.ToList(), "CourseId", "CourseName", (id != null) ? id.ToString() : "");
            return PartialView("_addNewCourse");
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(StudentVM studentVM, int[] courseId)
        {
            if (ModelState.IsValid)
            {
                Student student = new Student
                {
                    StudentName = studentVM.StudentName,
                    BirthDate = studentVM.BirthDate,
                    Age = studentVM.Age,
                    StudentStatus = studentVM.StudentStatus
                };
                //for Image
                HttpPostedFileBase file = studentVM.PicturePath;
                if (file != null)
                {
                    string filePath = Path.Combine("/Images/", Guid.NewGuid().ToString() + Path.GetExtension(file.FileName));
                    file.SaveAs(Server.MapPath(filePath));
                    student.Picture = filePath;
                }

                //for course
                foreach (var item in courseId)
                {
                    Enroll enroll = new Enroll()
                    {
                        Student = student,
                        StudentId = student.StudentId,
                        CourseId = item,
                    };
                    db.Enrolls.Add(enroll);
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }
        public ActionResult Edit(int? id)
        {
            Student student = db.Students.First(x => x.StudentId == id);
            var studentCourse = db.Enrolls.Where(x => x.StudentId == id).ToList();

            StudentVM studentVM = new StudentVM()
            {
                StudentId = student.StudentId,
                StudentName = student.StudentName,
                Age = student.Age,
                BirthDate = student.BirthDate,
                Picture = student.Picture,
                StudentStatus = student.StudentStatus
            };
            if (studentCourse.Count() > 0)
            {
                foreach (var item in studentCourse)
                {
                    studentVM.Enrolls.Add(item.CourseId);
                }
            }
            return View(studentVM);
        }
        [HttpPost]
        public ActionResult Edit(StudentVM studentVM, int[] courseId)
        {
            if (ModelState.IsValid)
            {
                Student student = new Student()
                {
                    StudentId = studentVM.StudentId,
                    StudentName = studentVM.StudentName,
                    BirthDate = studentVM.BirthDate,
                    Age = studentVM.Age,
                    StudentStatus = studentVM.StudentStatus
                };
                //for Image
                HttpPostedFileBase file = studentVM.PicturePath;
                if (file != null)
                {
                    string filePath = Path.Combine("/Images/", Guid.NewGuid().ToString() + Path.GetExtension(file.FileName));
                    file.SaveAs(Server.MapPath(filePath));
                    student.Picture = filePath;
                }
                else
                {
                    student.Picture = studentVM.Picture;
                }
                //Course
                var existsCourseEntry = db.Enrolls.Where(x => x.StudentId == student.StudentId).ToList();
                //Delete
                foreach (var enroll in existsCourseEntry)
                {
                    db.Enrolls.Remove(enroll);
                }
                //save All Course
                foreach (var item in courseId)
                {
                    Enroll enroll = new Enroll()
                    {
                        StudentId = student.StudentId,
                        CourseId = item
                    };
                    db.Enrolls.Add(enroll);
                }
                db.Entry(student).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }

        public ActionResult Delete(int? id)
        {
            var student = db.Students.Find(id);
            var existsCourseEntry = db.Enrolls.Where(x => x.StudentId == id).ToList();

            foreach (var enroll in existsCourseEntry)
            {
                db.Enrolls.Remove(enroll);
            }
            db.Entry(student).State = EntityState.Deleted;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}