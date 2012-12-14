// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Comment.cs" company="LiteBlog">
//   Copyright (c) 2012, LiteBlog. All Rights Reserved.
// </copyright>
// <summary>
//   The comment.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace LiteBlog.Common
{
    using System;
    using System.Xml;

    /// <summary>
    /// The comment.
    /// </summary>
    public class Comment
    {
        #region Fields

        /// <summary>
        /// The _file id.
        /// </summary>
        private string _fileID = string.Empty;

        /// <summary>
        /// The _highlighted text.
        /// </summary>
        private string _highlightedText = string.Empty;

        /// <summary>
        /// The _id.
        /// </summary>
        private string _id = string.Empty;

        /// <summary>
        /// The _ip.
        /// </summary>
        private string _ip = string.Empty;

        /// <summary>
        /// The _name.
        /// </summary>
        private string _name = string.Empty;

        /// <summary>
        /// The _text.
        /// </summary>
        private string _text;

        /// <summary>
        /// The _time.
        /// </summary>
        private DateTime _time;

        /// <summary>
        /// The _url.
        /// </summary>
        private string _url = string.Empty;

        /// <summary>
        /// The is approved.
        /// </summary>
        private bool isApproved = true;

        /// <summary>
        /// The is author.
        /// </summary>
        private bool isAuthor = false;

        #endregion

        #region Public Properties

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
        /// Gets the highlighted code.
        /// </summary>
        public string HighlightedCode
        {
            get
            {
                return this._highlightedText;
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
        /// Gets or sets the ip.
        /// </summary>
        public string Ip
        {
            get
            {
                return this._ip;
            }

            set
            {
                this._ip = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether is approved.
        /// </summary>
        public bool IsApproved
        {
            get
            {
                return this.isApproved;
            }

            set
            {
                this.isApproved = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether is author.
        /// </summary>
        public bool IsAuthor
        {
            get
            {
                return this.isAuthor;
            }

            set
            {
                this.isAuthor = value;
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
        /// Gets or sets the text.
        /// </summary>
        public string Text
        {
            get
            {
                return this._text;
            }

            set
            {
                this._text = value;
                this._text = CodeCleaner.CleanCode(this._text);
                this._highlightedText = SyntaxHighlighter.Highlight(this._text);
            }
        }

        /// <summary>
        /// Gets the tile text.
        /// </summary>
        public string TileText
        {
            get
            {
                string text = this.Text;
                XmlDocument doc = new XmlDocument();
                try
                {
                    string xml = "<p>" + text.Replace("&nbsp;", " ") + "</p>";
                    doc.LoadXml(xml);
                    text = doc.InnerText;
                }
                catch (Exception)
                {
                }

                if (text.Length > 150)
                {
                    text = text.Substring(0, 150);
                    text += " ..";
                }

                return text;
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