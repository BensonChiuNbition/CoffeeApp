using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.WindowsAzure.StorageClient;

namespace CoffeeAppWebRole.Models
{
    public class Member : TableServiceEntity
    {
        public Member() {
            base.PartitionKey = "Cafe";
            base.RowKey = Guid.NewGuid().ToString();
            //this.MemberID = Guid.NewGuid().ToString();
            this.NameEN = "#";
        }

        public Member(string partitionKey, string rowKey, string nameEN) {
            base.PartitionKey = partitionKey;
            base.RowKey = rowKey;
            //this.MemberID = rowKey;
            this.NameEN = nameEN;
        }

        //public string MemberID { get; set; }
        public string NameEN { get; set; }
        public string NameCH { get; set; }
        public string Gender { get; set; }
        public string Password { get; set; }
        public string DOB { get; set; }
        public string Phone { get; set; }
        public string EMail { get; set; }
        public string MailAddress { get; set; }
        public string Occupation { get; set; }
        public string WorkAddress { get; set; }
        public string ProfilePic { get; set; }
        public string DataAccessControl { get; set; }

        //public virtual ICollection<Enrollment> Enrollments { get; set; }
    }
}