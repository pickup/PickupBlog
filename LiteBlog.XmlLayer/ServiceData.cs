// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ServiceData.cs" company="LiteBlog">
//   Copyright (c) 2012, LiteBlog. All Rights Reserved.
// </copyright>
// <summary>
//   A class that manages service nodes in Service XML
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
    /// A class that manages service nodes in Service XML
    /// </summary>
    public class ServiceData : IServiceData
    {
        #region Constants

        /// <summary>
        /// The n o_ fil e_ error.
        /// </summary>
        private const string NO_FILE_ERROR = "Service file could not be found";

        /// <summary>
        /// The xm l_ forma t_ error.
        /// </summary>
        private const string XML_FORMAT_ERROR = "Service file is not in the right format";

        #endregion

        #region Fields

        /// <summary>
        /// The _path.
        /// </summary>
        private string _path;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceData"/> class.
        /// </summary>
        public ServiceData()
        {
            this._path = DataContext.Path + "Service.xml";
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
                return DataContext.Path + "Service.xml";
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Loads list of services
        /// Called by Session Start event
        /// </summary>
        /// <returns>List of service items</returns>
        public List<ServiceItem> Load()
        {
            List<ServiceItem> list = new List<ServiceItem>();
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
                foreach (XElement svcElem in root.Elements("ServiceItem"))
                {
                    ServiceItem item = new ServiceItem();
                    item.ID = svcElem.Attribute("ID").Value;
                    item.Type = svcElem.Attribute("Type").Value;
                    item.Method = svcElem.Attribute("Method").Value;
                    try
                    {
                        item.LastUpdated = DateTime.ParseExact(
                            svcElem.Attribute("LastUpdated").Value, 
                            DataContext.DateTimeFormat, 
                            CultureInfo.InvariantCulture);
                    }
                    catch
                    {
                        item.LastUpdated = DateTime.MinValue;
                    }

                    item.Frequency = int.Parse(svcElem.Attribute("Frequency").Value);
                    item.Path = this._path;
                    list.Add(item);
                }
            }
            catch (Exception ex)
            {
                Logger.Log(XML_FORMAT_ERROR, ex);
                throw new ApplicationException(XML_FORMAT_ERROR, ex);
            }

            return list;
        }

        /// <summary>
        /// Updates last updated time of each of the service
        /// Called by Session start event after running all the service
        /// </summary>
        /// <param name="svcList">
        /// List of ServiceItem
        /// </param>
        public void Save(List<ServiceItem> svcList)
        {
            XElement root = null;

            if (svcList.Count > 0)
            {
                try
                {
                    root = XElement.Load(svcList[0].Path);
                }
                catch (Exception ex)
                {
                    // This is commented because it may not work well in application end
                    // Logger.Log(NO_FILE_ERROR, ex);
                    throw new ApplicationException(NO_FILE_ERROR, ex);
                }

                try
                {
                    foreach (ServiceItem item in svcList)
                    {
                        var qry = from elem in root.Elements("ServiceItem")
                                  where elem.Attribute("ID").Value == item.ID
                                  select elem;

                        if (qry.Count<XElement>() == 1)
                        {
                            XElement svcElem = qry.First<XElement>();
                            svcElem.SetAttributeValue(
                                "LastUpdated", 
                                item.LastUpdated.ToString(DataContext.DateTimeFormat, CultureInfo.InvariantCulture));
                        }
                    }

                    root.Save(svcList[0].Path);
                }
                catch (Exception ex)
                {
                    // Logger.Log(XML_FORMAT_ERROR, ex);
                    throw new ApplicationException(XML_FORMAT_ERROR, ex);
                }
            }
        }

        #endregion
    }
}