// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CommentComp.cs" company="LiteBlog">
//   Copyright (c) 2012, LiteBlog. All Rights Reserved.
// </copyright>
// <summary>
//   The comment comp.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace MvcLiteBlog.BlogEngine
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;

    using ColorCode;

    using LiteBlog.Common;

    using MvcLiteBlog.Helpers;

    /// <summary>
    /// The comment comp.
    /// </summary>
    public class CommentComp
    {
        #region Public Methods and Operators

        /// <summary>
        /// The approve.
        /// </summary>
        /// <param name="commentID">
        /// The comment id.
        /// </param>
        public static void Approve(string commentID)
        {
            ConfigHelper.DataContext.CommentData.Approve(commentID);

            List<Comment> comments = GetComments();
            Comment comment = (from cmnt in comments where cmnt.ID == commentID select cmnt).FirstOrDefault<Comment>();

            if (!string.IsNullOrEmpty(comment.FileID))
            {
                ConfigHelper.DataContext.PostData.InsertComment(comment);
            }
        }

        /// <summary>
        /// The delete.
        /// </summary>
        /// <param name="commentID">
        /// The comment id.
        /// </param>
        public static void Delete(string commentID)
        {
            List<Comment> comments = GetComments();
            var qry = from comment in comments where comment.ID == commentID select comment;

            if (qry.Count<Comment>() != 1)
            {
                Logger.Log("The comment could not be located");

                // throw new ApplicationException("Comment could not be located");
            }

            Comment comment2 = qry.First<Comment>();

            ConfigHelper.DataContext.CommentData.Delete(comment2.ID);

            if (comment2.IsApproved)
            {
                ConfigHelper.DataContext.PostData.DeleteComment(comment2);
            }
        }

        /// <summary>
        /// The get comment.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The LiteBlog.Common.Comment.
        /// </returns>
        public static Comment GetComment(string id)
        {
            List<Comment> comments = ConfigHelper.DataContext.CommentData.GetComments();
            var qry = from elem in comments where elem.ID == id select elem;

            return qry.First<Comment>();
        }

        /// <summary>
        /// The get comments.
        /// </summary>
        /// <param name="bRefresh">
        /// The b refresh.
        /// </param>
        /// <returns>
        /// The System.Collections.Generic.List`1[T -&gt; LiteBlog.Common.Comment].
        /// </returns>
        public static List<Comment> GetComments(bool bRefresh = false)
        {
            List<Comment> comments = CacheHelper.Get<List<Comment>>(CacheType.Comments);
            if (comments == null || bRefresh)
            {
                comments = ConfigHelper.DataContext.CommentData.GetComments();

                // foreach (Comment comment in comments)
                // {
                // foreach (CodeSnippet snippet in comment.GetCode())
                // {
                // string innerCode = snippet.InnerCode;

                // // Syntax highlighter encodes
                // // if (snippet.Language == "HTML")
                // innerCode = HttpContext.Current.Server.HtmlDecode(innerCode);

                // snippet.InnerCode = SyntaxHighlight(innerCode, snippet.Language);
                // }

                // comment.RestoreCode();
                // }
                CacheHelper.Put(CacheType.Comments, comments);
            }

            return comments;
        }

        /// <summary>
        /// The get unapproved comments.
        /// </summary>
        /// <param name="bRefresh">
        /// The b refresh.
        /// </param>
        /// <returns>
        /// The System.Collections.Generic.List`1[T -&gt; LiteBlog.Common.Comment].
        /// </returns>
        public static List<Comment> GetUnapprovedComments(bool bRefresh = false)
        {
            List<Comment> comments = GetComments(bRefresh);
            var comments2 = from comment in comments where comment.IsApproved == false select comment;

            return comments2.ToList<Comment>();
        }

        /// <summary>
        /// The insert.
        /// </summary>
        /// <param name="fileID">
        /// The file id.
        /// </param>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <param name="url">
        /// The url.
        /// </param>
        /// <param name="text">
        /// The text.
        /// </param>
        /// <param name="isAuthor">
        /// The is author.
        /// </param>
        public static void Insert(string fileID, string name, string url, string text, bool isAuthor = false)
        {
            bool commentModeration = SettingsComp.GetSettings().CommentModeration;

            Comment comment = new Comment();
            comment.FileID = fileID;
            comment.Name = name;
            comment.Url = url;
            comment.Text = text;
            TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById(SettingsComp.GetSettings().Timezone);
            comment.Time = LocalTime.GetCurrentTime(tzi);
            comment.Ip = HttpContext.Current.Request.UserHostAddress;
            comment.IsAuthor = isAuthor;

            if (commentModeration)
            {
                comment.IsApproved = false;
            }
            else
            {
                comment.IsApproved = true;
            }

            comment.ID = ConfigHelper.DataContext.CommentData.Insert(comment);

            if (!commentModeration)
            {
                ConfigHelper.DataContext.PostData.InsertComment(comment);
            }
        }

        /// <summary>
        /// The refresh.
        /// </summary>
        /// <returns>
        /// The System.Collections.Generic.List`1[T -&gt; LiteBlog.Common.Comment].
        /// </returns>
        public static List<Comment> Refresh()
        {
            ConfigHelper.DataContext.CommentData.Refresh();
            List<Comment> comments = ConfigHelper.DataContext.CommentData.GetComments();
            CacheHelper.Put(CacheType.Comments, comments);
            return comments;
        }

        /// <summary>
        /// The update.
        /// </summary>
        /// <param name="comment">
        /// The comment.
        /// </param>
        public static void Update(Comment comment)
        {
            ConfigHelper.DataContext.CommentData.Update(comment);
            if (comment.IsApproved)
            {
                ConfigHelper.DataContext.PostData.UpdateComment(comment);
            }
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