// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MonthYearComparer.cs" company="LiteBlog">
//   Copyright (c) 2012, LiteBlog. All Rights Reserved.
// </copyright>
// <summary>
//   The month year comparer.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace MvcLiteBlog.Models
{
    using System.Collections.Generic;

    /// <summary>
    /// The month year comparer.
    /// </summary>
    public class MonthYearComparer : IEqualityComparer<MonthYear>
    {
        #region Public Methods and Operators

        /// <summary>
        /// The equals.
        /// </summary>
        /// <param name="x">
        /// The x.
        /// </param>
        /// <param name="y">
        /// The y.
        /// </param>
        /// <returns>
        /// The System.Boolean.
        /// </returns>
        public bool Equals(MonthYear x, MonthYear y)
        {
            return x == y;
        }

        /// <summary>
        /// The get hash code.
        /// </summary>
        /// <param name="obj">
        /// The obj.
        /// </param>
        /// <returns>
        /// The System.Int32.
        /// </returns>
        public int GetHashCode(MonthYear obj)
        {
            return obj.Month.GetHashCode() + obj.Year.GetHashCode();
        }

        #endregion
    }
}