// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CodeCleaner.cs" company="LiteBlog">
//   Copyright (c) 2012, LiteBlog. All Rights Reserved.
// </copyright>
// <summary>
//   CodeSnippet stores the code as <pre>Encoded string</pre>
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace LiteBlog.Common
{
    /// <summary>
    /// The code cleaner.
    /// </summary>
    public class CodeCleaner
    {
        #region Public Methods and Operators

        /// <summary>
        /// The clean code.
        /// </summary>
        /// <param name="code">
        /// The code.
        /// </param>
        /// <returns>
        /// The System.String.
        /// </returns>
        public static string CleanCode(string code)
        {
            CodeBlock codeBlock = new CodeBlock(code);
            codeBlock.GetSnippets();
            codeBlock.ReplaceSnippets();
            return codeBlock.HighlightedText;
        }

        #endregion
    }
}