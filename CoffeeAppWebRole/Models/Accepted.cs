using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.WindowsAzure.StorageClient;

namespace CoffeeAppWebRole.Models
{
    public class Accepted : TableServiceEntity
    {
        public Accepted()
        {
            base.PartitionKey = "Cafe";
            base.RowKey = Guid.NewGuid().ToString();
            //this.AcceptedID = Guid.NewGuid().ToString();
        }

        public Accepted(string partitionKey, string rowKey)
        {
            base.PartitionKey = partitionKey;
            base.RowKey = rowKey;
            //this.AcceptedID = rowKey;
        }

        //public string AcceptedID { get; set; }
        public string MemberID1 { get; set; }
        public string MemberID2 { get; set; }
    }
}