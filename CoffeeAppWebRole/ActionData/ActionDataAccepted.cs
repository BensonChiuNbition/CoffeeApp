using CoffeeAppWebRole.DAO;
using CoffeeAppWebRole.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CoffeeAppWebRole.ActionData
{
    public class ActionDataAccepted
    {
        private static ActionDataAccepted Instance;

        private static TableStorageContextAccepteds Context = new TableStorageContextAccepteds();

        protected ActionDataAccepted() {
        }

        public static ActionDataAccepted GetInstance()
        {
            if (Instance == null) {
                Instance = new ActionDataAccepted();
            }
            return Instance;
        }

        public void AddAccepted(string memberid1, string memberid2)
        {
            string[] s = { memberid1, memberid2 };
            Array.Sort(s, StringComparer.InvariantCulture);

            var accepted = new Accepted()
            {
                MemberID1 = s[0],
                MemberID2 = s[1]
            };
            Context.AddAccepted(accepted);
        }

        public Accepted GetAccepted(string memberid1, string memberid2)
        {
            string[] s = { memberid1, memberid2 };
            Array.Sort(s, StringComparer.InvariantCulture);

            return Context.GetAccepted(s[0], s[1]);
        }

        public IQueryable<Accepted> GetAccepteds(string id)
        {
            return Context.GetAccepteds(id);
        }

        public void CreateTable()
        {
            if (!TableStorageContextAccepteds.IsTableExisted())
            {
                TableStorageContextAccepteds.CreateTableIfNotExist();
            }

        }
    }
}