using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml.Linq;
using LiteBlog.Common;
using LiteBlog.Common.Contracts;

namespace LiteBlog.XmlLayer
{
    public class PageData : IPageData
    {

        /// <summary>
        /// Gets the path.
        /// </summary>
        internal static string Path
        {
            get
            {
                return DataContext.Path + "Pages";
            }
        }

        // obsolete
        internal static string GetPath(string id)
        {
            return string.Format("{0}\\{1}.xml", Path, id);
        }

        public Page Load(string fileId)
        {
            string filePath = GetPath(fileId);
            Page page = new Page();
            page.FileId = fileId;

            XDocument doc = XDocument.Load(string.Format(filePath, fileId));
            XElement elem = doc.Root;
            page.Title = (string)elem.Attribute("Title");
            page.Body = HttpContext.Current.Server.HtmlDecode(elem.Value);

            return page;
        }

        public void Save(Page page)
        {
            XDocument doc = XDocument.Parse("<Page></Page>");
            doc.Root.SetAttributeValue("FileId", page.FileId);
            doc.Root.SetAttributeValue("Title", page.Title);
            doc.Root.SetValue(HttpContext.Current.Server.HtmlEncode(page.Body));
            doc.Root.SetAttributeValue("Published", page.Published);

            doc.Save(GetPath(page.FileId));
        }


        public List<Page> GetPages()
        {
            string folder = Path;
            // get all pages
            List<Page> pages = new List<Page>();
            foreach (string filePath in Directory.GetFiles(folder))
            {
                XDocument doc = XDocument.Load(filePath);
                Page page = new Page();
                page.Title = (string)doc.Root.Attribute("Title");
                page.FileId = (string)doc.Root.Attribute("FileId");
                page.Body = HttpContext.Current.Server.HtmlDecode(doc.Root.Value);
                pages.Add(page);
            }
            return pages;
        }

        public void Delete(string fileId)
        {
            string filePath = GetPath(fileId);
            if (File.Exists(filePath))
                File.Delete(filePath);
        }


        public bool IsUniqueId(string fileId)
        {
            var qry = from p in this.GetPages()
                      where p.FileId == fileId
                      select p;

            Page page = qry.SingleOrDefault();

            return page == null;
        }


        public List<Page> GetPublishedPages()
        {
            string folder = Path;
            // get all pages
            List<Page> pages = new List<Page>();
            foreach (string filePath in Directory.GetFiles(folder))
            {
                XDocument doc = XDocument.Load(filePath);
                Page page = new Page();
                page.Title = (string)doc.Root.Attribute("Title");
                page.FileId = (string)doc.Root.Attribute("FileId");
                page.Body = HttpContext.Current.Server.HtmlDecode(doc.Root.Value);
                if (doc.Root.Attribute("Published") != null)
                    page.Published = (bool)doc.Root.Attribute("Published");
                if (page.Published)
                    pages.Add(page);
            }
            return pages;
        }
    }
}
