// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MonthYear.cs" company="LiteBlog">
//   Copyright (c) 2012, LiteBlog. All Rights Reserved.
// </copyright>
// <summary>
//   The month year.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace MvcLiteBlog.Models
{
    /// <summary>
    /// The month year.
    /// </summary>
    public class MonthYear
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the Month
        /// </summary>
        public string Month { get; set; }

        /// <summary>
        /// Gets or sets the year
        /// </summary>
        public int Year { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Equals operator
        /// </summary>
        /// <param name="my1">The my1</param>
        /// <param name="my2">The my2</param>
        /// <returns>whether equal</returns>
        public static bool operator ==(MonthYear my1, MonthYear my2)
        {
            return (my1.Month == my2.Month) && (my1.Year == my2.Year);
        }

        /// <summary>
        /// The !=.
        /// </summary>
        /// <param name="my1">
        /// The my 1.
        /// </param>
        /// <param name="my2">
        /// The my 2.
        /// </param>
        /// <returns>
        /// whether not equal
        /// </returns>
        public static bool operator !=(MonthYear my1, MonthYear my2)
        {
            return !(my1 == my2);
        }

        /// <summary>
        /// The equals.
        /// </summary>
        /// <param name="obj">
        /// The obj.
        /// </param>
        /// <returns>
        /// The System.Boolean.
        /// </returns>
        public override bool Equals(object obj)
        {
            return this == (MonthYear)obj;
        }

        /// <summary>
        /// The get hash code.
        /// </summary>
        /// <returns>
        /// The System.Int32.
        /// </returns>
        public override int GetHashCode()
        {
            return this.Month.GetHashCode() + this.Year.GetHashCode();
        }

        #endregion
    }
}