// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MultiButtonAttribute.cs" company="LiteBlog">
//   Copyright (c) 2012, LiteBlog. All Rights Reserved.
// </copyright>
// <summary>
//   The multi button attribute.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace MvcLiteBlog.Attributes
{
    using System;
    using System.Reflection;
    using System.Web.Mvc;

    /// <summary>
    /// The multi button attribute.
    /// </summary>
    public class MultiButtonAttribute : ActionNameSelectorAttribute
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the form name.
        /// </summary>
        public string FormName { get; set; }

        /// <summary>
        /// Gets or sets the form value.
        /// </summary>
        public string FormValue { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The is valid name.
        /// </summary>
        /// <param name="controllerContext">
        /// The controller context.
        /// </param>
        /// <param name="actionName">
        /// The action name.
        /// </param>
        /// <param name="methodInfo">
        /// The method info.
        /// </param>
        /// <returns>
        /// The System.Boolean.
        /// </returns>
        public override bool IsValidName(
            ControllerContext controllerContext, string actionName, MethodInfo methodInfo)
        {
            if (!string.IsNullOrEmpty(this.FormName)
                && controllerContext.HttpContext.Request.Form[this.FormName] == this.FormValue)
            {
                return true;
            }

            return false;
        }

        #endregion
    }
}