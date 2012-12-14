// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PostInfo.cs" company="LiteBlog">
//   Copyright (c) 2012, LiteBlog. All Rights Reserved.
// </copyright>
// <summary>
//   The post info.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace LiteBlog.Common
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// The post info.
    /// </summary>
    public class PostInfo
    {
        #region Fields

        /// <summary>
        /// The _author.
        /// </summary>
        private string _author = string.Empty;

        /// <summary>
        /// The _cat id.
        /// </summary>
        private string _catID;

        /// <summary>
        /// The _file id.
        /// </summary>
        private string _fileID;

        /// <summary>
        /// The _month id.
        /// </summary>
        private string _monthID;

        /// <summary>
        /// The _time.
        /// </summary>
        private DateTime _time;

        /// <summary>
        /// The _title.
        /// </summary>
        private string _title;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the author.
        /// </summary>
        public string Author
        {
            get
            {
                return this._author;
            }

            set
            {
                this._author = value;
            }
        }

        /// <summary>
        /// Gets or sets the cat id.
        /// </summary>
        public string CatID
        {
            get
            {
                return this._catID;
            }

            set
            {
                this._catID = value;
            }
        }

        /// <summary>
        /// Gets the categories.
        /// </summary>
        public List<string> Categories
        {
            get
            {
                List<string> categories = new List<string>();
                if (this._catID.IndexOf(',') > 0)
                {
                    string[] catIDs = this._catID.Split(new[] { ',' });
                    foreach (string catID in catIDs)
                    {
                        categories.Add(catID);
                    }
                }
                else
                {
                    categories.Add(this._catID);
                }

                return categories;
            }
        }

        /// <summary>
        /// Gets or sets the file id.
        /// </summary>
        public string FileID
        {
            get
            {
                return this._fileID;
            }

            set
            {
                this._fileID = value;
            }
        }

        /// <summary>
        /// Gets the Month
        /// </summary>
        public string Month
        {
            get
            {
                return this._time.ToString("MMMM");
            }
        }

        /// <summary>
        /// Gets or sets the month id.
        /// </summary>
        public string MonthID
        {
            get
            {
                return GetMonthID(this._time);
            }

            set
            {
                this._monthID = value;
            }
        }

        /// <summary>
        /// Gets or sets the time.
        /// </summary>
        public DateTime Time
        {
            get
            {
                return this._time;
            }

            set
            {
                this._time = value;
            }
        }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        public string Title
        {
            get
            {
                return this._title;
            }

            set
            {
                this._title = value;
            }
        }

        /// <summary>
        /// Gets or sets the views.
        /// </summary>
        public int Views { get; set; }

        /// <summary>
        /// Gets the Year
        /// </summary>
        public int Year
        {
            get
            {
                return this._time.Year;
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

        #endregion
    }
}