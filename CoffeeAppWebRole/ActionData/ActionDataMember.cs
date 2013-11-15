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

        public void UpdateMember_DataAccessControl(
            string userid,
            string ne,
            string nc,
            string gd,
            string dob,
            string em,
            string ma,
            string ph,
            string oc,
            string wa,
            string dac)
        {
            Member m = Context.GetMemberById(userid);
            m.NameEN = ne;
            m.NameCH = nc;
            m.Gender = gd;
            m.DOB = dob;
            m.EMail = em;
            m.MailAddress = ma;
            m.Phone = ph;
            m.Occupation = oc;
            m.WorkAddress = wa;
            m.DataAccessControl = dac;

            if (Member.Status.Init.ToString().Equals(m.MemberStatus)){
                m.MemberStatus = Member.Status.Normal.ToString();
            }

            Context.UpdateMember(m);
        }

        public void AddMember(int mid, string ln, string pd, string ne, string nc)
        {
            var member = new Member()
            {
                MemberID = mid.ToString(),
                LoginName = ln,
                Password = pd,
                NameEN = ne,
                NameCH = nc,
                Gender = "",
                DOB = "",
                Phone = "",
                EMail = "",
                MailAddress = "",
                Occupation = "",
                WorkAddress = "",
                ProfilePic = "",
                DataAccessControl = "{\"NameEN\":1,\"NameCH\":1,\"Gender\":1,\"DOB\":0,\"EMail\":1,\"MailAddress\":1,\"Phone\":1,\"Occupation\":0,\"WorkAddress\":0,\"ProfilePic\":1}",
                MemberStatus = Member.Status.Init.ToString()
            };
            Context.AddMember(member);
        }

        public void CreateTable()
        {
            if (!TableStorageContextAccepteds.IsTableExisted())
            {
                TableStorageContextMembers.CreateTableIfNotExist();

                var member = new Member()
                {
                    MemberID = "1",
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
                    MemberID = "2",
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
                    MemberID = "3",
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
                    MemberID = "4",
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