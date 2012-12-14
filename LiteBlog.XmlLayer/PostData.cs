// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PostData.cs" company="LiteBlog">
//   Copyright (c) 2012, LiteBlog. All Rights Reserved.
// </copyright>
// <summary>
//   Class that stores the Post XML
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace LiteBlog.XmlLayer
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Web;
    using System.Xml;
    using System.Xml.Linq;

    using LiteBlog.Common;
    using LiteBlog.Common.Contracts;

    /// <summary>
    /// Class that stores the Post XML
    /// </summary>
    public class PostData : IPostData
    {
        #region Constants

        /// <summary>
        /// The coun t_ forma t_ error.
        /// </summary>
        private const string COUNT_FORMAT_ERROR = "Comments count is not in right format for Post = {0}";

        /// <summary>
        /// The dat e_ forma t_ error.
        /// </summary>
        private const string DATE_FORMAT_ERROR = "Post Time is not in date format for Post = {0}";

        /// <summary>
        /// The n o_ commen t_ error.
        /// </summary>
        private const string NO_COMMENT_ERROR = "Comment = {0} could not be found";

        /// <summary>
        /// The n o_ fil e_ error.
        /// </summary>
        private const string NO_FILE_ERROR = "Post file could not be found";

        /// <summary>
        /// The xm l_ forma t_ error.
        /// </summary>
        private const string XML_FORMAT_ERROR = "Post file is not in the right format";

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Deletes post 
        /// Called from post management
        /// </summary>
        /// <param name="fileID">
        /// Post ID
        /// </param>
        public void Delete(string fileID)
        {
            string path = this.GetFilePath(fileID);
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }

        /// <summary>
        /// Delete comment
        /// Called from comment management
        /// </summary>
        /// <param name="comment">
        /// The Comment
        /// </param>
        public void DeleteComment(Comment comment)
        {
            string path = this.GetFilePath(comment.FileID);
            XElement root = null;

            try
            {
                root = XElement.Load(path);
            }
            catch (Exception ex)
            {
                Logger.Log(NO_FILE_ERROR, ex);
                throw new ApplicationException(NO_FILE_ERROR, ex);
            }

            var qry = from elem in root.Element("Comments").Elements("Comment")
                      where elem.Attribute("ID").Value == comment.ID
                      select elem;

            XElement commentElem = qry.First<XElement>();

            if (commentElem == null)
            {
                string msg = string.Format(NO_COMMENT_ERROR, comment.ID);
                Logger.Log(msg);
                throw new ApplicationException(msg);
            }

            commentElem.Remove();

            try
            {
                int count = int.Parse(root.Attribute("Comments").Value);
                count--;
                root.SetAttributeValue("Comments", count);
            }
            catch (Exception ex)
            {
                string msg = string.Format(COUNT_FORMAT_ERROR, comment.FileID);
                Logger.Log(msg, ex);
            }

            root.Save(path);
        }

        /// <summary>
        /// Inserts comment into a post
        /// Called when user enters a comment
        /// </summary>
        /// <param name="comment">
        /// The Comment
        /// </param>
        public void InsertComment(Comment comment)
        {
            string path = this.GetFilePath(comment.FileID);

            XElement root = null;
            try
            {
                root = XElement.Load(path);
            }
            catch (Exception ex)
            {
                Logger.Log(NO_FILE_ERROR, ex);
                throw new ApplicationException(NO_FILE_ERROR, ex);
            }

            XElement commentsElem = root.Element("Comments");
            if (commentsElem != null)
            {
                XElement commentElem = new XElement(
                    "Comment", 
                    new XAttribute("ID", comment.ID), 
                    new XAttribute("Name", comment.Name), 
                    new XAttribute("Url", comment.Url), 
                    new XAttribute(
                        "Time", comment.Time.ToString(DataContext.DateTimeFormat, CultureInfo.InvariantCulture)), 
                    new XAttribute("Ip", comment.Ip), 
                    new XAttribute("IsAuthor", comment.IsAuthor));

                commentElem.Value = comment.Text;
                commentsElem.AddFirst(commentElem);
            }

            try
            {
                int count = int.Parse(root.Attribute("Comments").Value);
                count++;
                root.SetAttributeValue("Comments", count);
            }
            catch (Exception ex)
            {
                string msg = string.Format(COUNT_FORMAT_ERROR, comment.FileID);
                Logger.Log(msg, ex);
            }

            root.Save(path);
        }

        /// <summary>
        /// Gets the post data
        /// Called when displaying a post
        /// </summary>
        /// <param name="fileID">
        /// Post ID
        /// </param>
        /// <returns>
        /// The Post
        /// </returns>
        public Post Load(string fileID)
        {
            string path = this.GetFilePath(fileID);

            XElement root = null;

            try
            {
                root = XElement.Load(path);
            }
            catch (Exception ex)
            {
                Logger.Log(NO_FILE_ERROR, ex);
                throw new ApplicationException(NO_FILE_ERROR, ex);
            }

            Post post = new Post();
            XElement postElem = root;

            try
            {
                post.FileID = postElem.Attribute("FileID").Value;
                post.Title = postElem.Attribute("Title").Value;
                post.Author = postElem.Attribute("Author").Value;
                post.CatID = postElem.Attribute("CatID").Value;

                try
                {
                    post.Time = DateTime.ParseExact(
                        postElem.Attribute("Time").Value, DataContext.DateTimeFormat, CultureInfo.InvariantCulture);
                }
                catch (Exception ex)
                {
                    string msg = string.Format(DATE_FORMAT_ERROR, fileID);
                    Logger.Log(msg, ex);
                    post.Time = DateTime.MinValue;
                }

                XElement docsElem = postElem.Element("Documents");
                if (docsElem != null)
                {
                    foreach (XElement docElem in docsElem.Elements("Document"))
                    {
                        Document document = new Document();
                        document.Path = docElem.Attribute("Path").Value;
                        post.Documents.Add(document);
                    }
                }

                string contents = HttpContext.Current.Server.HtmlDecode(GetInnerXml(postElem.Element("Content")));
                post.Contents = contents;

                XElement commentsElem = postElem.Element("Comments");
                if (commentsElem != null)
                {
                    foreach (XElement commentElem in commentsElem.Elements("Comment"))
                    {
                        Comment comment = new Comment();
                        comment.FileID = post.FileID;
                        comment.ID = commentElem.Attribute("ID").Value;
                        comment.Name = commentElem.Attribute("Name").Value;
                        comment.Text = HttpContext.Current.Server.HtmlDecode(GetInnerXml(commentElem));

                        // comment.Text = HttpContext.Current.Server.HtmlDecode(comment.Text);
                        comment.Url = commentElem.Attribute("Url").Value;
                        comment.Time = DateTime.ParseExact(
                            commentElem.Attribute("Time").Value, 
                            DataContext.DateTimeFormat, 
                            CultureInfo.InvariantCulture);
                        comment.Ip = commentElem.Attribute("Ip").Value;
                        if (commentElem.Attribute("IsAuthor") != null)
                        {
                            comment.IsAuthor = bool.Parse(commentElem.Attribute("IsAuthor").Value);
                        }

                        post.Comments.Add(comment);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Log(XML_FORMAT_ERROR, ex);
                throw new ApplicationException(XML_FORMAT_ERROR, ex);
            }

            return post;
        }

        /// <summary>
        /// Saves the content of the post in XML
        /// Called during post creation / editing / AutoSave
        /// </summary>
        /// <param name="post">
        /// The Post
        /// </param>
        public void Save(Post post)
        {
            XElement postElem = null;
            string path = this.GetFilePath(post.FileID);

            string template = "<?xml version=\"1.0\" encoding=\"utf-8\"?>";
            template += "<Post Title=\"{0}\" FileID=\"{1}\" Author=\"{2}\" Time=\"{3}\" CatID=\"{4}\" Comments=\"{5}\">";
            template += "<Documents></Documents>";
            template += "<Content></Content>";
            template += "<Comments></Comments></Post>";

            template = string.Format(
                template, 
                post.Title, 
                post.FileID, 
                post.Author, 
                post.Time.ToString(DataContext.DateTimeFormat, CultureInfo.InvariantCulture), 
                post.CatID, 
                post.Comments.Count);

            try
            {
                postElem = XElement.Parse(template);
            }
            catch (Exception ex)
            {
                Logger.Log(XML_FORMAT_ERROR, ex);
                throw new ApplicationException(XML_FORMAT_ERROR, ex);
            }

            try
            {
                XElement docsElem = postElem.Element("Documents");

                foreach (Document document in post.Documents)
                {
                    XElement docElem = new XElement("Document", new XAttribute("Path", document.Path));
                    docsElem.Add(docElem);
                }

                string contents2 = HttpContext.Current.Server.HtmlEncode(post.Contents);

                // postElem.Element("Content").Value = contents2;
                postElem.Element("Content").Value = post.Contents;

                XElement commentsElem = postElem.Element("Comments");
                commentsElem.RemoveAll();

                foreach (Comment comment in post.Comments)
                {
                    XElement commentElem = new XElement(
                        "Comment", 
                        new XAttribute("ID", comment.ID), 
                        new XAttribute("Name", comment.Name), 
                        new XAttribute("Url", comment.Url), 
                        new XAttribute("IsAuthor", comment.IsAuthor), 
                        new XAttribute(
                            "Time", comment.Time.ToString(DataContext.DateTimeFormat, CultureInfo.InvariantCulture)), 
                        new XAttribute("Ip", comment.Ip));

                    // commentElem.Value = HttpContext.Current.Server.HtmlEncode(comment.Text);
                    commentElem.Value = comment.Text;
                    commentsElem.Add(commentElem);
                }

                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Indent = true;
                XmlWriter writer = XmlWriter.Create(path, settings);
                postElem.Save(writer);
                writer.Close();
            }
            catch (Exception ex)
            {
                Logger.Log(XML_FORMAT_ERROR, ex);
                throw new ApplicationException(XML_FORMAT_ERROR, ex);
            }
        }

        /// <summary>
        /// The update comment.
        /// </summary>
        /// <param name="comment">
        /// The comment.
        /// </param>
        /// <exception cref="ApplicationException">
        /// Application exception
        /// </exception>
        public void UpdateComment(Comment comment)
        {
            string path = this.GetFilePath(comment.FileID);

            XElement root = null;
            try
            {
                root = XElement.Load(path);
            }
            catch (Exception ex)
            {
                Logger.Log(NO_FILE_ERROR, ex);
                throw new ApplicationException(NO_FILE_ERROR, ex);
            }

            var qry = from elem in root.Element("Comments").Elements("Comment")
                      where elem.Attribute("ID").Value == comment.ID
                      select elem;

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

            root.Save(path);
        }

        #endregion

        #region Methods

        /// <summary>
        /// The get cache path.
        /// </summary>
        /// <param name="fileID">
        /// The file id.
        /// </param>
        /// <returns>
        /// The System.String.
        /// </returns>
        internal static string GetCachePath(string fileID)
        {
            return string.Format("{0}Posts/{1}.xml", DataContext.Path, fileID);
        }

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

        /// <summary>
        /// The get file path.
        /// </summary>
        /// <param name="fileID">
        /// The file id.
        /// </param>
        /// <returns>
        /// The System.String.
        /// </returns>
        private string GetFilePath(string fileID)
        {
            return string.Format("{0}Posts/{1}.xml", DataContext.Path, fileID);
        }

        #endregion
    }
}