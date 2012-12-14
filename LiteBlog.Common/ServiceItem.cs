// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ServiceItem.cs" company="LiteBlog">
//   Copyright (c) 2012, LiteBlog. All Rights Reserved.
// </copyright>
// <summary>
//   The service item.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace LiteBlog.Common
{
    using System;

    /// <summary>
    /// The service item.
    /// </summary>
    public class ServiceItem
    {
        #region Fields

        /// <summary>
        /// The _freq.
        /// </summary>
        private int _freq = 0;

        /// <summary>
        /// The _id.
        /// </summary>
        private string _id;

        /// <summary>
        /// The _last updated.
        /// </summary>
        private DateTime _lastUpdated;

        /// <summary>
        /// The _method.
        /// </summary>
        private string _method;

        /// <summary>
        /// The _path.
        /// </summary>
        private string _path;

        /// <summary>
        /// The _type.
        /// </summary>
        private string _type;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the frequency.
        /// </summary>
        public int Frequency
        {
            get
            {
                return this._freq;
            }

            set
            {
                this._freq = value;
            }
        }

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public string ID
        {
            get
            {
                return this._id;
            }

            set
            {
                this._id = value;
            }
        }

        /// <summary>
        /// Gets or sets the last updated.
        /// </summary>
        public DateTime LastUpdated
        {
            get
            {
                return this._lastUpdated;
            }

            set
            {
                this._lastUpdated = value;
            }
        }

        /// <summary>
        /// Gets or sets the method.
        /// </summary>
        public string Method
        {
            get
            {
                return this._method;
            }

            set
            {
                this._method = value;
            }
        }

        /// <summary>
        /// Gets the next update.
        /// </summary>
        public DateTime NextUpdate
        {
            get
            {
                return this._lastUpdated.AddMinutes(this._freq);
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
        /// Gets or sets the type.
        /// </summary>
        public string Type
        {
            get
            {
                return this._type;
            }

            set
            {
                this._type = value;
            }
        }

        #endregion
    }
}