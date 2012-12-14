// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CodeSnippet.cs" company="LiteBlog">
//   Copyright (c) 2012, LiteBlog. All Rights Reserved.
// </copyright>
// <summary>
//   CodeSnippet stores the code as <pre>Encoded string</pre>
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace LiteBlog.Common
{
    using System;
    using System.Collections.Generic;
    using System.Web;
    using System.Xml;

    using ColorCode;

    /// <summary>
    /// CodeSnippet stores the code as <pre>Encoded string</pre>
    /// </summary>
    public class CodeSnippet
    {
        #region Fields

        /// <summary>
        /// The code.
        /// </summary>
        private string code;

        /// <summary>
        /// The code id.
        /// </summary>
        private int codeID;

        /// <summary>
        /// The css class.
        /// </summary>
        private string cssClass = "code";

        /// <summary>
        /// The language.
        /// </summary>
        private string language;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the code.
        /// </summary>
        public string Code
        {
            get
            {
                return this.code;
            }

            set
            {
                this.code = value;
            }
        }

        /// <summary>
        /// Gets or sets the code id.
        /// </summary>
        public int CodeID
        {
            get
            {
                return this.codeID;
            }

            set
            {
                this.codeID = value;
            }
        }

        /// <summary>
        /// Gets or sets the css class.
        /// </summary>
        public string CssClass
        {
            get
            {
                return this.cssClass;
            }

            set
            {
                this.cssClass = value;
            }
        }

        /// <summary>
        /// Gets or sets the inner code.
        /// </summary>
        public string InnerCode
        {
            get
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(this.code);
                return doc.DocumentElement.InnerXml;
            }

            set
            {
                string innerCode = @"<pre class=""{0}"" language=""{1}"">{2}</pre>";
                this.code = string.Format(innerCode, this.cssClass, this.language, value);
            }
        }

        /// <summary>
        /// Gets or sets the language.
        /// </summary>
        public string Language
        {
            get
            {
                return this.language;
            }

            set
            {
                this.language = value;
            }
        }

        /// <summary>
        /// Gets the placeholder.
        /// </summary>
        public string Placeholder
        {
            get
            {
                return string.Format("<div>{0}</div>", this.codeID);
            }
        }

        #endregion
    }
}