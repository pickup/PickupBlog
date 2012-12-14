// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Document.cs" company="LiteBlog">
//   Copyright (c) 2012, LiteBlog. All Rights Reserved.
// </copyright>
// <summary>
//   The post type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace LiteBlog.Common
{
    using System;

    /// <summary>
    /// A special class with functions of how to store a file
    /// </summary>
    public class Document
    {
        #region Fields

        /// <summary>
        /// The _path.
        /// </summary>
        private string _path;

        #endregion

        #region Public Properties

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

        #endregion
    }
}