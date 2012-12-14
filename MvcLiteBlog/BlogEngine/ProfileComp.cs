// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProfileComp.cs" company="LiteBlog">
//   Copyright (c) 2012, LiteBlog. All Rights Reserved.
// </copyright>
// <summary>
//   The custom profile.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace MvcLiteBlog.BlogEngine
{
    using System;
    using System.Web;
    using System.Web.Profile;

    using LiteBlog.Common;
    using MvcLiteBlog.Helpers;

    /// <summary>
    /// The profile comp.
    /// </summary>
    public class ProfileComp
    {
        #region Public Methods and Operators

        /// <summary>
        /// The delete profiles.
        /// </summary>
        public static void DeleteProfiles()
        {
            TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById(SettingsComp.GetSettings().Timezone);
            DateTime inactiveDate = LocalTime.GetCurrentTime(tzi).AddDays(-30);
            ProfileManager.DeleteInactiveProfiles(ProfileAuthenticationOption.All, inactiveDate);
        }

        /// <summary>
        /// The get commenter profile.
        /// </summary>
        /// <returns>
        /// The LiteBlog.BlogEngine.CommenterProfile.
        /// </returns>
        public static CommenterProfile GetCommenterProfile()
        {
            CommenterProfile cmntProfile = new CommenterProfile();
            CustomProfile profile = CustomProfile.GetProfile();
            if (profile != null)
            {
                cmntProfile.Name = profile.Name;
                cmntProfile.Url = profile.Url;
            }

            return cmntProfile;
        }

        /// <summary>
        /// The get display name.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The System.String.
        /// </returns>
        public static string GetDisplayName(string id)
        {
            string name = string.Empty;
            CustomProfile profile = CustomProfile.GetProfile(id);
            if (profile != null)
            {
                name = profile.Name;
            }

            return name;
        }

        /// <summary>
        /// The set commenter profile.
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <param name="url">
        /// The url.
        /// </param>
        public static void SetCommenterProfile(string name, string url)
        {
            CustomProfile profile = CustomProfile.GetProfile();
            if (profile != null)
            {
                profile.Name = name;
                profile.Url = url;
                profile.Save();
            }
        }

        // public static void SetVisitorProfile()
        // {
        // ProfileBase profile = HttpContext.Current.Profile;
        // if (profile != null)
        // {
        // profile.SetPropertyValue("IPAddress", HttpContext.Current.Request.UserHostAddress);
        // }
        // }

        /// <summary>
        /// The set display name.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <param name="name">
        /// The name.
        /// </param>
        public static void SetDisplayName(string id, string name)
        {
            CustomProfile profile = CustomProfile.GetProfile(id);
            if (profile != null)
            {
                profile.Name = name;
                profile.Save();
            }
        }

        /// <summary>
        /// The set visitor profile.
        /// </summary>
        public static void SetVisitorProfile()
        {
            CustomProfile profile = CustomProfile.GetProfile();
            if (profile != null)
            {
                profile.IPAddress = HttpContext.Current.Request.UserHostAddress;
                profile.Save();
            }
        }

        #endregion
    }
}