// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LiteBlogController.cs" company="LiteBlog">
//   Copyright (c) 2012, LiteBlog. All Rights Reserved.
// </copyright>
// <summary>
//   The lite blog controller.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace MvcLiteBlog.Controllers
{
    using System.Web.Mvc;

    /// <summary>
    /// The lite blog controller.
    /// </summary>
    public class LiteBlogController : Controller
    {
        // GET: /LiteBlog/
        #region Public Methods and Operators

        /// <summary>
        /// The index.
        /// </summary>
        /// <returns>
        /// The System.Web.Mvc.ActionResult.
        /// </returns>
        [HttpGet]
        public ActionResult Index()
        {
            return this.View();
        }

        #endregion
    }
}