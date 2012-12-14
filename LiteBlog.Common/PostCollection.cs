// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PostCollection.cs" company="LiteBlog">
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
    /// The post collection.
    /// </summary>
    public class PostCollection
    {
        #region Fields

        /// <summary>
        /// The _cat map.
        /// </summary>
        private Dictionary<string, List<PostInfo>> _catMap = new Dictionary<string, List<PostInfo>>();

        /// <summary>
        /// The _list.
        /// </summary>
        private List<PostInfo> _list = new List<PostInfo>();

        /// <summary>
        /// The _month map.
        /// </summary>
        private Dictionary<string, List<PostInfo>> _monthMap = new Dictionary<string, List<PostInfo>>();

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the category map.
        /// </summary>
        public Dictionary<string, List<PostInfo>> CategoryMap
        {
            get
            {
                return this._catMap;
            }
        }

        /// <summary>
        /// Gets the month map.
        /// </summary>
        public Dictionary<string, List<PostInfo>> MonthMap
        {
            get
            {
                return this._monthMap;
            }
        }

        /// <summary>
        /// Gets or sets the posts.
        /// </summary>
        public List<PostInfo> Posts
        {
            get
            {
                return this._list;
            }

            set
            {
                this._list = value;
            }
        }

        #endregion
    }
}