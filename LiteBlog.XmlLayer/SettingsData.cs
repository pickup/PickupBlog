// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SettingsData.cs" company="LiteBlog">
//   Copyright (c) 2012, LiteBlog. All Rights Reserved.
// </copyright>
// <summary>
//   Class that manages the Settings XML
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace LiteBlog.XmlLayer
{
    using System;
    using System.Xml.Linq;

    using LiteBlog.Common;
    using LiteBlog.Common.Contracts;
    using System.Web;

    /// <summary>
    /// Class that manages the Settings XML
    /// </summary>
    public class SettingsData : ISettingsData
    {
        #region Constants

        /// <summary>
        /// The n o_ fil e_ error.
        /// </summary>
        private const string NO_FILE_ERROR = "Application file could not be found";

        /// <summary>
        /// The xm l_ forma t_ error.
        /// </summary>
        private const string XML_FORMAT_ERROR = "Application file is not in the right format";

        #endregion

        #region Static Fields

        /// <summary>
        /// The _tzi.
        /// </summary>
        private static TimeZoneInfo _tzi = null;

        #endregion

        #region Fields

        /// <summary>
        /// The _path.
        /// </summary>
        private string _path;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SettingsData"/> class.
        /// </summary>
        public SettingsData()
        {
            this._path = DataContext.Path + "Settings.xml";
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
                return DataContext.Path + "Settings.xml";
            }
        }

        /// <summary>
        /// Gets the time zone info.
        /// </summary>
        public static TimeZoneInfo TimeZoneInfo
        {
            get
            {
                if (_tzi == null)
                {
                    _tzi = TimeZoneInfo.FindSystemTimeZoneById(GetTimeZoneInfo());
                }

                return _tzi;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Loads Settings XML into Settings object
        /// Called by Application settings management
        /// Called by RSS Feed Generation component
        /// </summary>
        /// <returns>The Settings</returns>
        public Settings Load()
        {
            Settings app = new Settings();
            XElement root = null;

            try
            {
                var p = this._path;
                root = XElement.Load(p);
            }
            catch (Exception ex)
            {
                Logger.Log(NO_FILE_ERROR, ex);
                throw new ApplicationException(NO_FILE_ERROR, ex);
            }

            try
            {
                app.Name = root.Element("Name").Value;
                app.Url = root.Element("Url").Value;
                app.PostCount = int.Parse(root.Element("PostCount").Value);
                app.CommentModeration = bool.Parse(root.Element("CommentModeration").Value);
                app.Timezone = root.Element("Timezone").Value;
            }
            catch (Exception ex)
            {
                Logger.Log(XML_FORMAT_ERROR, ex);
                throw new ApplicationException(XML_FORMAT_ERROR, ex);
            }

            return app;
        }

        /// <summary>
        /// Saves settings data into XML
        /// Called by Settings management
        /// </summary>
        /// <param name="app">
        /// The Settings
        /// </param>
        public void Save(Settings app)
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

            try
            {
                XElement settings = new XElement(
                    "Settings", 
                    new XElement("Name", app.Name), 
                    new XElement("Url", app.Url), 
                    new XElement("PostCount", app.PostCount), 
                    new XElement("Timezone", app.Timezone), 
                    new XElement("CommentModeration", app.CommentModeration));

                root.ReplaceNodes(settings.Elements());

                root.Save(this._path);
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
        /// The get time zone info.
        /// </summary>
        /// <returns>
        /// The System.String.
        /// </returns>
        /// <exception cref="ApplicationException">
        /// Application exception
        /// </exception>
        private static string GetTimeZoneInfo()
        {
            Settings app = new Settings();
            XElement root = null;

            try
            {
                root = XElement.Load(Path);
            }
            catch (Exception ex)
            {
                Logger.Log(NO_FILE_ERROR, ex);
                throw new ApplicationException(NO_FILE_ERROR, ex);
            }

            try
            {
                return root.Element("Timezone").Value;
            }
            catch (Exception ex)
            {
                Logger.Log(XML_FORMAT_ERROR, ex);
                throw new ApplicationException(XML_FORMAT_ERROR, ex);
            }
        }

        #endregion
    }
}