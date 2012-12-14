// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InputHelper.cs" company="LiteBlog">
//   Copyright (c) 2012, LiteBlog. All Rights Reserved.
// </copyright>
// <summary>
//   The input helper.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace MvcLiteBlog.Extensions
{
    using System;
    using System.Web;
    using System.Web.Mvc;

    /// <summary>
    /// The input helper.
    /// </summary>
    public static class InputHelper
    {
        #region Public Methods and Operators

        /// <summary>
        /// The valued check box.
        /// </summary>
        /// <param name="helper">
        /// The helper.
        /// </param>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <returns>
        /// The System.Web.IHtmlString.
        /// </returns>
        public static IHtmlString ValuedCheckBox(this HtmlHelper helper, string name, string value)
        {
            string html = @"<input type=""checkbox"" name=""{0}"" value=""{1}""/>";
            return helper.Raw(string.Format(html, name, value));
        }

        #endregion
    }
}