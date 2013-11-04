using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CoffeeApp.Models
{
    public class Enrollment
    {
        public int EnrollmentID { get; set; }
        public int CourseID { get; set; }
        public int MemberID { get; set; }

        public virtual Member Member { get; set; }
        public virtual Course Course { get; set; }
    }
}