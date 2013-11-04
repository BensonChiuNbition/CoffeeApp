using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CoffeeApp.Models
{
    public class Member
    {
        public int MemberID { get; set; }
        public string Name { get; set; }
        public string EMail{ get; set; }
        public string Address { get; set; }

        public virtual ICollection<Enrollment> Enrollments { get; set; }
    }
}