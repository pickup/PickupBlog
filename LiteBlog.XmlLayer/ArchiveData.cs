// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ArchiveData.cs" company="LiteBlog">
//   Copyright (c) 2012, LiteBlog. All Rights Reserved.
// </copyright>
// <summary>
//   Class that contains the archive data summary
//   Data stored as ArchiveID, year, month, Count (of blog items)
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace LiteBlog.XmlLayer
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;

    using LiteBlog.Common;
    using LiteBlog.Common.Contracts;

    /// <summary>
    /// Class that contains the archive data summary
    /// Data stored as ArchiveID, year, month, Count (of blog items)
    /// </summary>
    public class ArchiveData : IArchiveData
    {
        #region Constants

        /// <summary>
        /// The coun t_ error.
        /// </summary>
        private const string COUNT_ERROR = "Archive count is less than zero";

        /// <summary>
        /// The du p_ mont h_ error.
        /// </summary>
        private const string DUP_MONTH_ERROR = "Duplicate month found while creating new month";

        /// <summary>
        /// The forma t_ error.
        /// </summary>
        private const string FORMAT_ERROR = "Archive count is not in number format";

        /// <summary>
        /// The n o_ fil e_ error.
        /// </summary>
        private const string NO_FILE_ERROR = "Archive file could not be found";

        /// <summary>
        /// The n o_ mont h_ error.
        /// </summary>
        private const string NO_MONTH_ERROR = "Month = {0} could not be found";

        /// <summary>
        /// The xm l_ forma t_ error.
        /// </summary>
        private const string XML_FORMAT_ERROR = "Archive file is not in the right format";

        #endregion

        #region Fields

        /// <summary>
        /// The _path.
        /// </summary>
        private string _path;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ArchiveData"/> class.
        /// </summary>
        public ArchiveData()
        {
            this._path = DataContext.Path + "Archive.xml";
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
                return DataContext.Path + "Archive.xml";
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Increase or decrease the archive node count
        /// Called when post is published or unpublished
        /// </summary>
        /// <param name="archiveID">
        /// ID of archive node
        /// </param>
        /// <param name="num">
        /// count to increment - usually +1/-1
        /// </param>
        public void ChangeCount(string archiveID, int num)
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

            // What if ID does not exist
            var qry = from elem in root.Elements("ArchiveMonth")
                      where elem.Attribute("ID").Value == archiveID
                      select elem;

            if (qry.Count<XElement>() == 0)
            {
                string msg = string.Format(NO_MONTH_ERROR, archiveID);
                Logger.Log(msg);
                return;
            }

            XElement monthElem = qry.First<XElement>();

            try
            {
                int count = 0;
                try
                {
                    count = (int)monthElem.Attribute("Count");
                }
                catch (Exception ex)
                {
                    Logger.Log(FORMAT_ERROR, ex);
                    throw new ApplicationException(FORMAT_ERROR, ex);
                }

                count = count + num;
                if (count < 0)
                {
                    count = 0;
                    Logger.Log(COUNT_ERROR);
                }

                if (count == 0)
                {
                    monthElem.Remove();
                }
                else
                {
                    monthElem.SetAttributeValue("Count", count);
                }
            }
            catch (Exception ex)
            {
                Logger.Log(XML_FORMAT_ERROR, ex);
                throw new ApplicationException(XML_FORMAT_ERROR, ex);
            }

            root.Save(this._path);
        }

        /// <summary>
        /// Create a new node in Archive file for a new month
        /// Contains ID, month, year, count
        /// Called when a new post is published on a new month
        /// </summary>
        /// <param name="month">
        /// The Archive Month
        /// </param>
        public void Create(ArchiveMonth month)
        {
            XElement root = null;

            try
            {
                // file not found
                // invalid XML
                root = XElement.Load(this._path);
            }
            catch (Exception ex)
            {
                Logger.Log(NO_FILE_ERROR, ex);
                throw new ApplicationException(NO_FILE_ERROR, ex);
            }

            // check if month exists, Do not create, if exists
            var qry = from elem in root.Elements("ArchiveMonth") where (int)elem.Attribute("ID") == month.ID select elem;

            if (qry.Count<XElement>() > 0)
            {
                Logger.Log(DUP_MONTH_ERROR);
                throw new ApplicationException(DUP_MONTH_ERROR);
            }

            XElement monthElem = new XElement(
                "ArchiveMonth", 
                new XAttribute("ID", month.ID), 
                new XAttribute("Year", month.Year), 
                new XAttribute("Month", month.Month), 
                new XAttribute("Count", month.Count));

            // first element (latest post in new month)
            // insert somewhere in the past where no post for a month
            bool lastNode = true;
            foreach (XElement refElem in root.Elements("ArchiveMonth"))
            {
                int refArchID = (int)refElem.Attribute("ID");

                if (month.ID > refArchID)
                {
                    refElem.AddBeforeSelf(monthElem);
                    lastNode = false;
                    break;
                }
            }

            // only element
            // last element in the list
            if (lastNode)
            {
                root.Add(monthElem);
            }

            root.Save(this._path);
        }

        /// <summary>
        /// Delete an archive node from the file
        /// Called when post is unpublished with month with only one post
        /// </summary>
        /// <param name="archiveID">
        /// ID of archive to delete
        /// </param>
        public void Delete(string archiveID)
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

            // What if ID does not exist
            var qry = from elem in root.Elements("ArchiveMonth")
                      where elem.Attribute("ID").Value == archiveID
                      select elem;

            if (qry.Count<XElement>() == 0)
            {
                string msg = string.Format(NO_MONTH_ERROR, archiveID);
                Logger.Log(msg);
                return;
            }

            XElement remElem = qry.First<XElement>();
            if (remElem != null)
            {
                remElem.Remove();
            }

            root.Save(this._path);
        }

        /// <summary>
        /// Gets a list of archive months - id / year / month / count
        /// Used by archive user control to display the tree-like structure
        /// </summary>
        /// <returns>List of ArchiveMonth objects</returns>
        public List<ArchiveMonth> GetArchiveMonths()
        {
            List<ArchiveMonth> months = new List<ArchiveMonth>();

            XElement root = null;
            try
            {
                root = XElement.Load(this._path);
            }
            catch (Exception ex)
            {
                // missing file
                // invalid XML in file
                Logger.Log(NO_FILE_ERROR, ex);
                throw new ApplicationException(NO_FILE_ERROR, ex);
            }

            // No Archive month
            foreach (XElement monthElem in root.Elements("ArchiveMonth"))
            {
                // missing attribute
                ArchiveMonth month = new ArchiveMonth();

                try
                {
                    // missing attribute
                    month.ID = int.Parse(monthElem.Attribute("ID").Value);
                    month.Month = int.Parse(monthElem.Attribute("Month").Value);
                    month.Year = int.Parse(monthElem.Attribute("Year").Value);
                    month.Count = int.Parse(monthElem.Attribute("Count").Value);
                }
                catch (Exception ex)
                {
                    Logger.Log(FORMAT_ERROR, ex);
                    continue;
                }

                months.Add(month);
            }

            return months;
        }

        #endregion
    }
}