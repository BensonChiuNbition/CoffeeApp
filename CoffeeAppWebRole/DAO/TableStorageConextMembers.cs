

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
    public class TableStorageContextMembers : TableServiceContext
    {
        private const string MembersTableName = "Members";

        private static CloudStorageAccount CloudStorageAccount()
        {
            // Retrieve the storage account from the connection string.

            CloudStorageAccount storageAccount = Microsoft.WindowsAzure.CloudStorageAccount.Parse(
                CloudConfigurationManager.GetSetting("TableStorgeConnectionString"));
            return storageAccount;
        }

        public static void CreateTableIfNotExist()
        {
            CloudTableClient tableClient = CloudStorageAccount().CreateCloudTableClient();

            tableClient.CreateTableIfNotExist(MembersTableName);
        }

        public static Boolean IsTableExisted()
        {
            CloudTableClient tableClient = CloudStorageAccount().CreateCloudTableClient();
            return tableClient.DoesTableExist(MembersTableName);
        }

        public TableStorageContextMembers () : base(CloudStorageAccount().TableEndpoint.ToString(), CloudStorageAccount().Credentials){
            
        }

        public IQueryable<Member> Members
        {
            get { return this.CreateQuery<Member>(MembersTableName); }
        }

        public void AddMember(Member member)
        {
            AddObject(MembersTableName, member);
            SaveChangesWithRetries();
        }

        public void UpdateMember(Member member)
        {
            UpdateObject(member);
            SaveChangesWithRetries();
        }

        public Member GetMemberById(string id)
        {
            var query = from member in this.CreateQuery<Member>(MembersTableName)
                        where member.PartitionKey == "Cafe" && member.MemberID == id
                        select member;
            Member mb = null;
            foreach (var m in query)
            {
                mb = m;
            }
            return mb;
        }
    }
}