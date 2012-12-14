// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CustomProfile.cs" company="LiteBlog">
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
    /// The custom profile.
    /// </summary>
    public class CustomProfile : ProfileBase
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the ip address.
        /// </summary>
        [SettingsAllowAnonymous(true)]
        public string IPAddress
        {
            get
            {
                return (string)base["IPAddress"];
            }

            set
            {
                base["IPAddress"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        [SettingsAllowAnonymous(true)]
        public string Name
        {
            get
            {
                return (string)base["Name"];
            }

            set
            {
                base["Name"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the url.
        /// </summary>
        [SettingsAllowAnonymous(true)]
        public string Url
        {
            get
            {
                return (string)base["Url"];
            }

            set
            {
                base["Url"] = value;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The get profile.
        /// </summary>
        /// <param name="userName">
        /// The user name.
        /// </param>
        /// <returns>
        /// The LiteBlog.BlogEngine.CustomProfile.
        /// </returns>
        public static CustomProfile GetProfile(string userName)
        {
            return (CustomProfile)ProfileBase.Create(userName);
        }

        /// <summary>
        /// The get profile.
        /// </summary>
        /// <returns>
        /// The LiteBlog.BlogEngine.CustomProfile.
        /// </returns>
        public static CustomProfile GetProfile()
        {
            return (CustomProfile)HttpContext.Current.Profile;
        }

        #endregion
    }
}