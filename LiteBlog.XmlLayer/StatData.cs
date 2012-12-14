// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StatData.cs" company="LiteBlog">
//   Copyright (c) 2012, LiteBlog. All Rights Reserved.
// </copyright>
// <summary>
//   Class that manages statistics data in XML
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
    /// Class that manages statistics data in XML
    /// </summary>
    public class StatData : IStatData
    {
        #region Constants

        /// <summary>
        /// The n o_ fil e_ error.
        /// </summary>
        private const string NO_FILE_ERROR = "Stat file could not be found";

        /// <summary>
        /// The xm l_ forma t_ error.
        /// </summary>
        private const string XML_FORMAT_ERROR = "Stat file is not in the right format";

        #endregion

        #region Fields

        /// <summary>
        /// The _path.
        /// </summary>
        private string _path;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="StatData"/> class.
        /// </summary>
        public StatData()
        {
            this._path = DataContext.Path + "Stat.xml";
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the path.
        /// </summary>
        public static string Path
        {
            get
            {
                return DataContext.Path + "Stat.xml";
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Loads statistics data into a Stat object
        /// Called by Application Start event
        /// </summary>
        /// <returns>The Stat object</returns>
        public Stat Load()
        {
            Stat stat = new Stat();
            List<Tuple<PostInfo, int>> popular = new List<Tuple<PostInfo, int>>();
            XElement root = null;

            try
            {
                root = XElement.Load(this._path);
                stat.Path = this._path;
            }
            catch (Exception ex)
            {
                Logger.Log(NO_FILE_ERROR, ex);
                throw new ApplicationException(NO_FILE_ERROR, ex);
            }

            try
            {
                List<XElement> feedElems = new List<XElement>();
                List<XElement> delPostElems = new List<XElement>();

                // BlogData blogData = new BlogData();
                // List<PostInfo> posts = blogData.GetBlogItems();
                foreach (XElement statElem in root.Elements("Stat"))
                {
                    string id = statElem.Attribute("id").Value;
                    int count = int.Parse(statElem.Attribute("Count").Value);

                    // get feed nodes first
                    if (Stat.IsFeedKey(id))
                    {
                        feedElems.Add(statElem);
                        stat.Feeds[id] = count;
                        continue;
                    }

                    switch (id)
                    {
                        case "Visits":
                            stat.Visits = count;
                            break;
                        case "Hits":
                            stat.Hits = count;
                            break;
                    }
                }

                foreach (XElement feedElem in feedElems)
                {
                    feedElem.Remove();
                }

                BlogData blogData = new BlogData();
                stat.PageVisits = blogData.GetBlogItems().OrderByDescending(p => p.Views).ToDictionary(p => p.FileID);
            }
            catch (Exception ex)
            {
                Logger.Log(XML_FORMAT_ERROR, ex);
                throw new ApplicationException(XML_FORMAT_ERROR, ex);
            }

            root.Save(this._path);

            return stat;
        }

        /// <summary>
        /// Saves statistics data into XML
        /// Called by application end event
        /// Called by Session end event - once per hour 
        /// </summary>
        /// <param name="stat">
        /// The Stat
        /// </param>
        public void Save(Stat stat)
        {
            XElement root = null;

            try
            {
                root = XElement.Load(stat.Path);
            }
            catch (Exception ex)
            {
                // Logger.Log(NO_FILE_ERROR, ex);
                throw new ApplicationException(NO_FILE_ERROR, ex);
            }

            try
            {
                var qry = from elem in root.Elements("Stat") where elem.Attribute("id").Value == "Visits" select elem;

                if (qry.Count<XElement>() == 1)
                {
                    qry.First<XElement>().SetAttributeValue("Count", stat.Visits);
                }

                var qry2 = from elem in root.Elements("Stat") where elem.Attribute("id").Value == "Hits" select elem;

                if (qry2.Count<XElement>() == 1)
                {
                    qry2.First<XElement>().SetAttributeValue("Count", stat.Hits);
                }

                DateTime now = LocalTime.GetCurrentTime(SettingsData.TimeZoneInfo);

                string todayKey = Stat.GetFeedKey(now);
                string yesKey = Stat.GetFeedKey(now.AddDays(-1));
                string twoKey = Stat.GetFeedKey(now.AddDays(-2));

                foreach (KeyValuePair<string, int> kvp2 in stat.Feeds)
                {
                    if (kvp2.Key == todayKey || kvp2.Key == yesKey || kvp2.Key == twoKey)
                    {
                        XElement statElem = new XElement(
                            "Stat", new XAttribute("id", kvp2.Key), new XAttribute("Count", kvp2.Value));

                        root.Add(statElem);
                    }
                }

                BlogData blogData = new BlogData();
                foreach (KeyValuePair<string, PostInfo> kvp in stat.PageVisits)
                {
                    blogData.ChangeViews(kvp.Key, kvp.Value.Views);
                }
            }
            catch (Exception ex)
            {
                // Logger.Log(XML_FORMAT_ERROR, ex);
                throw new ApplicationException(XML_FORMAT_ERROR, ex);
            }

            root.Save(stat.Path);
        }

        #endregion
    }
}