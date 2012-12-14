// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SyntaxHighlighter.cs" company="LiteBlog">
//   Copyright (c) 2012, LiteBlog. All Rights Reserved.
// </copyright>
// <summary>
//   CodeSnippet stores the code as <pre>Encoded string</pre>
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace LiteBlog.Common
{
    using System.Web;
    using ColorCode;

    /// <summary>
    /// The syntax highlighter.
    /// </summary>
    public class SyntaxHighlighter
    {
        #region Public Methods and Operators

        /// <summary>
        /// The highlight.
        /// </summary>
        /// <param name="code">
        /// The code.
        /// </param>
        /// <returns>
        /// The System.String.
        /// </returns>
        public static string Highlight(string code)
        {
            CodeBlock codeBlock = new CodeBlock(code);
            codeBlock.GetSnippets();
            foreach (CodeSnippet snippet in codeBlock.Snippets)
            {
                string innerCode = snippet.InnerCode;

                // Syntax highlighter encodes
                // if (snippet.Language == "HTML")
                innerCode = HttpContext.Current.Server.HtmlDecode(innerCode);
                snippet.InnerCode = SyntaxHighlight(innerCode, snippet.Language);
            }

            codeBlock.ReplaceSnippets();
            return codeBlock.HighlightedText;
        }

        #endregion

        #region Methods

        /// <summary>
        /// The syntax highlight.
        /// </summary>
        /// <param name="code">
        /// The code.
        /// </param>
        /// <param name="language">
        /// The language.
        /// </param>
        /// <returns>
        /// The System.String.
        /// </returns>
        private static string SyntaxHighlight(string code, string language)
        {
            CodeColorizer colorizer = new CodeColorizer();
            ILanguage lang = Languages.CSharp;
            switch (language)
            {
                case "C#":
                    lang = Languages.CSharp;
                    break;
                case "HTML":
                    lang = Languages.Html;
                    break;
                case "VB.NET":
                    lang = Languages.VbDotNet;
                    break;
                case "XML":
                    lang = Languages.Xml;
                    break;
                case "SQL":
                    lang = Languages.Sql;
                    break;
                case "JScript":
                    lang = Languages.JavaScript;
                    break;
            }

            return colorizer.Colorize(code, lang);
        }

        #endregion
    }
}