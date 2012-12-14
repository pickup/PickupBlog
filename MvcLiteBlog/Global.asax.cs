// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Global.asax.cs" company="LiteBlog">
//   Copyright (c) 2012, LiteBlog. All Rights Reserved.
// </copyright>
// <summary>
//   The mvc application.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace MvcLiteBlog
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Profile;
    using System.Web.Routing;
    using System.Web.Security;
    using LiteBlog.Common;
    using MvcLiteBlog.BlogEngine;
    using MvcLiteBlog.Helpers;

    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    /// <summary>
    /// The mvc application.
    /// </summary>
    public class MvcApplication : System.Web.HttpApplication
    {
        #region Public Methods and Operators

        /// <summary>
        /// The register routes.
        /// </summary>
        /// <param name="routes">
        /// The routes.
        /// </param>
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Rss",
                "Rss",
                new { controller = "Home", action = "Rss" });

            routes.MapRoute(
                "Category",
                "{id}",
                new { controller = "Category", action = "Index" },
                new { id = new KeywordConstraint() });

            routes.MapRoute(
                "Post",
                "Post/{id}",
                new { controller = "Post", action = "Index" },
                new { id = new KeywordConstraint2() });

            routes.MapRoute(
                "Archive",
                "{year}/{month}",
                new { controller = "Archive", action = "Index" },
                new { year = new NumberConstraint(), month = new NumberConstraint() });

            routes.MapRoute(
                "Default",
                "{controller}/{action}/{id}",
                new { controller = "Home", action = "Index", id = UrlParameter.Optional });
        }

        /// <summary>
        /// The profile_ on migrate anonymous.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="args">
        /// The args.
        /// </param>
        public void Profile_OnMigrateAnonymous(object sender, ProfileMigrateEventArgs args)
        {
            ProfileManager.DeleteProfile(args.AnonymousID);
            AnonymousIdentificationModule.ClearAnonymousIdentifier();
        }

        #endregion

        #region Methods

        /// <summary>
        /// The application_ begin request.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
        }

        /// <summary>
        /// The application_ end.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        protected void Application_End(object sender, EventArgs e)
        {
            Stat stat = (Stat)this.Application["Stat"];

            // Store directly as cache is gone:
            StatComp.Save(stat);

            List<ServiceItem> svcList = (List<ServiceItem>)this.Application["Service"];

            // Store directly as cache is gone:
            ServiceComp.Save(svcList);
        }

        /// <summary>
        /// The application_ start.
        /// </summary>
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RegisterRoutes(RouteTable.Routes);

            // Initialize membership provider dynamically
            // This is a hack to initialize the provider parameters dynamically
            NameValueCollection config = new NameValueCollection();
            config["DataPath"] = ConfigHelper.DataPath;
            config["Timezone"] = SettingsComp.GetSettings().Timezone;
            config["AppName"] = string.Empty;
            Membership.Provider.Initialize("XmlMemProv", config);
            ProfileManager.Provider.Initialize("XmlProfProv", config);

            ServiceComp.Initialize();
            StatComp.Initialize();
        }

        /// <summary>
        /// The session_ start.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        protected void Session_Start(object sender, EventArgs e)
        {
            ServiceComp.Run();
            StatComp.IncrementVisits();

            // Commenting below as Anonymous profile initialization is not happening
            // ProfileComp.SetVisitorProfile();
        }

        #endregion
    }
}