// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Month.cs" company="LiteBlog">
//   Copyright (c) 2012, LiteBlog. All Rights Reserved.
// </copyright>
// <summary>
//   The month.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace LiteBlog.Common
{
    using System;

    /// <summary>
    /// The month.
    /// </summary>
    public class Month
    {
        #region Fields

        /// <summary>
        /// The _count.
        /// </summary>
        private int _count = 0;

        /// <summary>
        /// The _month id.
        /// </summary>
        private string _monthID = string.Empty;

        /// <summary>
        /// The _name.
        /// </summary>
        private string _name = string.Empty;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Month"/> class.
        /// </summary>
        public Month()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Month"/> class.
        /// </summary>
        /// <param name="dt">
        /// The dt.
        /// </param>
        public Month(DateTime dt)
        {
            this._monthID = GetMonthID(dt);
            this._name = GetMonthName(dt);
            this._count = 0;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the count.
        /// </summary>
        public int Count
        {
            get
            {
                return this._count;
            }

            set
            {
                this._count = value;
            }
        }

        /// <summary>
        /// Gets or sets the month id.
        /// </summary>
        public string MonthID
        {
            get
            {
                return this._monthID;
            }

            set
            {
                this._monthID = value;
            }
        }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name
        {
            get
            {
                return this._name;
            }

            set
            {
                this._name = value;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The get month id.
        /// </summary>
        /// <param name="date">
        /// The date.
        /// </param>
        /// <returns>
        /// The System.String.
        /// </returns>
        public static string GetMonthID(DateTime date)
        {
            return date.ToString("MMMyyyy");
        }

        /// <summary>
        /// The get month name.
        /// </summary>
        /// <param name="date">
        /// The date.
        /// </param>
        /// <returns>
        /// The System.String.
        /// </returns>
        public static string GetMonthName(DateTime date)
        {
            return date.ToString("MMM yyyy");
        }

        /// <summary>
        /// The get date.
        /// </summary>
        /// <returns>
        /// The System.DateTime.
        /// </returns>
        public DateTime GetDate()
        {
            return DateTime.ParseExact("1" + this._name, "dMMM yyyy", System.Globalization.CultureInfo.InvariantCulture);
        }

        #endregion
    }
}