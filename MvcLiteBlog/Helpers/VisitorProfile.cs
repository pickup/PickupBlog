// --------------------------------------------------------------------------------------------------------------------
// <copyright file="VisitorProfile.cs" company="LiteBlog">
//   Copyright (c) 2012, LiteBlog. All Rights Reserved.
// </copyright>
// <summary>
//   The custom profile.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace MvcLiteBlog.Helpers
{
    using System;
    using System.Web;
    using System.Web.Profile;

    using LiteBlog.Common;

    /// <summary>
    /// The visitor profile.
    /// </summary>
    public class VisitorProfile
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the ip.
        /// </summary>
        public string IP { get; set; }

        #endregion
    }
}