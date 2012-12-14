// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LocalTime.cs" company="LiteBlog">
//   Copyright (c) 2012, LiteBlog. All Rights Reserved.
// </copyright>
// <summary>
//   The local time.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace LiteBlog.Common
{
    using System;

    /// <summary>
    /// The local time.
    /// </summary>
    public class LocalTime
    {
        #region Public Methods and Operators

        /// <summary>
        /// The convert.
        /// </summary>
        /// <param name="time">
        /// The time.
        /// </param>
        /// <param name="tzi">
        /// The tzi.
        /// </param>
        /// <returns>
        /// The System.DateTime.
        /// </returns>
        public static DateTime Convert(DateTime time, TimeZoneInfo tzi)
        {
            return TimeZoneInfo.ConvertTime(time, tzi);
        }

        /// <summary>
        /// The get current time.
        /// </summary>
        /// <param name="tzi">
        /// The tzi.
        /// </param>
        /// <returns>
        /// The System.DateTime.
        /// </returns>
        public static DateTime GetCurrentTime(TimeZoneInfo tzi)
        {
            return TimeZoneInfo.ConvertTime(DateTime.Now, tzi);
        }

        #endregion
    }
}