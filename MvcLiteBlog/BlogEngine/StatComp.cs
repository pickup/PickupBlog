// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StatComp.cs" company="LiteBlog">
//   Copyright (c) 2012, LiteBlog. All Rights Reserved.
// </copyright>
// <summary>
//   The stat comp.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace MvcLiteBlog.BlogEngine
{
    using System;
    using System.Collections.Generic;
    using System.Web;
    using System.Web.Profile;

    using LiteBlog.Common;

    using MvcLiteBlog.Helpers;

    /// <summary>
    /// The stat comp.
    /// </summary>
    public class StatComp
    {
        #region Public Methods and Operators

        /// <summary>
        /// The increment feeds.
        /// </summary>
        public static void IncrementFeeds()
        {
            Stat stat = (Stat)HttpContext.Current.Application["Stat"];
            TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById(SettingsComp.GetSettings().Timezone);
            string feedKey = Stat.GetFeedKey(LocalTime.GetCurrentTime(tzi));

            if (stat.Feeds.ContainsKey(feedKey))
            {
                stat.Feeds[feedKey] = stat.Feeds[feedKey] + 1;
            }
            else
            {
                stat.Feeds[feedKey] = 1;
            }

            HttpContext.Current.Application["Stat"] = stat;
        }

        /// <summary>
        /// The increment hits.
        /// </summary>
        public static void IncrementHits()
        {
            Stat stat = (Stat)HttpContext.Current.Application["Stat"];
            stat.Hits = stat.Hits + 1;
            HttpContext.Current.Application["Stat"] = stat;
        }

        /// <summary>
        /// The increment posts.
        /// </summary>
        /// <param name="fileID">
        /// The file id.
        /// </param>
        public static void IncrementPosts(string fileID)
        {
            Stat stat = (Stat)HttpContext.Current.Application["Stat"];
            if (stat.PageVisits.ContainsKey(fileID))
            {
                stat.PageVisits[fileID].Views = stat.PageVisits[fileID].Views + 1;
            }

            HttpContext.Current.Application["Stat"] = stat;
        }

        /// <summary>
        /// The increment visits.
        /// </summary>
        public static void IncrementVisits()
        {
            Stat stat = (Stat)HttpContext.Current.Application["Stat"];
            stat.Visits = stat.Visits + 1;
            HttpContext.Current.Application["Stat"] = stat;
        }

        /// <summary>
        /// to be called only from Application.Start() only
        /// </summary>
        public static void Initialize()
        {
            Stat stat = ConfigHelper.DataContext.StatData.Load();
            stat.Visitors = ProfileManager.GetNumberOfProfiles(ProfileAuthenticationOption.All);
            HttpContext.Current.Application["Stat"] = stat;
        }

        /// <summary>
        /// The save.
        /// </summary>
        /// <param name="stat">
        /// The stat.
        /// </param>
        public static void Save(Stat stat)
        {
            ConfigHelper.DataContext.StatData.Save(stat);
        }

        #endregion
    }
}