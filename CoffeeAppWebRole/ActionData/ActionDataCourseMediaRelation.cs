using CoffeeAppWebRole.DAO;
using CoffeeAppWebRole.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CoffeeAppWebRole.ActionData
{
    public class ActionDataCourseMediaRelation
    {
        private static ActionDataCourseMediaRelation Instance;

        private static TableStorageContextCourseMediaRelations Context = new TableStorageContextCourseMediaRelations();

        protected ActionDataCourseMediaRelation() {
        }

        public static ActionDataCourseMediaRelation GetInstance()
        {
            if (Instance == null) {
                Instance = new ActionDataCourseMediaRelation();
            }
            return Instance;
        }

        public CourseMediaRelation GetCourseMediaRelationByCourseIdAndMediaId(string courseId, string mediaId)
        {
            return Context.GetCourseMediaRelationByCourseIdAndMediaId(courseId, mediaId);
        }
        public IQueryable<CourseMediaRelation> GetCourseMediaRelationByCourseId(string courseId)
        {
            return Context.GetCourseMediaRelationsByCourseId(courseId);
        }

        public IQueryable<CourseMediaRelation> GetCourseMediaRelatiosByMediaId(string mediaId)
        {
            return Context.GetCourseMediaRelationsByMediaId(mediaId);
        }

        public void CreateTable()
        {
            if (!TableStorageContextCourseMediaRelations.IsTableExisted())
            {
                TableStorageContextCourseMediaRelations.CreateTableIfNotExist();

                var courseMediaRelation = new CourseMediaRelation()
                {
                    CourseID = "COFB09",
                    MediaID = "v1111"
                };
                var courseMediaRelation2 = new CourseMediaRelation()
                {
                    CourseID = "COFB09",
                    MediaID = "m1111"
                };
                Context.AddCourseMediaRelation(courseMediaRelation);
                Context.AddCourseMediaRelation(courseMediaRelation2);
            }
        }
    }
}