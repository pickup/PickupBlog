// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Stat.cs" company="LiteBlog">
//   Copyright (c) 2012, LiteBlog. All Rights Reserved.
// </copyright>
// <summary>
//   The stat.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace LiteBlog.Common
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    /// <summary>
    /// The stat.
    /// </summary>
    public class Stat
    {
        #region Fields

        /// <summary>
        /// The _feeds.
        /// </summary>
        private Dictionary<string, int> _feeds = new Dictionary<string, int>();

        /// <summary>
        /// The _hits.
        /// </summary>
        private int _hits;

        /// <summary>
        /// The _most popular.
        /// </summary>
        private List<Tuple<PostInfo, int>> _mostPopular = new List<Tuple<PostInfo, int>>();

        /// <summary>
        /// The _path.
        /// </summary>
        private string _path;

        /// <summary>
        /// The _visitors.
        /// </summary>
        private int _visitors = 0;

        /// <summary>
        /// The _visits.
        /// </summary>
        private int _visits;

        /// <summary>
        /// The _page visits.
        /// </summary>
        private Dictionary<string, PostInfo> pageVisits = new Dictionary<string, PostInfo>();

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the feed count.
        /// </summary>
        public int FeedCount
        {
            get
            {
                if (this._feeds.Count > 0)
                {
                    return this._feeds.Values.Max<int>();
                }

                return 0;
            }
        }

        /// <summary>
        /// Gets or sets the feeds.
        /// </summary>
        public Dictionary<string, int> Feeds
        {
            get
            {
                return this._feeds;
            }

            set
            {
                this._feeds = value;
            }
        }

        /// <summary>
        /// Gets or sets the hits.
        /// </summary>
        public int Hits
        {
            get
            {
                return this._hits;
            }

            set
            {
                this._hits = value;
            }
        }

        /// <summary>
        /// Gets or sets the page visits.
        /// </summary>
        public Dictionary<string, PostInfo> PageVisits
        {
            get
            {
                return this.pageVisits;
            }

            set
            {
                this.pageVisits = value;
            }
        }

        /// <summary>
        /// Gets or sets the path.
        /// </summary>
        public string Path
        {
            get
            {
                return this._path;
            }

            set
            {
                this._path = value;
            }
        }

        /// <summary>
        /// Gets or sets the visitors.
        /// </summary>
        public int Visitors
        {
            get
            {
                return this._visitors;
            }

            set
            {
                this._visitors = value;
            }
        }

        /// <summary>
        /// Gets or sets the visits.
        /// </summary>
        public int Visits
        {
            get
            {
                return this._visits;
            }

            set
            {
                this._visits = value;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The get feed key.
        /// </summary>
        /// <param name="date">
        /// The date.
        /// </param>
        /// <returns>
        /// The System.String.
        /// </returns>
        public static string GetFeedKey(DateTime date)
        {
            return "Feed" + date.ToString("ddMMyyyy", CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// The is feed key.
        /// </summary>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <returns>
        /// The System.Boolean.
        /// </returns>
        public static bool IsFeedKey(string key)
        {
            if (!key.StartsWith("Feed"))
            {
                return false;
            }

            DateTime dt = DateTime.MinValue;
            DateTime.TryParseExact(
                key.Substring(4), "ddMMyyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dt);
            if (dt == DateTime.MinValue)
            {
                return false;
            }

            return true;
        }

        #endregion
    }
}