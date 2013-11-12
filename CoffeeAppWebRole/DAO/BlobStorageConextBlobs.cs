

using CoffeeAppWebRole.Models;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace CoffeeAppWebRole.DAO
{
    public class BlobStorageContextBlobs : TableServiceContext
    {
        private const string BlobStorageContainerName = "blobscontainer";

        private static CloudStorageAccount CloudStorageAccount()
        {
            // Retrieve the storage account from the connection string.

            CloudStorageAccount storageAccount = Microsoft.WindowsAzure.CloudStorageAccount.Parse(
                CloudConfigurationManager.GetSetting("TableStorgeConnectionString"));
            return storageAccount;
        }

        public static void CreateBlobStorageIfNotExist()
        {
            // Create the blob client.
            CloudBlobClient blobClient = CloudStorageAccount().CreateCloudBlobClient();

            // Retrieve a reference to a container. 
            CloudBlobContainer container = blobClient.GetContainerReference(BlobStorageContainerName);

            // Create the container if it doesn't already exist.
            container.CreateIfNotExist();

            container.SetPermissions(
                new BlobContainerPermissions
                {
                    PublicAccess = BlobContainerPublicAccessType.Blob
                });
        }

        public BlobStorageContextBlobs () : base(CloudStorageAccount().TableEndpoint.ToString(), CloudStorageAccount().Credentials){
            
        }

        public void UploadBlob(string blobId, string localFilePath)
        {
            // Create the blob client.
            CloudBlobClient blobClient = CloudStorageAccount().CreateCloudBlobClient();

            // Retrieve a reference to a container. 
            CloudBlobContainer container = blobClient.GetContainerReference(BlobStorageContainerName);


            // Retrieve reference to a blob named "myblob".
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(blobId);
            
            // Create or overwrite the "myblob" blob with contents from a local file.
            using (var fileStream = System.IO.File.OpenRead(localFilePath))
            {
                blockBlob.UploadFromStream(fileStream);
            }
        }

        public byte[] GetBlob(string blobId)
        {
            // Create the blob client.
            CloudBlobClient blobClient = CloudStorageAccount().CreateCloudBlobClient();

            // Retrieve a reference to a container. 
            CloudBlobContainer container = blobClient.GetContainerReference(BlobStorageContainerName);


            // Retrieve reference to a blob named "myblob".
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(blobId);

            byte[] byteArray;
            try
            {
                byteArray = blockBlob.DownloadByteArray();
            }
            catch (Exception e)
            {
                byteArray = null;
            }
            return byteArray;
        }

        /*
        public void ListEnrollments()
        {
            var query = from enrollment in this.CreateQuery<Enrollment>(EnrollmentsTableName)
                        where enrollment.PartitionKey == "Cafe"
                        select enrollment;
            
            foreach (var n in query)
            {
                Debug.WriteLine("c: {0} {1} {2}", n.RowKey, n.CourseID, n.MemberID, n.EnrollStatus);
            }

        }
         * */
    }
}