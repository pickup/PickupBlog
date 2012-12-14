// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DraftPostData.cs" company="LiteBlog">
//   Copyright (c) 2012, LiteBlog. All Rights Reserved.
// </copyright>
// <summary>
//   Class that stores draftpost
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace LiteBlog.XmlLayer
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Web;
    using System.Xml;
    using System.Xml.Linq;

    using LiteBlog.Common;
    using LiteBlog.Common.Contracts;

    /// <summary>
    /// Class that stores draftpost
    /// </summary>
    public class DraftPostData : IDraftPostData
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
        /// Creates a draft post
        /// Called when creating a new post
        /// </summary>
        /// <param name="title">
        /// The Title
        /// </param>
        /// <param name="author">
        /// The Author
        /// </param>
        /// <param name="time">
        /// The Time
        /// </param>
        /// <param name="catID">
        /// Category ID
        /// </param>
        /// <returns>
        /// Draft Post
        /// </returns>
        public DraftPost Create(string title, string author, DateTime time, string catID)
        {
            DraftPost post = new DraftPost();

            post.FileID = string.Empty;
            DateTime now = LocalTime.GetCurrentTime(SettingsData.TimeZoneInfo);
            post.DraftID = DraftPost.GetNewDraftID(this.GetUniqueFileID(title), now);
            post.Time = time;
            post.CatID = catID;
            post.Title = title;
            post.Author = author;
            post.Type = PostType.New;

            string xmlTime = post.Time.ToString(DataContext.DateTimeFormat, CultureInfo.InvariantCulture);

            string template = "<?xml version=\"1.0\" encoding=\"utf-8\"?>";
            template +=
                "<Post Title=\"{0}\" FileID=\"{1}\" Author=\"{2}\" Time=\"{3}\" CatID=\"{4}\" Comments=\"0\" Type=\"New\">";
            template += "<Documents></Documents>";
            template += "<Content></Content>";
            template += "<Comments></Comments></Post>";

            string xml = string.Format(template, title, post.FileID, author, xmlTime, catID);

            XElement postElem = XElement.Parse(xml);

            string path = this.GetFilePath(post.DraftID);
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            XmlWriter writer = XmlWriter.Create(path, settings);
            postElem.Save(writer);
            writer.Close();

            DraftData draft = new DraftData();
            draft.Insert(post.DraftID, title);
            return post;
        }

        /// <summary>
        /// Creates draft post
        /// Called when editing a published post
        /// </summary>
        /// <param name="fileID">
        /// Post ID
        /// </param>
        /// <returns>
        /// Draft Post
        /// </returns>
        public DraftPost Create(string fileID)
        {
            PostData data = new PostData();
            Post post = data.Load(fileID);
            DraftPost draft = new DraftPost(post);
            DateTime now = LocalTime.GetCurrentTime(SettingsData.TimeZoneInfo);
            draft.DraftID = DraftPost.GetNewDraftID(fileID, now);
            this.Save(draft);
            DraftData data2 = new DraftData();
            data2.Insert(draft.DraftID, draft.Title);
            return draft;
        }

        /// <summary>
        /// Deletes draft
        /// Called from draft management
        /// </summary>
        /// <param name="fileID">
        /// Draft ID
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
        /// Loads draft
        /// Called when creating a post
        /// </summary>
        /// <param name="draftID">
        /// Draft ID
        /// </param>
        /// <returns>
        /// Draft Post
        /// </returns>
        public DraftPost Load(string draftID)
        {
            string path = this.GetFilePath(draftID);

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

            XElement postElem = root;
            DraftPost post = new DraftPost();

            try
            {
                post.FileID = postElem.Attribute("FileID").Value;
                post.DraftID = draftID;
                post.Title = postElem.Attribute("Title").Value;
                post.Author = postElem.Attribute("Author").Value;
                post.CatID = postElem.Attribute("CatID").Value;
                XAttribute attr = postElem.Attribute("Type");
                if (attr != null)
                {
                    post.Type = (PostType)Enum.Parse(typeof(PostType), attr.Value, true);
                }

                // Load draft data, if we are loading a draft
                if (post.Type == PostType.Draft)
                {
                    if (postElem.Attribute("OldCatID") != null)
                    {
                        post.OldCatID = postElem.Attribute("OldCatID").Value;
                    }
                }

                try
                {
                    post.Time = DateTime.ParseExact(
                        postElem.Attribute("Time").Value, DataContext.DateTimeFormat, CultureInfo.InvariantCulture);
                }
                catch (Exception ex)
                {
                    string msg = string.Format(DATE_FORMAT_ERROR, draftID);
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

                // string contents = HttpContext.Current.Server.HtmlDecode(postElem.Element("Content").Value);               
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
                        comment.Url = commentElem.Attribute("Url").Value;
                        comment.Time = DateTime.ParseExact(
                            commentElem.Attribute("Time").Value, 
                            DataContext.DateTimeFormat, 
                            CultureInfo.InvariantCulture);
                        comment.Ip = commentElem.Attribute("Ip").Value;

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
        /// Publish the post
        /// Draft posts will merge the comments
        /// New posts will get a unique file id
        /// OldCatID, DraftType etc will be removed
        /// </summary>
        /// <param name="post">
        /// Draft Post
        /// </param>
        /// <returns>
        /// Post ID
        /// </returns>
        public string Publish(DraftPost post)
        {
            if (post.Type == PostType.Draft)
            {
                string draftPath = this.GetFilePath(post.DraftID);
                XElement root = null;
                root = XElement.Load(draftPath);
                XAttribute attr = root.Attribute("OldCatID");
                if (attr != null)
                {
                    attr.Remove();
                }

                XAttribute attr2 = root.Attribute("Type");
                if (attr2 != null)
                {
                    attr2.Remove();
                }

                // Get a copy of the post to merge comments
                PostData data = new PostData();
                Post existPost = data.Load(post.FileID);

                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Indent = true;
                string publishPath = this.GetFilePath(post.FileID);
                XmlWriter writer = XmlWriter.Create(publishPath, settings);
                root.Save(writer);
                writer.Close();

                // Merge comments
                Post publishPost = data.Load(post.FileID);
                publishPost.Comments = existPost.Comments;
                data.Save(publishPost);

                System.IO.File.Delete(draftPath);
            }

            if (post.Type == PostType.New)
            {
                string filePath = this.GetFilePath(post.DraftID);
                XElement root = null;
                root = XElement.Load(filePath);

                XAttribute attr2 = root.Attribute("Type");
                if (attr2 != null)
                {
                    attr2.Remove();
                }

                post.FileID = this.GetUniqueFileID(post.Title);

                root.Attribute("FileID").Value = post.FileID;

                string publishPath = this.GetFilePath(post.FileID);
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Indent = true;
                XmlWriter writer = XmlWriter.Create(publishPath, settings);
                root.Save(writer);
                writer.Close();
                System.IO.File.Delete(filePath);
            }

            // Newly published posts will get their file ID here.
            return post.FileID;
        }

        /// <summary>
        /// Save draft post
        /// Called while creating / editing post
        /// </summary>
        /// <param name="post">
        /// Draft Post
        /// </param>
        public void Save(DraftPost post)
        {
            XElement root = null;
            string path = this.GetFilePath(post.DraftID);

            string template = "<?xml version=\"1.0\" encoding=\"utf-8\"?>";
            template +=
                "<Post Title=\"{0}\" FileID=\"{1}\" Author=\"{2}\" Time=\"{3}\" CatID=\"{4}\" Comments=\"{5}\" Type=\"{6}\">";
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
                post.Comments.Count, 
                post.Type.ToString());

            try
            {
                root = XElement.Parse(template);
            }
            catch (Exception ex)
            {
                Logger.Log(XML_FORMAT_ERROR, ex);
                throw new ApplicationException(XML_FORMAT_ERROR, ex);
            }

            XElement postElem = root;

            try
            {
                XElement docsElem = postElem.Element("Documents");

                foreach (Document document in post.Documents)
                {
                    XElement docElem = new XElement("Document", new XAttribute("Path", document.Path));
                    docsElem.Add(docElem);
                }

                // string contents2 = HttpContext.Current.Server.HtmlEncode(post.Contents);
                // postElem.Element("Content").Value = contents2;
                postElem.Element("Content").Value = post.Contents;

                XElement commentsElem = postElem.Element("Comments");
                foreach (Comment comment in post.Comments)
                {
                    XElement commentElem = new XElement(
                        "Comment", 
                        new XAttribute("ID", comment.ID), 
                        new XAttribute("Name", comment.Name), 
                        new XAttribute("Url", comment.Url), 
                        new XAttribute(
                            "Time", comment.Time.ToString(DataContext.DateTimeFormat, CultureInfo.InvariantCulture)), 
                        new XAttribute("Ip", comment.Ip));

                    // commentElem.Value = HttpContext.Current.Server.HtmlEncode(comment.Text);
                    commentElem.Value = comment.Text;
                    commentsElem.Add(commentElem);
                }

                if (post.Type == PostType.Draft)
                {
                    // Save the old CatID attribute
                    postElem.SetAttributeValue("OldCatID", post.OldCatID);
                }

                if (!File.Exists(path))
                {
                    FileStream fs2 = File.Create(path);
                    fs2.Close();
                }

                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Indent = true;
                using (FileStream fs = File.Open(path, FileMode.Truncate, FileAccess.Write, FileShare.None))
                {
                    using (XmlWriter writer = XmlWriter.Create(fs, settings))
                    {
                        root.Save(writer);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Log(XML_FORMAT_ERROR, ex);
                throw new ApplicationException(XML_FORMAT_ERROR, ex);
            }
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
            return string.Format(@"{0}Posts\{1}.xml", DataContext.Path, fileID);
        }

        /// <summary>
        /// The get unique file id.
        /// </summary>
        /// <param name="title">
        /// The title.
        /// </param>
        /// <returns>
        /// The System.String.
        /// </returns>
        private string GetUniqueFileID(string title)
        {
            string fileID = Regex.Replace(title.Replace(" ", "-"), @"[^\w-]", string.Empty, RegexOptions.None);
            string path = this.GetFilePath(fileID);

            if (File.Exists(path))
            {
                int index = 1;
                string tmpID = string.Format("{0}-{1}", fileID, index);
                path = this.GetFilePath(tmpID);
                while (File.Exists(path))
                {
                    index++;
                    tmpID = string.Format("{0}-{1}", fileID, index);
                    path = this.GetFilePath(tmpID);
                }

                fileID = tmpID;
            }

            return fileID;
        }

        #endregion
    }
}