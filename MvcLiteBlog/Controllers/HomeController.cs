// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HomeController.cs" company="LiteBlog">
//   Copyright (c) 2012, LiteBlog. All Rights Reserved.
// </copyright>
// <summary>
//   The home controller.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace MvcLiteBlog.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.ServiceModel.Syndication;
    using System.Web.Mvc;
    using System.Web.Routing;
    using System.Xml;

    using LiteBlog.Common;

    using MvcLiteBlog.BlogEngine;
    using MvcLiteBlog.Helpers;
    using MvcLiteBlog.Models;

    /// <summary>
    /// The home controller.
    /// </summary>
    public class HomeController : Controller
    {
        #region Public Methods and Operators

        /// <summary>
        /// The about.
        /// </summary>
        /// <returns>
        /// The System.Web.Mvc.ActionResult.
        /// </returns>
        public ActionResult About()
        {
            return this.View();
        }

        /// <summary>
        /// The admin menu.
        /// </summary>
        /// <returns>
        /// The System.Web.Mvc.ActionResult.
        /// </returns>
        public ActionResult AdminMenu()
        {
            return this.PartialView("AdminMenuControl");
        }

        /// <summary>
        /// The admin title.
        /// </summary>
        /// <returns>
        /// The System.Web.Mvc.ActionResult.
        /// </returns>
        public ActionResult AdminTitle()
        {
            var model = PageComp.GetPublishedPages();
            Settings app = SettingsComp.GetSettings();
            this.ViewData["Title"] = app.Name;
            this.ViewData["IsAuthenticated"] = this.User.Identity.IsAuthenticated;
            this.ViewData["UserName"] = this.User.Identity.IsAuthenticated ? this.User.Identity.Name : string.Empty;
            return this.PartialView("AdminTitleControl", model);
        }

        /// <summary>
        /// The Atom
        /// </summary>
        /// <returns>
        /// The System.Web.Mvc.ActionResult.
        /// </returns>
        public ActionResult Atom()
        {
            if (CacheHelper.Get<AtomBool>(CacheType.Atom) == null)
            {
                this.WriteFeedXml(FeedType.ATOM);
                AtomBool atom = new AtomBool(); // hack to use cache
                CacheHelper.Put(CacheType.Atom, atom);
            }

            StatComp.IncrementFeeds();

            ContentResult cr = new ContentResult();
            cr.ContentType = "application/atom+xml";

            StreamReader sr = new StreamReader(this.Server.MapPath("~/App_Data/Atom.xml"));
            cr.Content = sr.ReadToEnd();

            return cr;
        }

        /// <summary>
        /// The error.
        /// </summary>
        /// <returns>
        /// The System.Web.Mvc.ActionResult.
        /// </returns>
        public ActionResult Error()
        {
            return this.View();
        }

        /// <summary>
        /// The index.
        /// </summary>
        /// <returns>
        /// The System.Web.Mvc.ActionResult.
        /// </returns>
        [HttpGet]
        public ActionResult Index()
        {
            this.ViewData.Model = BlogComp.GetDefaultPosts();
            return this.View();
        }

        /// <summary>
        /// The most popular tile.
        /// </summary>
        /// <returns>
        /// The System.Web.Mvc.ActionResult.
        /// </returns>
        public ActionResult MostPopularTile()
        {
            Stat stat = this.ControllerContext.HttpContext.Application["Stat"] as Stat;
            return this.PartialView(stat.PageVisits.Take(10));
        }

        /// <summary>
        /// The rss.
        /// </summary>
        /// <returns>
        /// The System.Web.Mvc.ActionResult.
        /// </returns>
        public ActionResult Rss()
        {
            if (CacheHelper.Get<RssBool>(CacheType.Rss) == null)
            {
                this.WriteFeedXml(FeedType.RSS);
                RssBool rss = new RssBool(); // hack to use cache
                CacheHelper.Put(CacheType.Rss, rss);
            }

            StatComp.IncrementFeeds();

            ContentResult cr = new ContentResult();
            cr.ContentType = "application/rss+xml";

            StreamReader sr = new StreamReader(this.Server.MapPath("~/App_Data/rss.xml"));
            cr.Content = sr.ReadToEnd();

            return cr;
        }

        /// <summary>
        /// The stat.
        /// </summary>
        /// <returns>
        /// The System.Web.Mvc.ActionResult.
        /// </returns>
        public ActionResult Stat()
        {
            // Increment monthly visitors
            ProfileComp.SetVisitorProfile();

            // Increment hits of the page
            StatComp.IncrementHits();

            Stat stat = System.Web.HttpContext.Current.Application["Stat"] as Stat;
            this.ViewData.Model = stat;
            return this.PartialView("StatControl");
        }

        /// <summary>
        /// The title.
        /// </summary>
        /// <returns>
        /// The System.Web.Mvc.ActionResult.
        /// </returns>
        public ActionResult Title()
        {
            Settings app = SettingsComp.GetSettings();
            var model = PageComp.GetPublishedPages();
            this.ViewData["Title"] = app.Name;
            return this.PartialView("TitleControl", model);
        }

        #endregion

        #region Methods

        /// <summary>
        /// The initialize.
        /// </summary>
        /// <param name="requestContext">
        /// The request context.
        /// </param>
        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);
        }

        /// <summary>
        /// The write feed xml.
        /// </summary>
        /// <param name="feedType">
        /// The feed Type.
        /// </param>
        private void WriteFeedXml(FeedType feedType)
        {
            Settings app = SettingsComp.GetSettings();
            string url = app.Url;
            if (url == string.Empty)
            {
                Uri uri = System.Web.HttpContext.Current.Request.Url;
                string dnsPath = uri.Scheme + "://" + uri.Host;
                if (!uri.IsDefaultPort)
                {
                    dnsPath = dnsPath + ":" + uri.Port.ToString();
                }

                string appPath = System.Web.HttpContext.Current.Request.ApplicationPath;
                url = dnsPath + appPath;
            }

            if (url.Substring(url.Length - 1, 1) != "/")
            {
                url += "/";
            }

            List<PostInfo> posts = BlogComp.GetPosts();
            List<SyndicationItem> items = new List<SyndicationItem>();

            int count = 0;
            foreach (PostInfo post in posts)
            {
                if (++count > 20)
                {
                    break;
                }

                SyndicationItem item = new SyndicationItem();
                item.Title = SyndicationContent.CreatePlaintextContent(post.Title);

                TimeSpan ts = DateTime.UtcNow - DateTime.Now;
                item.PublishDate = new DateTimeOffset(post.Time, ts);

                string url2 = string.Format("{0}Post/{1}", url, post.FileID);
                SyndicationLink link = SyndicationLink.CreateAlternateLink(new Uri(url2));

                item.Links.Add(link);

                Post post2 = PostComp.GetPost(post.FileID);
                item.Content = SyndicationContent.CreateHtmlContent(post2.Contents);

                string catName = CategoryComp.GetCategoryName(post.CatID);
                item.Categories.Add(new SyndicationCategory(catName));

                items.Add(item);
            }

            SyndicationFeed feed = new SyndicationFeed(items);

            feed.Title = SyndicationContent.CreatePlaintextContent(app.Name);
            feed.Links.Add(SyndicationLink.CreateAlternateLink(new Uri(url)));

            List<Author> authors = AuthorComp.GetAuthors();
            foreach (Author author in authors)
            {
                feed.Authors.Add(new SyndicationPerson(author.Email, author.Name, author.Url));
            }

            List<Category> catList = CategoryComp.GetCategories();
            foreach (Category cat in catList)
            {
                feed.Categories.Add(new SyndicationCategory(cat.Name));
            }

            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Encoding = System.Text.Encoding.UTF8;
            settings.OmitXmlDeclaration = true;
            settings.Indent = true;

            if (feedType == FeedType.RSS)
            {
                XmlWriter writer = XmlWriter.Create(this.Server.MapPath("~/App_Data/Rss.xml"), settings);
                feed.SaveAsRss20(writer);
                writer.Flush();
                writer.Close();
            }
            else
            {
                XmlWriter writer = XmlWriter.Create(this.Server.MapPath("~/App_Data/Atom.xml"), settings);
                feed.SaveAsAtom10(writer);
                writer.Flush();
                writer.Close();
            }
        }

        #endregion
    }
}