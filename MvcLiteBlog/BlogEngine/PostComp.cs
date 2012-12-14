// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PostComp.cs" company="LiteBlog">
//   Copyright (c) 2012, LiteBlog. All Rights Reserved.
// </copyright>
// <summary>
//   The post comp.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace MvcLiteBlog.BlogEngine
{
    using System;
    using System.Globalization;

    using ColorCode;

    using LiteBlog.Common;
    using LiteBlog.Common.Contracts;

    using MvcLiteBlog.BlogEngine;
    using MvcLiteBlog.Helpers;

    /// <summary>
    /// The post comp.
    /// </summary>
    public class PostComp
    {
        #region Public Methods and Operators

        /// <summary>
        /// The change author.
        /// </summary>
        /// <param name="fileID">
        /// The file id.
        /// </param>
        /// <param name="author">
        /// The author.
        /// </param>
        public static void ChangeAuthor(string fileID, string author)
        {
            IPostData data = ConfigHelper.DataContext.PostData;
            Post post = data.Load(fileID);
            post.Author = author;
            data.Save(post);
        }

        /// <summary>
        /// The change category.
        /// </summary>
        /// <param name="fileID">
        /// The file id.
        /// </param>
        /// <param name="oldCatID">
        /// The old cat id.
        /// </param>
        /// <param name="newCatID">
        /// The new cat id.
        /// </param>
        public static void ChangeCategory(string fileID, string oldCatID, string newCatID)
        {
            IPostData data = ConfigHelper.DataContext.PostData;
            Post post = data.Load(fileID);
            string[] catIDs = post.CatID.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            string newCatIDs = string.Empty;
            foreach (string catID in catIDs)
            {
                if (catID == oldCatID)
                {
                    if (newCatID != string.Empty)
                    {
                        newCatIDs += newCatID + ",";
                    }
                }
                else
                {
                    newCatIDs += catID + ",";
                }
            }

            if (newCatIDs.Length > 0)
            {
                if (newCatIDs[newCatIDs.Length - 1] == ',')
                {
                    newCatIDs = newCatIDs.Substring(0, newCatIDs.Length - 1);
                }
            }

            post.CatID = newCatIDs;
            data.Save(post);
        }

        // private static string SyntaxHighlight2(string code, string language)
        // {
        // CodeHighlighter ch = new CodeHighlighter();
        // ch.LanguageKey = language;

        // ch.OutliningEnabled = false;
        // ch.LineNumberMarginVisible = false;
        // ch.Text = code;

        // StringBuilder sb = new StringBuilder();
        // StringWriter sw = new StringWriter(sb);

        // HtmlTextWriter htw = new HtmlTextWriter(sw);

        // Page thisPage = HttpContext.Current.Handler as Page;
        // thisPage.Controls.Add(ch);
        // ch.RenderControl(htw);
        // thisPage.Controls.Remove(ch);

        // return sb.ToString();

        // }

        /// <summary>
        /// The delete.
        /// </summary>
        /// <param name="fileID">
        /// The file id.
        /// </param>
        public static void Delete(string fileID)
        {
            IPostData data = ConfigHelper.DataContext.PostData;
            data.Delete(fileID);
        }

        /// <summary>
        /// The get categories.
        /// </summary>
        /// <param name="catID">
        /// The cat id.
        /// </param>
        /// <returns>
        /// The System.String.
        /// </returns>
        public static string GetCategories(string catID)
        {
            if (catID.Length == 0)
            {
                return "Uncategorized";
            }

            if (catID.IndexOf(",") < 0)
            {
                string cat = CategoryComp.GetCategoryName(catID);
                return string.Format("Category : {0}", cat);
            }
            else
            {
                string[] catIDs = catID.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                string cats = "Categories : ";
                foreach (string id in catIDs)
                {
                    cats += CategoryComp.GetCategoryName(id) + ", ";
                }

                cats = cats.Substring(0, cats.Length - 2);
                return cats;
            }
        }

        /// <summary>
        /// Call this method from the UI only
        /// Do not use it from Admin.
        /// After GetPost using this function, do not save!!
        /// </summary>
        /// <param name="fileID">
        /// The File ID
        /// </param>
        /// <param name="bRefresh">
        /// The b Refresh.
        /// </param>
        /// <param name="bHighlight">
        /// The b Highlight.
        /// </param>
        /// <returns>
        /// The LiteBlog.Common.Post.
        /// </returns>
        public static Post GetPost(string fileID, bool bRefresh = false, bool bHighlight = true)
        {
            Post post = CacheHelper.Get<Post>(fileID);
            if (post == null || bRefresh)
            {
                IPostData data = ConfigHelper.DataContext.PostData;

                post = data.Load(fileID);
                post.CategoriesText = GetCategories(post.CatID);
                string author = post.Author;
                if (string.IsNullOrEmpty(author))
                {
                    author = AuthorComp.GetDefaultAuthor().Name;
                }

                post.TimeText = GetTime(post.Time, author);

                if (bHighlight)
                {
                    // foreach (CodeSnippet snippet in post.GetCode())
                    // {
                    // string innerCode = snippet.InnerCode;
                    // // if(snippet.Language == "HTML")
                    // // Syntax Highlighter will encode any HTML text, So you can decode double
                    // // if you are using the highlighter
                    // innerCode = HttpContext.Current.Server.HtmlDecode(snippet.InnerCode);

                    // snippet.InnerCode = SyntaxHighlight(innerCode, snippet.Language);   
                    // }

                    // post.RestoreCode();

                    // foreach (Comment comment in post.Comments)
                    // {
                    // foreach (CodeSnippet snippet2 in comment.GetCode())
                    // {
                    // string innerCode = snippet2.InnerCode;
                    // // if (snippet2.Language == "HTML")
                    // // Syntax Highligher encodes
                    // innerCode = HttpContext.Current.Server.HtmlDecode(snippet2.InnerCode);

                    // snippet2.InnerCode = SyntaxHighlight(innerCode, snippet2.Language);
                    // }

                    // comment.RestoreCode();
                    // }
                }

                CacheHelper.PutPost(fileID, post);
            }

            // StatComp.IncrementPosts(fileID);
            return post;
        }

        /// <summary>
        /// The get time.
        /// </summary>
        /// <param name="time">
        /// The time.
        /// </param>
        /// <param name="author">
        /// The author.
        /// </param>
        /// <returns>
        /// The System.String.
        /// </returns>
        public static string GetTime(DateTime time, string author)
        {
            string postTime = time.ToString(ConfigHelper.DateFormat, CultureInfo.InvariantCulture);
            return string.Format("Posted by {0} on {1}", author, postTime);
        }

        /// <summary>
        /// The load.
        /// </summary>
        /// <param name="fileID">
        /// The file id.
        /// </param>
        /// <returns>
        /// The LiteBlog.Common.Post.
        /// </returns>
        public static Post Load(string fileID)
        {
            IPostData data = ConfigHelper.DataContext.PostData;
            Post post = data.Load(fileID);
            return post;
        }

        /// <summary>
        /// The save.
        /// </summary>
        /// <param name="post">
        /// The post.
        /// </param>
        public static void Save(Post post)
        {
            IPostData data = ConfigHelper.DataContext.PostData;
            data.Save(post);
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