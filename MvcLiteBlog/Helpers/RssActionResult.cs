// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RssActionResult.cs" company="LiteBlog">
//   Copyright (c) 2012, LiteBlog. All Rights Reserved.
// </copyright>
// <summary>
//   The home controller.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace MvcLiteBlog.Helpers
{
    using System.ServiceModel.Syndication;
    using System.Web.Mvc;
    using System.Xml;

    /// <summary>
    /// The rss action result.
    /// </summary>
    public class RssActionResult : ActionResult
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the feed.
        /// </summary>
        public SyndicationFeed Feed { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The execute result.
        /// </summary>
        /// <param name="context">
        /// The context.
        /// </param>
        public override void ExecuteResult(ControllerContext context)
        {
            context.HttpContext.Response.ContentType = "application/rss+xml";

            Rss20FeedFormatter rssFormatter = new Rss20FeedFormatter(this.Feed);
            using (XmlWriter writer = XmlWriter.Create(context.HttpContext.Response.Output))
            {
                rssFormatter.WriteTo(writer);
            }
        }

        #endregion
    }
}