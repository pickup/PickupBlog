// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BlogData.cs" company="LiteBlog">
//   Copyright (c) 2012, LiteBlog. All Rights Reserved.
// </copyright>
// <summary>
//   Class that manages the post metadata XML
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace LiteBlog.XmlLayer
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Xml.Linq;

    using LiteBlog.Common;
    using LiteBlog.Common.Contracts;

    /// <summary>
    /// Class that manages the post metadata XML
    /// </summary>
    public class BlogData : IBlogData
    {
        #region Constants

        /// <summary>
        /// The dat e_ forma t_ error.
        /// </summary>
        private const string DATE_FORMAT_ERROR = "Time field for {0} is not in the right format";

        /// <summary>
        /// The du p_ pos t_ error.
        /// </summary>
        private const string DUP_POST_ERROR = "A duplicate post was found in the blog meta file";

        /// <summary>
        /// The n o_ fil e_ error.
        /// </summary>
        private const string NO_FILE_ERROR = "Blog meta file could not be found";

        /// <summary>
        /// The n o_ pos t_ error.
        /// </summary>
        private const string NO_POST_ERROR = "Post = {0} could not be found";

        /// <summary>
        /// The views format error
        /// </summary>
        private const string VIEWS_FORMAT_ERROR = "Views field for {0} is not in the right format";

        /// <summary>
        /// The xm l_ forma t_ error.
        /// </summary>
        private const string XML_FORMAT_ERROR = "Blog meta file is not in the right format";

        #endregion

        #region Fields

        /// <summary>
        /// The _path.
        /// </summary>
        private string _path;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BlogData"/> class.
        /// </summary>
        public BlogData()
        {
            this._path = DataContext.Path + "Blog.xml";
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
                return DataContext.Path + "Blog.xml";
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Change author of the post
        /// Called from user management when author name is changed
        /// </summary>
        /// <param name="fileID">
        /// Post ID
        /// </param>
        /// <param name="author">
        /// The Author
        /// </param>
        public void ChangeAuthor(string fileID, string author)
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

            var qry = from elem in root.Elements("Post") where elem.Attribute("FileID").Value == fileID select elem;

            if (qry.Count<XElement>() == 0)
            {
                string msg = string.Format(NO_POST_ERROR, fileID);
                Logger.Log(msg);
                throw new ApplicationException(msg);
            }

            XElement postElem = qry.First<XElement>();
            postElem.SetAttributeValue("Author", author);

            // root.Save(_path);
            XmlHelper.Save(root, this._path);
        }

        /// <summary>
        /// Change category of post
        /// Called from category management
        /// </summary>
        /// <param name="fileID">
        /// Post ID
        /// </param>
        /// <param name="oldCatID">
        /// Old Category ID
        /// </param>
        /// <param name="newCatID">
        /// New Category ID
        /// </param>
        public void ChangeCategory(string fileID, string oldCatID, string newCatID)
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

            var qry = from elem in root.Elements("Post") where elem.Attribute("FileID").Value == fileID select elem;

            if (qry.Count<XElement>() == 0)
            {
                string msg = string.Format(NO_POST_ERROR, fileID);
                Logger.Log(msg);
                throw new ApplicationException(msg);
            }

            XElement postElem = qry.First<XElement>();

            string[] catIDs = postElem.Attribute("CatID").Value.Split(
                new[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            string newCatIDs = string.Empty;
            bool bFound = false;
            foreach (string catID in catIDs)
            {
                if (catID == oldCatID)
                {
                    if (newCatID != string.Empty)
                    {
                        newCatIDs += newCatID + ",";
                    }

                    bFound = true;
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

            postElem.SetAttributeValue("CatID", newCatIDs);

            if (bFound)
            {
                // XmlWriterSettings settings = new XmlWriterSettings();
                // settings.Indent = true;
                // XmlWriter writer = XmlWriter.Create(_path, settings);
                // root.Save(writer);
                // writer.Close();
                XmlHelper.Save(root, this._path);
            }
        }

        /// <summary>
        /// Change number of views
        /// </summary>
        /// <param name="fileID">
        /// Post ID
        /// </param>
        /// <param name="views">
        /// The Views
        /// </param>
        public void ChangeViews(string fileID, int views)
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

            var qry = from elem in root.Elements("Post") where elem.Attribute("FileID").Value == fileID select elem;

            if (qry.Count<XElement>() == 0)
            {
                string msg = string.Format(NO_POST_ERROR, fileID);
                Logger.Log(msg);
            }
            else
            {
                XElement postElem = qry.First<XElement>();
                postElem.SetAttributeValue("Views", views);
                XmlHelper.Save(root, this._path);
            }
        }

        /// <summary>
        /// Creates a new post node for published posts
        /// Called at the time of publishing a post
        /// </summary>
        /// <param name="postInfo">
        /// The PostInfo
        /// </param>
        public void Create(PostInfo postInfo)
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

            var qry = from elem in root.Elements("Post")
                      where elem.Attribute("FileID").Value == postInfo.FileID
                      select elem;

            if (qry.Count<XElement>() > 0)
            {
                Logger.Log(DUP_POST_ERROR);
                throw new ApplicationException(DUP_POST_ERROR);
            }

            XElement postElem = new XElement(
                "Post", 
                new XAttribute("Title", postInfo.Title), 
                new XAttribute("FileID", postInfo.FileID), 
                new XAttribute("Author", postInfo.Author), 
                new XAttribute("CatID", postInfo.CatID), 
                new XAttribute("MonthID", postInfo.MonthID), 
                new XAttribute("Time", postInfo.Time.ToString(DataContext.DateTimeFormat, CultureInfo.InvariantCulture)), 
                new XAttribute("Views", 1));

            try
            {
                bool lastNode = true;
                foreach (XElement refElem in root.Elements("Post"))
                {
                    DateTime refDate;
                    try
                    {
                        refDate = DateTime.ParseExact(
                            refElem.Attribute("Time").Value, DataContext.DateTimeFormat, CultureInfo.InvariantCulture);
                    }
                    catch (Exception ex)
                    {
                        string msg = string.Format(DATE_FORMAT_ERROR, refElem.Attribute("FileID").Value);
                        Logger.Log(msg, ex);
                        refDate = DateTime.MaxValue;
                    }

                    if (postInfo.Time > refDate)
                    {
                        refElem.AddBeforeSelf(postElem);
                        lastNode = false;
                        break;
                    }
                }

                if (lastNode)
                {
                    root.Add(postElem);
                }

                // root.Save(_path);
                XmlHelper.Save(root, this._path);
            }
            catch (Exception ex)
            {
                Logger.Log(XML_FORMAT_ERROR, ex);
                throw new ApplicationException(XML_FORMAT_ERROR, ex);
            }
        }

        /// <summary>
        /// Deletes a post entry
        /// Called when published post is deleted
        /// </summary>
        /// <param name="fileID">
        /// Post ID
        /// </param>
        public void Delete(string fileID)
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

            var qry = from elem in root.Elements("Post") where elem.Attribute("FileID").Value == fileID select elem;

            if (qry.Count<XElement>() == 0)
            {
                string msg = string.Format(NO_POST_ERROR, fileID);
                Logger.Log(msg);
                throw new ApplicationException(msg);
            }

            XElement postElem = qry.First<XElement>();
            postElem.Remove();

            // root.Save(_path);
            XmlHelper.Save(root, this._path);
        }

        /// <summary>
        /// Gets list of post items
        /// Called from home page
        /// Called from archive page
        /// Called from category page
        /// Called from post management page
        /// Caching disables frequent access
        /// </summary>
        /// <returns>List of Posts</returns>
        public List<PostInfo> GetBlogItems()
        {
            List<PostInfo> posts = new List<PostInfo>();

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
                foreach (XElement blogElem in root.Elements("Post"))
                {
                    PostInfo post = new PostInfo();
                    post.Title = blogElem.Attribute("Title").Value;
                    if (blogElem.Attribute("Author") != null)
                    {
                        post.Author = blogElem.Attribute("Author").Value;
                    }

                    post.FileID = blogElem.Attribute("FileID").Value;
                    post.CatID = blogElem.Attribute("CatID").Value;

                    try
                    {
                        post.Time = DateTime.ParseExact(
                            blogElem.Attribute("Time").Value, DataContext.DateTimeFormat, CultureInfo.InvariantCulture);
                    }
                    catch (Exception ex)
                    {
                        string msg = string.Format(DATE_FORMAT_ERROR, post.FileID);
                        Logger.Log(msg, ex);
                        post.Time = DateTime.MinValue;
                    }

                    try
                    {
                        post.Views = int.Parse(blogElem.Attribute("Views").Value);
                    }
                    catch (Exception ex)
                    {
                        string msg = string.Format(VIEWS_FORMAT_ERROR, post.FileID);
                        Logger.Log(msg, ex);
                        post.Views = 1;
                    }

                    post.MonthID = blogElem.Attribute("MonthID").Value;
                    posts.Add(post);
                }
            }
            catch (Exception ex)
            {
                Logger.Log(XML_FORMAT_ERROR, ex);
                throw new ApplicationException(XML_FORMAT_ERROR, ex);
            }

            return posts;
        }

        /// <summary>
        /// Update the post node
        /// Called when a published post is re-published
        /// </summary>
        /// <param name="fileID">
        /// FileID of post
        /// </param>
        /// <param name="title">
        /// Post Title
        /// </param>
        /// <param name="catID">
        /// Category ID
        /// </param>
        public void Update(string fileID, string title, string catID)
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

            var qry = from elem in root.Elements("Post") where elem.Attribute("FileID").Value == fileID select elem;

            if (qry.Count<XElement>() == 0)
            {
                string msg = string.Format(NO_POST_ERROR, fileID);
                Logger.Log(msg);
                throw new ApplicationException(msg);
            }

            XElement postElem = qry.First<XElement>();

            postElem.SetAttributeValue("Title", title);
            postElem.SetAttributeValue("CatID", catID);

            // root.Save(_path);
            XmlHelper.Save(root, this._path);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Checks whether a post exists
        /// </summary>
        /// <param name="fileID">
        /// Post ID
        /// </param>
        /// <returns>
        /// True / False
        /// </returns>
        internal bool PostExists(string fileID)
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

            var qry = from elem in root.Elements("Post") where elem.Attribute("FileID").Value == fileID select elem;

            return qry.Count<XElement>() > 0;
        }

        #endregion
    }
}