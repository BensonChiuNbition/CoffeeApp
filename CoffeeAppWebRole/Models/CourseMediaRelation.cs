using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.WindowsAzure.StorageClient;

namespace CoffeeAppWebRole.Models
{
    public class CourseMediaRelation : TableServiceEntity
    {
        public CourseMediaRelation()
        {
            base.PartitionKey = "Cafe";
            base.RowKey = Guid.NewGuid().ToString();
        }

        public CourseMediaRelation(string partitionKey, string rowKey)
        {
            base.PartitionKey = partitionKey;
            base.RowKey = rowKey;
        }

        public string CourseID { get; set; }
        public string MediaID { get; set; }
    }
}