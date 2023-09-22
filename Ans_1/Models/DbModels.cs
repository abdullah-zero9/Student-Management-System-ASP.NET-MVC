using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace Ans_1.Models
{
    public class Student
    {
        public Student()
        {
            this.Enrolls = new List<Enroll>();
        }
        public int StudentId { get; set; }
        [Required, StringLength(50), Display(Name = "Student Name")]
        public string StudentName { get; set; }
        [Required, Display(Name = "Date of Birth"), Column(TypeName = "date"), DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime BirthDate { get; set; }
        public int Age { get; set; }
        public string Picture { get; set; }
        [Display(Name = "Student Status")]
        public bool StudentStatus { get; set; }
        //nav
        public ICollection<Enroll> Enrolls { get; set; }
    }
    public class Course
    {
        public Course()
        {
            this.Enrolls = new List<Enroll>();
        }
        public int CourseId { get; set; }
        [Required, StringLength(50), Display(Name = "Course Title")]
        public string CourseName { get; set; }
        //nav
        public ICollection<Enroll> Enrolls { get; set; }
    }
    public class Enroll
    {
        public int EnrollId { get; set; }
        [ForeignKey("Student")]
        public int StudentId { get; set; }
        [ForeignKey("Course")]
        public int CourseId { get; set; }
        //nav
        public virtual Student Student { get; set; }
        public virtual Course Course { get; set; }
    }
    public class EnrollDbContext : DbContext
    {
        public DbSet<Student> Students { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Enroll> Enrolls { get; set; }
    }
}