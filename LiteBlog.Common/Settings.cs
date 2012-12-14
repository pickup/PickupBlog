// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Settings.cs" company="LiteBlog">
//   Copyright (c) 2012, LiteBlog. All Rights Reserved.
// </copyright>
// <summary>
//   The settings.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace LiteBlog.Common
{
    /// <summary>
    /// The settings.
    /// </summary>
    public class Settings
    {
        #region Fields

        /// <summary>
        /// The _moderate.
        /// </summary>
        private bool _moderate = false;

        /// <summary>
        /// The _name.
        /// </summary>
        private string _name;

        /// <summary>
        /// The _post count.
        /// </summary>
        private int _postCount = 5;

        /// <summary>
        /// The _timezone.
        /// </summary>
        private string _timezone = "India Standard Time";

        /// <summary>
        /// The _url.
        /// </summary>
        private string _url;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets a value indicating whether comment moderation.
        /// </summary>
        public bool CommentModeration
        {
            get
            {
                return this._moderate;
            }

            set
            {
                this._moderate = value;
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
        /// Gets or sets the post count.
        /// </summary>
        public int PostCount
        {
            get
            {
                return this._postCount;
            }

            set
            {
                this._postCount = value;
            }
        }

        /// <summary>
        /// Gets or sets the timezone.
        /// </summary>
        public string Timezone
        {
            get
            {
                return this._timezone;
            }

            set
            {
                this._timezone = value;
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