// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Author.cs" company="LiteBlog">
//   Copyright (c) 2012, LiteBlog. All Rights Reserved.
// </copyright>
// <summary>
//   The author.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace LiteBlog.Common
{
    /// <summary>
    /// The author.
    /// </summary>
    public class Author
    {
        #region Fields

        /// <summary>
        /// The _email.
        /// </summary>
        private string _email = string.Empty;

        /// <summary>
        /// The _id.
        /// </summary>
        private string _id = string.Empty;

        /// <summary>
        /// The _locked.
        /// </summary>
        private bool _locked = false;

        /// <summary>
        /// The _name.
        /// </summary>
        private string _name = string.Empty;

        /// <summary>
        /// The _url.
        /// </summary>
        private string _url = string.Empty;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        public string Email
        {
            get
            {
                return this._email;
            }

            set
            {
                this._email = value;
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
        /// Gets or sets a value indicating whether locked.
        /// </summary>
        public bool Locked
        {
            get
            {
                return this._locked;
            }

            set
            {
                this._locked = value;
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

        /// <summary>
        /// Gets or sets the url.
        /// </summary>
        public string Url
        {
            get
            {
                return this._url;
            }

            set
            {
                this._url = value;
            }
        }

        #endregion
    }
}