using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.WindowsAzure.StorageClient;

namespace CoffeeAppWebRole.Models
{
    public class Course : TableServiceEntity
    {
        public Course()
        {
            base.PartitionKey = "Cafe";
            base.RowKey = Guid.NewGuid().ToString();
            //this.CourseID = Guid.NewGuid().ToString();
        }

        public Course(string partitionKey, string rowKey)
        {
            base.PartitionKey = partitionKey;
            base.RowKey = rowKey;
            //this.CourseID = rowKey;
        }

        public string CourseID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string DateStart { get; set; }
        public string DateEnd { get; set; }
        public string NumOfStudent { get; set; }
        public string CourseDateTime { get; set; }
        public string Instructor { get; set; }
    }
}