// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ArchiveMonth.cs" company="LiteBlog">
//   Copyright (c) 2012, LiteBlog. All Rights Reserved.
// </copyright>
// <summary>
//   The archive month.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace LiteBlog.Common
{
    using System;

    /// <summary>
    /// The archive month.
    /// </summary>
    public class ArchiveMonth
    {
        #region Constants

        /// <summary>
        /// The _archive format.
        /// </summary>
        private const string ArchiveFormat = "yyyy年MM月";
        //private const string ArchiveFormat = "MMM yyyy";

        /// <summary>
        /// The _start year.
        /// </summary>
        private const int StartYear = 1990;

        #endregion

        #region Fields

        /// <summary>
        /// The _count.
        /// </summary>
        private int _count = 0;

        /// <summary>
        /// The _id.
        /// </summary>
        private int _id = 0;

        /// <summary>
        /// The _month.
        /// </summary>
        private int _month = 0;

        /// <summary>
        /// The _year.
        /// </summary>
        private int _year = 0;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ArchiveMonth"/> class.
        /// </summary>
        public ArchiveMonth()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ArchiveMonth"/> class.
        /// </summary>
        /// <param name="month">
        /// The month.
        /// </param>
        /// <param name="year">
        /// The year.
        /// </param>
        public ArchiveMonth(int month, int year)
        {
            this._month = month;
            this._year = year;
            this._id = GetArchiveID(month, year);
            this._count = 1;
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
        /// Gets or sets the id.
        /// </summary>
        public int ID
        {
            get
            {
                return GetArchiveID(this._month, this._year);
            }

            set
            {
                this._id = value;
            }
        }

        /// <summary>
        /// Gets or sets the month.
        /// </summary>
        public int Month
        {
            get
            {
                return this._month;
            }

            set
            {
                this._month = value;
            }
        }

        /// <summary>
        /// Gets the name.
        /// </summary>
        public string Name
        {
            get
            {
                DateTime dt = new DateTime(this._year, this._month, 1);
                return dt.ToString(ArchiveFormat, System.Globalization.CultureInfo.InvariantCulture);
            }
        }

        /// <summary>
        /// Gets the url.
        /// </summary>
        public string Url
        {
            get
            {
                return string.Format("~/{0}/{1:00}", this._year, this._month);
            }
        }

        /// <summary>
        /// Gets or sets the year.
        /// </summary>
        public int Year
        {
            get
            {
                return this._year;
            }

            set
            {
                this._year = value;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The get archive id.
        /// </summary>
        /// <param name="month">
        /// The month.
        /// </param>
        /// <param name="year">
        /// The year.
        /// </param>
        /// <returns>
        /// The System.Int32.
        /// </returns>
        public static int GetArchiveID(int month, int year)
        {
            if (year < StartYear)
            {
                return 0;
            }

            return month + ((year - StartYear) * 12);
        }

        #endregion

        // do not change this after deployment
        // Used to derive _archiveID
    }
}