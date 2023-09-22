using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace Ans_1.Models.ViewModels
{
    public class StudentVM
    {
        public StudentVM()
        {
            this.Enrolls = new List<int>();
        }
        public int StudentId { get; set; }
        [Required, StringLength(50), Display(Name = "Student Name")]
        public string StudentName { get; set; }
        [Required, Display(Name = "Date of Birth"), Column(TypeName = "date"), DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime BirthDate { get; set; }
        public int Age { get; set; }
        public string Picture { get; set; }
        [Display(Name = "Picture")]
        public HttpPostedFileBase PicturePath { get; set; }
        [Display(Name = "Student Status")]
        public bool StudentStatus { get; set; }
        public List<int> Enrolls { get; set; }
    }
}