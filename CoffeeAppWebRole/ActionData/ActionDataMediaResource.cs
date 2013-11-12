using CoffeeAppWebRole.DAO;
using CoffeeAppWebRole.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CoffeeAppWebRole.ActionData
{
    public class ActionDataMediaResource
    {
        private static ActionDataMediaResource Instance;

        private static TableStorageContextMediaResources Context = new TableStorageContextMediaResources();

        protected ActionDataMediaResource() {
        }

        public static ActionDataMediaResource GetInstance()
        {
            if (Instance == null) {
                Instance = new ActionDataMediaResource();
            }
            return Instance;
        }

        public IQueryable<MediaResource> GetMediaResources()
        {
            return Context.MediaResources;
        }

        public MediaResource GetMediaResourceById(string mediaResourceId)
        {
            return Context.GetMediaResourceById(mediaResourceId);
        }

        public void CreateTable()
        {
            if (!TableStorageContextMediaResources.IsTableExisted())
            {
                TableStorageContextMediaResources.CreateTableIfNotExist();

                var media = new MediaResource()
                {
                    RowKey = "v1111",
                    Description = "A testing video",
                    MediaType = MediaResource.TypeMedia.VideoHttp.ToString(),
                    Path = "http://www.youtube.com/embed/2Ao5b6uqI40"
                };
                var media2 = new MediaResource()
                {
                    RowKey = "v1112",
                    Description = "Easy：簡易咖啡拉花教學",
                    MediaType = MediaResource.TypeMedia.VideoHttp.ToString(),
                    Path = "http://www.youtube.com/embed/GSS2Oa2WhI8"
                };
                var media3 = new MediaResource()
                {
                    RowKey = "m1111",
                    Description = "Love Coffee",
                    MediaType = MediaResource.TypeMedia.PhotoHttp.ToString(),
                    Path = "http://upload.wikimedia.org/wikipedia/commons/f/f8/Love_Coffee.jpg"
                };
                var media4 = new MediaResource()
                {
                    RowKey = "m1112",
                    Description = "Black Coffee",
                    MediaType = MediaResource.TypeMedia.PhotoHttp.ToString(),
                    Path = "http://upload.wikimedia.org/wikipedia/commons/4/45/A_small_cup_of_coffee.JPG"
                };
                Context.AddMediaResource(media);
                Context.AddMediaResource(media2);
                Context.AddMediaResource(media3);
                Context.AddMediaResource(media4);
            }
        }
    }
}