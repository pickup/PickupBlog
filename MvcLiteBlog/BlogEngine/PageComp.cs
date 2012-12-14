using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LiteBlog.Common;
using MvcLiteBlog.Helpers;

namespace MvcLiteBlog.BlogEngine
{
    public class PageComp
    {
        public static Page Load(string fileId)
        {
            // Check from published pages
            var qry = from p in GetPublishedPages()
                      where p.FileId == fileId
                      select p;

            Page page = qry.SingleOrDefault();

            // if not in cache, get directly
            if (page == null)
                page = ConfigHelper.DataContext.PageData.Load(fileId);

            return page;
        }

        public static void Save(Page page)
        {
            ConfigHelper.DataContext.PageData.Save(page);
        }

        public static List<Page> GetPages()
        {
            return ConfigHelper.DataContext.PageData.GetPages();
        }

        public static List<Page> GetPublishedPages()
        {
            List<Page> pages = CacheHelper.Get<List<Page>>(CacheType.Pages);
            if (pages == null)
            {
                pages = ConfigHelper.DataContext.PageData.GetPublishedPages();
                CacheHelper.Put(CacheType.Pages, pages );
            }
            return pages;
        }

        public static void Delete(string fileId)
        {
            ConfigHelper.DataContext.PageData.Delete(fileId);
        }

        public static void Publish(Page page)
        {
            string oldFileId = page.FileId;
            Delete(oldFileId);

            string newFileId = page.Title.Replace(" ", "");
            page.FileId = CreateUniqueId(newFileId);
            page.Published = true;
            Save(page);
        }

       private static string CreateUniqueId(string fileId)
        {
            if (ConfigHelper.DataContext.PageData.IsUniqueId(fileId))
                return fileId;

            int uid = 1;
            string uniqueId = fileId + "-" + uid.ToString();
            while (!ConfigHelper.DataContext.PageData.IsUniqueId(uniqueId))
            {
                ++uid;
                uniqueId = fileId + "-" + uid.ToString();
            }
            return uniqueId;
        }
    }
}