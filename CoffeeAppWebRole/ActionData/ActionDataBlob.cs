using CoffeeAppWebRole.DAO;
using CoffeeAppWebRole.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CoffeeAppWebRole.ActionData
{
    public class ActionDataBlob
    {
        private static ActionDataBlob Instance;

        private static BlobStorageContextBlobs Context = new BlobStorageContextBlobs();

        protected ActionDataBlob() {
        }

        public static ActionDataBlob GetInstance()
        {
            if (Instance == null) {
                Instance = new ActionDataBlob();
            }
            return Instance;
        }

        public void UploadBlob(string blobId, string localFilePath)
        {
            Context.UploadBlob(blobId, localFilePath);
        }

        public byte[] GetBlob(string blobId)
        {
            return Context.GetBlob(blobId);
        }

        public void CreateBlobStorage()
        {
            BlobStorageContextBlobs.CreateBlobStorageIfNotExist();
            

        }
    }
}