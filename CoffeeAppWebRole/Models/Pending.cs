using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.WindowsAzure.StorageClient;

namespace CoffeeAppWebRole.Models
{
    public class Pending : TableServiceEntity
    {
        // A is sender, B is reciever
        public enum StatusB { Pending, Ignored };

        public Pending()
        {
            base.PartitionKey = "Cafe";
            base.RowKey = Guid.NewGuid().ToString();
        }

        public Pending(string partitionKey, string rowKey)
        {
            base.PartitionKey = partitionKey;
            base.RowKey = rowKey;
        }

        public string AMemberID { get; set; }
        public string BMemberID { get; set; }
        public string BStatus { get; set; }
    }
}