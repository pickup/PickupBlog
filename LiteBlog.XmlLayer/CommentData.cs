// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CommentData.cs" company="LiteBlog">
//   Copyright (c) 2012, LiteBlog. All Rights Reserved.
// </copyright>
// <summary>
//   Class that stores Comment XML
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace LiteBlog.XmlLayer
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using System.Web;
    using System.Xml.Linq;

    using LiteBlog.Common;
    using LiteBlog.Common.Contracts;

    /// <summary>
    /// Class that stores Comment XML
    /// </summary>
    public class CommentData : ICommentData
    {
        #region Constants

        /// <summary>
        /// The dat e_ forma t_ error.
        /// </summary>
        private const string DATE_FORMAT_ERROR = "Time field for {0} is not in the right format";

        /// <summary>
        /// The n o_ commen t_ error.
        /// </summary>
        private const string NO_COMMENT_ERROR = "Comment = {0} could not be found";

        /// <summary>
        /// The n o_ fil e_ error.
        /// </summary>
        private const string NO_FILE_ERROR = "Comment file could not be found";

        /// <summary>
        /// The xm l_ forma t_ error.
        /// </summary>
        private const string XML_FORMAT_ERROR = "Comment file is not in the right format";

        #endregion

        #region Fields

        /// <summary>
        /// The _path.
        /// </summary>
        private string _path;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CommentData"/> class.
        /// </summary>
        public CommentData()
        {
            this._path = DataContext.Path + "Comment.xml";
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the path.
        /// </summary>
        internal static string Path
        {
            get
            {
                return DataContext.Path + "Comment.xml";
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Approve comment
        /// Called from comment management
        /// </summary>
        /// <param name="commentID">
        /// Comment ID
        /// </param>
        public void Approve(string commentID)
        {
            XElement root = null;

            try
            {
                root = XElement.Load(this._path);
            }
            catch (Exception ex)
            {
                Logger.Log(NO_FILE_ERROR, ex);
                throw new ApplicationException(NO_FILE_ERROR, ex);
            }

            var qry = from elem in root.Elements("Comment") where elem.Attribute("ID").Value == commentID select elem;

            XElement commentElem = qry.First<XElement>();

            if (commentElem == null)
            {
                string msg = string.Format(NO_COMMENT_ERROR, commentID);
                Logger.Log(msg);
                throw new ApplicationException(msg);
            }

            commentElem.SetAttributeValue("Approved", true);

            XmlHelper.Save(root, this._path);
        }

        /// <summary>
        /// Delete comment from XML
        /// </summary>
        /// <param name="commentID">
        /// Comment ID
        /// </param>
        public void Delete(string commentID)
        {
            XElement root = null;

            try
            {
                root = XElement.Load(this._path);
            }
            catch (Exception ex)
            {
                Logger.Log(NO_FILE_ERROR, ex);
                throw new ApplicationException(NO_FILE_ERROR, ex);
            }

            var qry = from elem in root.Elements("Comment") where elem.Attribute("ID").Value == commentID select elem;

            XElement commentElem = qry.First<XElement>();

            if (commentElem == null)
            {
                string msg = string.Format(NO_COMMENT_ERROR, commentID);
                Logger.Log(msg);
                throw new ApplicationException(msg);
            }

            commentElem.Remove();
            XmlHelper.Save(root, this._path);
        }

        /// <summary>
        /// Get list of comments
        /// Called by comment managment
        /// Called by recent comments
        /// </summary>
        /// <returns>List of comments</returns>
        public List<Comment> GetComments()
        {
            List<Comment> comments = new List<Comment>();
            XElement root = null;

            try
            {
                root = XElement.Load(this._path);
            }
            catch (Exception ex)
            {
                Logger.Log(NO_FILE_ERROR, ex);
                throw new ApplicationException(NO_FILE_ERROR, ex);
            }

            try
            {
                foreach (XElement commentElem in root.Elements("Comment"))
                {
                    Comment comment = new Comment();
                    comment.ID = commentElem.Attribute("ID").Value;
                    comment.FileID = commentElem.Attribute("FileID").Value;
                    comment.Name = commentElem.Attribute("Name").Value;
                    comment.Ip = commentElem.Attribute("Ip").Value;
                    comment.Url = commentElem.Attribute("Url").Value;

                    // comment.Text = HttpContext.Current.Server.HtmlDecode(commentElem.Value);
                    comment.Text = HttpContext.Current.Server.HtmlDecode(GetInnerXml(commentElem));

                    // comment.Text = HttpContext.Current.Server.HtmlDecode(comment.Text);
                    try
                    {
                        comment.IsApproved = bool.Parse(commentElem.Attribute("Approved").Value);
                        if (commentElem.Attribute("IsAuthor") != null)
                        {
                            comment.IsAuthor = bool.Parse(commentElem.Attribute("IsAuthor").Value);
                        }

                        comment.Time = DateTime.ParseExact(
                            commentElem.Attribute("Time").Value, 
                            DataContext.DateTimeFormat, 
                            CultureInfo.InvariantCulture);
                    }
                    catch (Exception ex)
                    {
                        string msg = string.Format(DATE_FORMAT_ERROR, comment.ID);
                        Logger.Log(msg, ex);
                        comment.Time = DateTime.MinValue;
                    }

                    comments.Add(comment);
                }
            }
            catch (Exception ex)
            {
                Logger.Log(XML_FORMAT_ERROR, ex);
                throw new ApplicationException(XML_FORMAT_ERROR, ex);
            }

            return comments;
        }

        /// <summary>
        /// Inserts comment into XML
        /// </summary>
        /// <param name="comment">
        /// The Comment
        /// </param>
        /// <returns>
        /// Comment ID
        /// </returns>
        public string Insert(Comment comment)
        {
            XElement root = null;

            try
            {
                root = XElement.Load(this._path);
            }
            catch (Exception ex)
            {
                Logger.Log(NO_FILE_ERROR, ex);
                throw new ApplicationException(NO_FILE_ERROR, ex);
            }

            int id = 0;
            try
            {
                id = int.Parse(root.Attribute("TotalComments").Value);
            }
            catch (Exception ex)
            {
                Logger.Log(XML_FORMAT_ERROR, ex);
            }

            id++;

            root.Attribute("TotalComments").Value = id.ToString();

            XElement commentElem = new XElement(
                "Comment", 
                new XAttribute("ID", id), 
                new XAttribute("FileID", comment.FileID), 
                new XAttribute("Name", comment.Name), 
                new XAttribute("Url", comment.Url), 
                new XAttribute("Ip", comment.Ip), 
                new XAttribute("Time", comment.Time.ToString(DataContext.DateTimeFormat, CultureInfo.InvariantCulture)), 
                new XAttribute("IsAuthor", comment.IsAuthor), 
                new XAttribute("Approved", comment.IsApproved));

            // commentElem.Value = HttpContext.Current.Server.HtmlEncode(comment.Text);
            commentElem.Value = comment.Text;

            try
            {
                root.AddFirst(commentElem);
                int commentCount = root.Elements("Comment").Count<XElement>();
                if (commentCount > 100)
                {
                    List<XElement> removes = new List<XElement>();
                    for (int index = 100; index < commentCount; index++)
                    {
                        removes.Add(root.Elements("Comment").ElementAt(index));
                    }

                    foreach (XElement remove in removes)
                    {
                        remove.Remove();
                    }
                }

                // root.Save(_path);
                XmlHelper.Save(root, this._path);
            }
            catch (Exception ex)
            {
                Logger.Log(XML_FORMAT_ERROR, ex);
                throw new ApplicationException(XML_FORMAT_ERROR, ex);
            }

            return id.ToString();
        }

        /// <summary>
        /// Refreshes the comment XML from comments from the post
        /// Not called by application
        /// </summary>
        public void Refresh()
        {
            List<Comment> comments = new List<Comment>();
            XElement root = null;

            try
            {
                root = XElement.Load(this._path);
            }
            catch (Exception ex)
            {
                Logger.Log(NO_FILE_ERROR, ex);
                throw new ApplicationException(NO_FILE_ERROR, ex);
            }

            root.RemoveAll();
            root.SetAttributeValue("TotalComments", 0);
            root.Save(this._path);

            BlogData blogData = new BlogData();
            List<PostInfo> posts = blogData.GetBlogItems();

            foreach (PostInfo post in posts)
            {
                PostData postData = new PostData();
                Post post2 = postData.Load(post.FileID);
                comments.AddRange(post2.Comments);
            }

            var qry = from cmnt in comments orderby cmnt.Time ascending select cmnt;

            List<Comment> comments2 = qry.ToList<Comment>();
            Dictionary<string, List<Comment>> dic = new Dictionary<string, List<Comment>>();

            foreach (Comment comment in comments2)
            {
                comment.ID = this.Insert(comment);
                if (!dic.ContainsKey(comment.FileID))
                {
                    dic[comment.FileID] = new List<Comment>();
                }

                dic[comment.FileID].Add(comment);
            }

            foreach (string fileID in dic.Keys)
            {
                PostData postData = new PostData();
                Post post = postData.Load(fileID);
                post.Comments = dic[fileID];
                postData.Save(post);
            }
        }

        /// <summary>
        /// The update.
        /// </summary>
        /// <param name="comment">
        /// The comment.
        /// </param>
        /// <exception cref="ApplicationException">
        /// Application exception
        /// </exception>
        public void Update(Comment comment)
        {
            XElement root = null;

            try
            {
                root = XElement.Load(this._path);
            }
            catch (Exception ex)
            {
                Logger.Log(NO_FILE_ERROR, ex);
                throw new ApplicationException(NO_FILE_ERROR, ex);
            }

            var qry = from elem in root.Elements("Comment") where elem.Attribute("ID").Value == comment.ID select elem;

            if (qry.Count<XElement>() != 1)
            {
                string msg = string.Format(NO_COMMENT_ERROR, comment.ID);
                Logger.Log(msg);
                throw new ApplicationException(msg);
            }

            XElement commentElem = qry.First<XElement>();
            commentElem.SetAttributeValue("Name", comment.Name);
            commentElem.SetAttributeValue("Url", comment.Url ?? string.Empty);
            commentElem.SetAttributeValue("IsAuthor", comment.IsAuthor);
            commentElem.Value = comment.Text;

            // root.Save(_path);
            XmlHelper.Save(root, this._path);
        }

        #endregion

        #region Methods

        /// <summary>
        /// The get inner xml.
        /// </summary>
        /// <param name="elem">
        /// The elem.
        /// </param>
        /// <returns>
        /// The System.String.
        /// </returns>
        internal static string GetInnerXml(XElement elem)
        {
            StringBuilder innerXml = new StringBuilder();
            elem.Nodes().ToList().ForEach(node => innerXml.Append(node.ToString()));
            return innerXml.ToString();
        }

        #endregion
    }
}