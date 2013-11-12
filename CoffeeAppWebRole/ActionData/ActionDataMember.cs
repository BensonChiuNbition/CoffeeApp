using CoffeeAppWebRole.DAO;
using CoffeeAppWebRole.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CoffeeAppWebRole.ActionData
{
    public class ActionDataMember
    {
        private static ActionDataMember Instance;

        private static TableStorageContextMembers Context = new TableStorageContextMembers();

        protected ActionDataMember() {
        }

        public static ActionDataMember GetInstance()
        {
            if (Instance == null) {
                Instance = new ActionDataMember();
            }
            return Instance;
        }

        public Member GetMemberById(string userid)
        {
            return Context.GetMemberById(userid);
        }

        public void UpdateMember_DataAccessControl(string userid, string dac)
        {
            Member m = Context.GetMemberById(userid);
            m.DataAccessControl = dac;

            Context.UpdateMember(m);
        }

        public void CreateTable()
        {
            if (!TableStorageContextAccepteds.IsTableExisted())
            {
                TableStorageContextMembers.CreateTableIfNotExist();

                var member = new Member()
                {
                    RowKey = "1",
                    LoginName = "glam",
                    Password = "abc123",
                    NameEN = "GG Lam",
                    NameCH = "林喬拔",
                    Gender = "M",
                    DOB = "1970/01/01",
                    Phone = "3345678",
                    EMail = "abc@123.com",
                    MailAddress = "1 Peak Road",
                    Occupation = "Car Washer",
                    WorkAddress = "2 Peak Road",
                    ProfilePic = "/Storage/Images/image01322.jpg",
                    DataAccessControl = "{\"NameEN\":1,\"NameCH\":1,\"Gender\":1,\"DOB\":0,\"EMail\":1,\"MailAddress\":1,\"Phone\":1,\"Occupation\":0,\"WorkAddress\":0,\"ProfilePic\":1}"
                };
                var member2 = new Member()
                {
                    RowKey = "2",
                    LoginName = "jsc",
                    Password = "abc123",
                    NameEN = "Jessica C",
                    NameCH = "謝西嘉C",
                    Gender = "F",
                    DOB = "1971/01/01",
                    Phone = "4345678",
                    EMail = "abcd@123.com",
                    MailAddress = "2 Peak Road",
                    Occupation = "Model",
                    WorkAddress = "Ritz Carton",
                    ProfilePic = "/Storage/Images/image01323.jpg",
                    DataAccessControl = "{\"NameEN\":1,\"NameCH\":1,\"Gender\":1,\"DOB\":0,\"EMail\":1,\"MailAddress\":1,\"Phone\":1,\"Occupation\":0,\"WorkAddress\":0,\"ProfilePic\":1}"
                };
                var member3 = new Member()
                {
                    RowKey = "3",
                    LoginName = "jmt",
                    Password = "abc123",
                    NameEN = "Jimmy Tang",
                    NameCH = "鄧尖美",
                    Gender = "M",
                    DOB = "1972/11/01",
                    Phone = "4342378",
                    EMail = "jm@123.com",
                    MailAddress = "4 Peak Road",
                    Occupation = "Farmer",
                    WorkAddress = "Tai Po",
                    ProfilePic = "/Storage/Images/image013s3.jpg",
                    DataAccessControl = "{\"NameEN\":1,\"NameCH\":1,\"Gender\":1,\"DOB\":0,\"EMail\":1,\"MailAddress\":1,\"Phone\":1,\"Occupation\":0,\"WorkAddress\":0,\"ProfilePic\":1}"
                };
                var member4 = new Member()
                {
                    RowKey = "4",
                    LoginName = "tjd",
                    Password = "abc123",
                    NameEN = "Judy Tong",
                    NameCH = "湯珠滴",
                    Gender = "M",
                    DOB = "1990/12/01",
                    Phone = "4142378",
                    EMail = "jdd@123.com",
                    MailAddress = "10 Peak Road",
                    Occupation = "OL",
                    WorkAddress = "Central",
                    ProfilePic = "/Storage/Images/image013s3.jpg",
                    DataAccessControl = "{\"NameEN\":1,\"NameCH\":1,\"Gender\":1,\"DOB\":0,\"EMail\":1,\"MailAddress\":1,\"Phone\":1,\"Occupation\":0,\"WorkAddress\":0,\"ProfilePic\":1}"
                };
                Context.AddMember(member);
                Context.AddMember(member2);
                Context.AddMember(member3);
                Context.AddMember(member4);
            }
        }
    }
}