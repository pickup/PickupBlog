// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DraftPost.cs" company="LiteBlog">
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
    /// The draft post.
    /// </summary>
    public class DraftPost : Post
    {
        #region Fields

        /// <summary>
        /// This is used if editing an existing post
        /// </summary>
        private string _draftID = string.Empty;

        /// <summary>
        /// The _old cat id.
        /// </summary>
        private string _oldCatID = string.Empty;

        /// <summary>
        /// The _type.
        /// </summary>
        private PostType _type = PostType.New;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DraftPost"/> class.
        /// </summary>
        public DraftPost()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DraftPost"/> class.
        /// </summary>
        /// <param name="post">
        /// The post.
        /// </param>
        public DraftPost(Post post)
        {
            this._fileID = post.FileID;
            this._type = PostType.Draft;

            this._catID = post.CatID;
            this._oldCatID = this._catID;

            this._title = post.Title;
            this._author = post.Author;
            this._time = post.Time;

            this._comments = post.Comments;
            this._docs = post.Documents;
            this._contents = post.Contents;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the draft id.
        /// </summary>
        public string DraftID
        {
            get
            {
                return this._draftID;
            }

            set
            {
                this._draftID = value;
            }
        }

        // required at the time of publishing

        /// <summary>
        /// Gets or sets the old cat id.
        /// </summary>
        public string OldCatID
        {
            get
            {
                return this._oldCatID;
            }

            set
            {
                this._oldCatID = value;
            }
        }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        public PostType Type
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

        #region Public Methods and Operators

        /// <summary>
        /// The get new draft id.
        /// </summary>
        /// <param name="fileID">
        /// The file id.
        /// </param>
        /// <param name="now">
        /// The now.
        /// </param>
        /// <returns>
        /// The System.String.
        /// </returns>
        public static string GetNewDraftID(string fileID, DateTime now)
        {
            return string.Format("{0}-Draft-{1}", fileID, now.ToString("MMddyyhhmmss"));
        }

        /// <summary>
        /// The is draft.
        /// </summary>
        /// <param name="fileID">
        /// The file id.
        /// </param>
        /// <returns>
        /// The System.Boolean.
        /// </returns>
        public static bool IsDraft(string fileID)
        {
            return fileID.Contains("-Draft-");
        }

        #endregion
    }
}