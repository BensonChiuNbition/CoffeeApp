using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.WindowsAzure.StorageClient;

namespace CoffeeAppWebRole.Models
{
    public class MediaResource : TableServiceEntity
    {
        public enum TypeMedia { PhotoCloud, VideoCloud, PhotoHttp, VideoHttp, Other };

        public MediaResource()
        {
            base.PartitionKey = "Cafe";
            base.RowKey = Guid.NewGuid().ToString();
            //this.MediaID = Guid.NewGuid().ToString();
        }

        public MediaResource(string partitionKey, string rowKey)
        {
            base.PartitionKey = partitionKey;
            base.RowKey = rowKey;
            //this.MediaID = rowKey;
        }

        public string MediaID { get; set; }
        public string MediaType{ get; set; }
        public string Path { get; set; }
        public string Description { get; set; }
    }
}