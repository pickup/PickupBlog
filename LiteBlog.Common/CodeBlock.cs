// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CodeBlock.cs" company="LiteBlog">
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
    /// The code block.
    /// </summary>
    public class CodeBlock
    {
        #region Fields

        /// <summary>
        /// The _b encode.
        /// </summary>
        private bool bEncode = false;

        /// <summary>
        /// The _h text.
        /// </summary>
        private string hText;

        /// <summary>
        /// The _raw text.
        /// </summary>
        private string _rawText;

        /// <summary>
        /// The _snippets.
        /// </summary>
        private List<CodeSnippet> _snippets;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CodeBlock"/> class. 
        /// Default constructor highlights the code, 
        /// For doing other processing, 
        /// use GetSnippets to get CodeSnippets
        /// Replace CodeSnippets with somethng
        /// use ReplaceSnippets to replace _hText
        /// Use HighlightedText to get processed CodeBlock
        /// </summary>
        /// <param name="code">
        /// raw text with code
        /// </param>
        public CodeBlock(string code)
        {
            this._rawText = code;
            this.hText = code;

            // CssClass = cssClass;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets a value indicating whether encode.
        /// </summary>
        public bool Encode
        {
            get
            {
                return this.bEncode;
            }
        }

        /// <summary>
        /// Gets the highlighted text.
        /// </summary>
        public string HighlightedText
        {
            get
            {
                return this.hText;
            }
        }

        /// <summary>
        /// Gets the raw text.
        /// </summary>
        public string RawText
        {
            get
            {
                return this._rawText;
            }
        }

        /// <summary>
        /// Gets the snippets.
        /// </summary>
        public List<CodeSnippet> Snippets
        {
            get
            {
                return this._snippets;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Call this method to get snippets for processing
        /// Call 
        /// </summary>
        public void GetSnippets()
        {
            this._snippets = new List<CodeSnippet>();
            this.hText = this._rawText;
            Dictionary<int, string> replaceContents = new Dictionary<int, string>();

            // Get all the code snippets
            string startToken = "<pre";
            string endToken = "</pre>";
            int curPos = 0;
            int foundIndex = 0;
            while ((curPos = this.hText.IndexOf(startToken, curPos)) != -1)
            {
                int endPos = this.hText.IndexOf(endToken, curPos);
                if (endPos != -1)
                {
                    foundIndex++;
                    string code = this.hText.Substring(curPos, endPos - curPos + endToken.Length);

                    CodeSnippet snippet = new CodeSnippet();

                    // snippet.CssClass = CssClass;
                    snippet.CodeID = foundIndex;
                    replaceContents.Add(foundIndex, code);

                    string innerCode = code.Substring(
                        code.IndexOf(">") + 1, code.Length - (code.IndexOf(">") + 1 + endToken.Length));

                    // if (innerCode.Contains("<"))
                    // code = code.Replace(innerCode, HttpContext.Current.Server.HtmlEncode(innerCode));
                    XmlDocument pre = new XmlDocument();
                    try
                    {
                        code = code.Replace("&nbsp;", " ");
                        pre.LoadXml(code);
                    }
                    catch
                    {
                        // is this encoding necessary? -- for older documents
                        // code = code.Replace(innerCode, HttpContext.Current.Server.HtmlEncode(innerCode));
                        // pre.LoadXml(code);
                        this.bEncode = true;
                    }

                    snippet.Code = code;

                    XmlAttribute cssClass = pre.DocumentElement.Attributes["class"];
                    if (cssClass == null)
                    {
                        snippet.CssClass = "code";
                    }
                    else
                    {
                        snippet.CssClass = cssClass.Value;
                    }

                    XmlAttribute language = pre.DocumentElement.Attributes["language"];
                    if (language == null)
                    {
                        snippet.Language = "C#";
                    }
                    else
                    {
                        snippet.Language = language.Value;
                    }

                    this._snippets.Add(snippet);
                    curPos = endPos + endToken.Length;
                }
                else
                {
                    break;
                }
            }

            foreach (CodeSnippet snippet in this._snippets)
            {
                this.hText = this.hText.Replace(replaceContents[snippet.CodeID], snippet.Placeholder);
            }
        }

        /// <summary>
        /// The replace snippets.
        /// </summary>
        public void ReplaceSnippets()
        {
            foreach (CodeSnippet snippet in this._snippets)
            {
                this.hText = this.hText.Replace(snippet.Placeholder, snippet.Code);
            }

            this._snippets.Clear();
        }

        #endregion
    }
}