// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DraftData.cs" company="LiteBlog">
//   Copyright (c) 2012, LiteBlog. All Rights Reserved.
// </copyright>
// <summary>
//   Class that manages the draftpost entry XML
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
    /// Class that manages the draftpost entry XML
    /// </summary>
    public class DraftData : IDraftData
    {
        #region Constants

        /// <summary>
        /// The dat e_ forma t_ error.
        /// </summary>
        private const string DATE_FORMAT_ERROR = "Time field for {0} is not in the right format";

        /// <summary>
        /// The n o_ fil e_ error.
        /// </summary>
        private const string NO_FILE_ERROR = "Blog meta file could not be found";

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
        /// Initializes a new instance of the <see cref="DraftData"/> class.
        /// </summary>
        public DraftData()
        {
            this._path = DataContext.Path + "Draft.xml";
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Adds a schedule attribute
        /// Called when post is published for future
        /// </summary>
        /// <param name="draftID">
        /// Draft ID
        /// </param>
        /// <param name="publishDate">
        /// Publish Date
        /// </param>
        public void AddSchedule(string draftID, DateTime publishDate)
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

            var qry = from elem in root.Elements("Draft") where elem.Attribute("FileID").Value == draftID select elem;

            XElement draftElem = qry.First<XElement>();

            if (draftElem != null)
            {
                draftElem.SetAttributeValue(
                    "Scheduled", publishDate.ToString(DataContext.DateTimeFormat, CultureInfo.InvariantCulture));
            }

            // root.Save(_path);
            XmlHelper.Save(root, this._path);
        }

        /// <summary>
        /// Deletes a draftpost
        /// Called when deleting from draft management
        /// </summary>
        /// <param name="draftID">
        /// Draft ID
        /// </param>
        public void Delete(string draftID)
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

            var qry = from elem in root.Elements("Draft") where elem.Attribute("FileID").Value == draftID select elem;

            XElement draftElem = qry.First<XElement>();

            if (draftElem != null)
            {
                draftElem.Remove();
            }

            // root.Save(_path);
            XmlHelper.Save(root, this._path);
        }

        /// <summary>
        /// Gets a list of draft entries
        /// Called from draft management
        /// </summary>
        /// <returns>List of Draft entries</returns>
        public List<PostInfo> GetDraftItems()
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
                foreach (XElement draftElem in root.Elements("Draft"))
                {
                    PostInfo post = new PostInfo();
                    post.Title = draftElem.Attribute("Title").Value;
                    post.FileID = draftElem.Attribute("FileID").Value;

                    try
                    {
                        post.Time = DateTime.ParseExact(
                            draftElem.Attribute("Date").Value, DataContext.DateTimeFormat, CultureInfo.InvariantCulture);
                    }
                    catch (Exception ex)
                    {
                        string msg = string.Format(DATE_FORMAT_ERROR, post.FileID);
                        Logger.Log(msg, ex);
                        post.Time = DateTime.MinValue;
                    }

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
        /// Gets list of scheduled draft entries
        /// Called by scheduled publisher service
        /// </summary>
        /// <returns>List of draft entries</returns>
        public List<PostInfo> GetScheduledDraftItems()
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

            var qry = from elem in root.Elements("Draft") where elem.Attribute("Scheduled") != null select elem;

            try
            {
                foreach (XElement draftElem in qry)
                {
                    PostInfo post = new PostInfo();
                    post.Title = draftElem.Attribute("Title").Value;
                    post.FileID = draftElem.Attribute("FileID").Value;

                    try
                    {
                        post.Time = DateTime.ParseExact(
                            draftElem.Attribute("Scheduled").Value, 
                            DataContext.DateTimeFormat, 
                            CultureInfo.InvariantCulture);
                    }
                    catch (Exception ex)
                    {
                        string msg = string.Format(DATE_FORMAT_ERROR, post.FileID);
                        Logger.Log(msg, ex);
                        post.Time = DateTime.MaxValue;
                    }

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
        /// Inserts a draftpost entry
        /// Called when creating a new post
        /// Called when editing a published post
        /// </summary>
        /// <param name="draftID">
        /// Draft ID
        /// </param>
        /// <param name="title">
        /// The Title
        /// </param>
        public void Insert(string draftID, string title)
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

            TimeZoneInfo tzi = SettingsData.TimeZoneInfo;

            XElement draftElem = new XElement(
                "Draft", 
                new XAttribute("FileID", draftID), 
                new XAttribute("Title", title), 
                new XAttribute(
                    "Date", 
                    LocalTime.GetCurrentTime(tzi).ToString(DataContext.DateTimeFormat, CultureInfo.InvariantCulture)));

            root.Add(draftElem);

            // root.Save(_path);
            XmlHelper.Save(root, this._path);
        }

        #endregion
    }
}