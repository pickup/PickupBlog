// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Post.cs" company="LiteBlog">
//   Copyright (c) 2012, LiteBlog. All Rights Reserved.
// </copyright>
// <summary>
//   The post.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace LiteBlog.Common
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// The post.
    /// </summary>
    public class Post
    {
        #region Fields

        /// <summary>
        /// The _author.
        /// </summary>
        protected string _author = string.Empty;

        /// <summary>
        /// The _cat id.
        /// </summary>
        protected string _catID;

        /// <summary>
        /// The _comments.
        /// </summary>
        protected List<Comment> _comments = new List<Comment>();

        /// <summary>
        /// The _contents.
        /// </summary>
        protected string _contents = string.Empty;

        /// <summary>
        /// The _docs.
        /// </summary>
        protected List<Document> _docs = new List<Document>();

        /// <summary>
        /// The _file id.
        /// </summary>
        protected string _fileID = string.Empty;

        /// <summary>
        /// The _time.
        /// </summary>
        protected DateTime _time;

        /// <summary>
        /// The _title.
        /// </summary>
        protected string _title;

        /// <summary>
        /// The _highlighted contents.
        /// </summary>
        private string _highlightedContents = string.Empty;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Post"/> class.
        /// </summary>
        public Post()
        {
        }

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
        /// Gets or sets the categories text.
        /// </summary>
        public string CategoriesText { get; set; }

        /// <summary>
        /// Gets or sets the comments.
        /// </summary>
        public List<Comment> Comments
        {
            get
            {
                return this._comments;
            }

            set
            {
                this._comments = value;
            }
        }

        /// <summary>
        /// Gets or sets the contents.
        /// </summary>
        public string Contents
        {
            get
            {
                return this._contents;
            }

            set
            {
                this._contents = value;
                this._contents = CodeCleaner.CleanCode(this._contents);
                this._highlightedContents = SyntaxHighlighter.Highlight(this._contents);
            }
        }

        /// <summary>
        /// Gets or sets the documents.
        /// </summary>
        public List<Document> Documents
        {
            get
            {
                return this._docs;
            }

            set
            {
                this._docs = value;
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
        /// Gets the highlighted contents.
        /// </summary>
        public string HighlightedContents
        {
            get
            {
                return this._highlightedContents;
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
        /// Gets or sets the time text.
        /// </summary>
        public string TimeText { get; set; }

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

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The get comment count.
        /// </summary>
        /// <returns>
        /// The System.String.
        /// </returns>
        public string GetCommentCount()
        {
            if (this._comments.Count == 0)
            {
                return "没有评论";
            }

            if (this._comments.Count > 0)
            {
                return string.Format("{0}条评论", this._comments.Count);
            }

            return "没有评论";
        }

        #endregion
    }
}