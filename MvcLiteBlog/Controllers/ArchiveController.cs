// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ArchiveController.cs" company="LiteBlog">
//   Copyright (c) 2012, LiteBlog. All Rights Reserved.
// </copyright>
// <summary>
//   The archive controller.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace MvcLiteBlog.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    using LiteBlog.Common;

    using MvcLiteBlog.BlogEngine;
    using MvcLiteBlog.Models;

    /// <summary>
    /// The archive controller.
    /// </summary>
    public class ArchiveController : Controller
    {
        #region Public Methods and Operators

        /// <summary>
        /// The archive.
        /// </summary>
        /// <returns>
        /// The System.Web.Mvc.ActionResult.
        /// </returns>
        public ActionResult Archive()
        {
            List<PostInfo> posts = BlogComp.GetPosts();
            IEnumerable<IGrouping<int, IGrouping<MonthYear, PostInfo>>> model =
                posts.GroupBy(p => new MonthYear { Month = p.Month, Year = p.Year }, new MonthYearComparer()).GroupBy(
                    group => group.Key.Year);

            return this.View(model);
        }

        /// <summary>
        /// The index.
        /// </summary>
        /// <param name="year">
        /// The year.
        /// </param>
        /// <param name="month">
        /// The month.
        /// </param>
        /// <returns>
        /// The System.Web.Mvc.ActionResult.
        /// </returns>
        public ActionResult Index(int year, int month)
        {
            string name = ArchiveComp.GetMonthName(month, year);
            if (name == string.Empty)
            {
                ArchiveMonth defMonth = ArchiveComp.GetDefaultMonth();
                year = defMonth.Year;
                month = defMonth.Month;
                name = defMonth.Name;
            }

            List<PostInfo> posts = BlogComp.GetPostsByMonth(year, month);
            this.ViewData.Model = posts;
            this.ViewData["ArchiveName"] = name;
            this.ViewBag.Year = year;
            return this.View();
        }

        /// <summary>
        /// The recent posts tile.
        /// </summary>
        /// <returns>
        /// The System.Web.Mvc.ActionResult.
        /// </returns>
        public ActionResult RecentPostsTile()
        {
            List<PostInfo> posts = BlogComp.GetPosts();
            this.ViewData.Model = posts.Take(3);
            return this.PartialView();
        }

        /// <summary>
        /// The widget.
        /// </summary>
        /// <returns>
        /// The System.Web.Mvc.ActionResult.
        /// </returns>
        public ActionResult Widget()
        {
            List<ArchiveMonth> archMonths = ArchiveComp.GetArchiveMonths();
            IEnumerable<IGrouping<int, ArchiveMonth>> model = archMonths.GroupBy(m => m.Year);
            return this.PartialView("ArchiveControl", model);
        }

        #endregion
    }
}