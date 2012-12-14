// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CommenterProfile.cs" company="LiteBlog">
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
    /// The commenter profile.
    /// </summary>
    public class CommenterProfile
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the ip.
        /// </summary>
        public string IP { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the url.
        /// </summary>
        public string Url { get; set; }

        #endregion
    }
}