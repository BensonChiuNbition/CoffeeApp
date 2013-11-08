using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.WindowsAzure.StorageClient;

namespace CoffeeAppWebRole.Models
{
    public class Enrollment : TableServiceEntity
    {
        public enum Status { None, Past, Current, Future };

        public Enrollment()
        {
            base.PartitionKey = "Cafe";
            base.RowKey = Guid.NewGuid().ToString();
            //this.EnrollmentID = Guid.NewGuid().ToString();
        }

        public Enrollment(string partitionKey, string rowKey)
        {
            base.PartitionKey = partitionKey;
            base.RowKey = rowKey;
            //this.EnrollmentID = rowKey;
        }

        //public string EnrollmentID { get; set; }
        public string MemberID { get; set; }
        public string CourseID { get; set; }
        public string EnrollStatus { get; set; }
    }
}