// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ArchiveComp.cs" company="LiteBlog">
//   Copyright (c) 2012, LiteBlog. All Rights Reserved.
// </copyright>
// <summary>
//   The archive comp.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace MvcLiteBlog.BlogEngine
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using LiteBlog.Common;
    using LiteBlog.Common.Contracts;

    using MvcLiteBlog.Helpers;

    /// <summary>
    /// The archive comp.
    /// </summary>
    public class ArchiveComp
    {
        #region Public Methods and Operators

        /// <summary>
        /// The decrement.
        /// </summary>
        /// <param name="month">
        /// The month.
        /// </param>
        /// <param name="year">
        /// The year.
        /// </param>
        public static void Decrement(int month, int year)
        {
            IArchiveData data = ConfigHelper.DataContext.ArchiveData;
            data.ChangeCount(ArchiveMonth.GetArchiveID(month, year).ToString(), -1);
        }

        /// <summary>
        /// The get archive months.
        /// </summary>
        /// <returns>
        /// The System.Collections.Generic.List`1[T -&gt; LiteBlog.Common.ArchiveMonth].
        /// </returns>
        public static List<ArchiveMonth> GetArchiveMonths()
        {
            List<ArchiveMonth> months = CacheHelper.Get<List<ArchiveMonth>>(CacheType.Archive);
            if (months == null)
            {
                IArchiveData data = ConfigHelper.DataContext.ArchiveData;
                months = data.GetArchiveMonths();
                CacheHelper.Put(CacheType.Archive, months);
            }

            return months;
        }

        /// <summary>
        /// The get default month.
        /// </summary>
        /// <returns>
        /// The LiteBlog.Common.ArchiveMonth.
        /// </returns>
        public static ArchiveMonth GetDefaultMonth()
        {
            return GetArchiveMonths().First<ArchiveMonth>();
        }

        /// <summary>
        /// The get month name.
        /// </summary>
        /// <param name="month">
        /// The month.
        /// </param>
        /// <param name="year">
        /// The year.
        /// </param>
        /// <returns>
        /// The System.String.
        /// </returns>
        public static string GetMonthName(int month, int year)
        {
            var months = from archMonth in GetArchiveMonths()
                         where archMonth.Month == month && archMonth.Year == year
                         select archMonth;

            if (months.Count<ArchiveMonth>() == 1)
            {
                return months.First<ArchiveMonth>().Name;
            }

            return string.Empty;
        }

        /// <summary>
        /// The increment.
        /// </summary>
        /// <param name="month">
        /// The month.
        /// </param>
        /// <param name="year">
        /// The year.
        /// </param>
        public static void Increment(int month, int year)
        {
            IArchiveData data = ConfigHelper.DataContext.ArchiveData;
            if (string.IsNullOrEmpty(GetMonthName(month, year)))
            {
                data.Create(new ArchiveMonth(month, year));
            }
            else
            {
                data.ChangeCount(ArchiveMonth.GetArchiveID(month, year).ToString(), 1);
            }
        }

        #endregion
    }
}