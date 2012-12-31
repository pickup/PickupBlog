// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XmlProfileProvider.cs" company="LiteBlog">
//   Copyright (c) 2012, LiteBlog. All Rights Reserved.
// </copyright>
// <summary>
//   The xml profile provider.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace LiteBlog.XmlProviders
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Configuration;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Web.Profile;
    using System.Xml;

    using LiteBlog.Common;
    using System.Web;

    // using MvcLiteBlog.BlogEngine;

    /// <summary>
    /// The xml profile provider.
    /// </summary>
    public class XmlProfileProvider : ProfileProvider
    {
        #region Fields

        /// <summary>
        /// The _tzi.
        /// </summary>
        private TimeZoneInfo _tzi;

        /// <summary>
        /// The app name.
        /// </summary>
        private string appName = string.Empty;

        /// <summary>
        /// The date format.
        /// </summary>
        private string dateFormat = "MM/dd/yyyy hh:mm tt";

        /// <summary>
        /// The doc.
        /// </summary>
        private XmlDocument doc = new XmlDocument();

        /// <summary>
        /// The first load
        /// </summary>
        private bool firstLoad = true;

        /// <summary>
        /// The log.
        /// </summary>
        private bool log = false;

        /// <summary>
        /// The path.
        /// </summary>
        private string path = string.Empty;

        /// <summary>
        /// The path 2.
        /// </summary>
        private string path2 = string.Empty;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the application name.
        /// </summary>
        public override string ApplicationName
        {
            get
            {
                return this.appName;
            }

            set
            {
                this.appName = value;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The delete inactive profiles.
        /// </summary>
        /// <param name="authenticationOption">
        /// The authentication option.
        /// </param>
        /// <param name="userInactiveSinceDate">
        /// The user inactive since date.
        /// </param>
        /// <returns>
        /// The System.Int32.
        /// </returns>
        public override int DeleteInactiveProfiles(
            ProfileAuthenticationOption authenticationOption, DateTime userInactiveSinceDate)
        {
            lock (this)
            {
                ProfileInfoCollection collection = new ProfileInfoCollection();
                XmlNodeList profiles = this.doc.SelectNodes("/Profiles/Profile");
                int inactiveCount = 0;
                List<XmlNode> toRemove = new List<XmlNode>();
                if (profiles != null)
                {
                    foreach (XmlNode profile in profiles)
                    {
                        DateTime actDate = DateTime.MinValue;
                        DateTime.TryParseExact(
                            profile.Attributes["LastActivity"].Value, 
                            this.dateFormat, 
                            CultureInfo.InvariantCulture, 
                            DateTimeStyles.None, 
                            out actDate);
                        if (actDate < userInactiveSinceDate)
                        {
                            toRemove.Add(profile);
                            inactiveCount++;
                        }
                    }

                    foreach (XmlNode removeNode in toRemove)
                    {
                        this.doc.DocumentElement.RemoveChild(removeNode);
                    }
                }

                this.Save();

                return inactiveCount;
            }
        }

        /// <summary>
        /// The delete profiles.
        /// </summary>
        /// <param name="usernames">
        /// The usernames.
        /// </param>
        /// <returns>
        /// The System.Int32.
        /// </returns>
        public override int DeleteProfiles(string[] usernames)
        {
            lock (this)
            {
                int removeIndex = 0;
                foreach (string userName in usernames)
                {
                    string xpath = string.Format(@"/Profiles/Profile[@UserName=""{0}""]", userName);
                    XmlNodeList profiles = this.doc.SelectNodes(xpath);
                    if (profiles != null)
                    {
                        foreach (XmlNode profile in profiles)
                        {
                            this.doc.DocumentElement.RemoveChild(profile);
                            removeIndex++;
                        }
                    }
                }

                if (removeIndex > 0)
                {
                    this.Save();
                }

                return removeIndex;
            }
        }

        /// <summary>
        /// The delete profiles.
        /// </summary>
        /// <param name="profiles">
        /// The profiles.
        /// </param>
        /// <returns>
        /// The System.Int32.
        /// </returns>
        public override int DeleteProfiles(ProfileInfoCollection profiles)
        {
            List<string> userNames = new List<string>();
            foreach (ProfileInfo profile in profiles)
            {
                userNames.Add(profile.UserName);
            }

            return this.DeleteProfiles(userNames.Distinct<string>().ToArray<string>());
        }

        /// <summary>
        /// The find inactive profiles by user name.
        /// </summary>
        /// <param name="authenticationOption">
        /// The authentication option.
        /// </param>
        /// <param name="usernameToMatch">
        /// The username to match.
        /// </param>
        /// <param name="userInactiveSinceDate">
        /// The user inactive since date.
        /// </param>
        /// <param name="pageIndex">
        /// The page index.
        /// </param>
        /// <param name="pageSize">
        /// The page size.
        /// </param>
        /// <param name="totalRecords">
        /// The total records.
        /// </param>
        /// <returns>
        /// The System.Web.Profile.ProfileInfoCollection.
        /// </returns>
        public override ProfileInfoCollection FindInactiveProfilesByUserName(
            ProfileAuthenticationOption authenticationOption, 
            string usernameToMatch, 
            DateTime userInactiveSinceDate, 
            int pageIndex, 
            int pageSize, 
            out int totalRecords)
        {
            ProfileInfoCollection collection = new ProfileInfoCollection();
            ProfileInfoCollection tmpColl = this.FindProfilesByUserName(
                authenticationOption, usernameToMatch, 0, int.MaxValue, out totalRecords);
            foreach (ProfileInfo profile in tmpColl)
            {
                if (profile.LastActivityDate < userInactiveSinceDate)
                {
                    collection.Add(profile);
                }
            }

            totalRecords = collection.Count;
            return new ProfileInfoCollection();
        }

        /// <summary>
        /// The find profiles by user name.
        /// </summary>
        /// <param name="authenticationOption">
        /// The authentication option.
        /// </param>
        /// <param name="usernameToMatch">
        /// The username to match.
        /// </param>
        /// <param name="pageIndex">
        /// The page index.
        /// </param>
        /// <param name="pageSize">
        /// The page size.
        /// </param>
        /// <param name="totalRecords">
        /// The total records.
        /// </param>
        /// <returns>
        /// The System.Web.Profile.ProfileInfoCollection.
        /// </returns>
        public override ProfileInfoCollection FindProfilesByUserName(
            ProfileAuthenticationOption authenticationOption, 
            string usernameToMatch, 
            int pageIndex, 
            int pageSize, 
            out int totalRecords)
        {
            ProfileInfoCollection collection = new ProfileInfoCollection();
            string xpath = string.Format(@"/Profiles/Profile[@UserName=""{0}""]", usernameToMatch);
            XmlNodeList profiles = this.doc.SelectNodes(xpath);
            if (profiles != null)
            {
                totalRecords = profiles.Count;
                int startIndex = pageIndex * pageSize;
                int endIndex = startIndex + pageSize - 1;

                if (totalRecords < (startIndex + 1))
                {
                    return collection;
                }

                if (endIndex > totalRecords - 1)
                {
                    endIndex = totalRecords - 1;
                }

                for (int curIndex = startIndex; curIndex <= endIndex; curIndex++)
                {
                    XmlNode profile = profiles[curIndex];

                    string userName = profile.Attributes["UserName"].Value;
                    DateTime actDate = DateTime.MinValue;
                    DateTime.TryParseExact(
                        profile.Attributes["LastActivity"].Value, 
                        this.dateFormat, 
                        CultureInfo.InvariantCulture, 
                        DateTimeStyles.None, 
                        out actDate);
                    DateTime updDate = DateTime.MinValue;
                    DateTime.TryParseExact(
                        profile.Attributes["LastUpdated"].Value, 
                        this.dateFormat, 
                        CultureInfo.InvariantCulture, 
                        DateTimeStyles.None, 
                        out updDate);

                    ProfileInfo info = new ProfileInfo(userName, true, actDate, updDate, profile.InnerXml.Length);
                    collection.Add(info);
                }
            }

            totalRecords = collection.Count;
            return collection;
        }

        /// <summary>
        /// The get all inactive profiles.
        /// </summary>
        /// <param name="authenticationOption">
        /// The authentication option.
        /// </param>
        /// <param name="userInactiveSinceDate">
        /// The user inactive since date.
        /// </param>
        /// <param name="pageIndex">
        /// The page index.
        /// </param>
        /// <param name="pageSize">
        /// The page size.
        /// </param>
        /// <param name="totalRecords">
        /// The total records.
        /// </param>
        /// <returns>
        /// The System.Web.Profile.ProfileInfoCollection.
        /// </returns>
        public override ProfileInfoCollection GetAllInactiveProfiles(
            ProfileAuthenticationOption authenticationOption, 
            DateTime userInactiveSinceDate, 
            int pageIndex, 
            int pageSize, 
            out int totalRecords)
        {
            ProfileInfoCollection collection = new ProfileInfoCollection();
            ProfileInfoCollection tmpColl = this.GetAllProfiles(authenticationOption, 0, int.MaxValue, out totalRecords);
            foreach (ProfileInfo profile in tmpColl)
            {
                if (profile.LastActivityDate < userInactiveSinceDate)
                {
                    collection.Add(profile);
                }
            }

            totalRecords = collection.Count;
            return collection;
        }

        /// <summary>
        /// The get all profiles.
        /// </summary>
        /// <param name="authenticationOption">
        /// The authentication option.
        /// </param>
        /// <param name="pageIndex">
        /// The page index.
        /// </param>
        /// <param name="pageSize">
        /// The page size.
        /// </param>
        /// <param name="totalRecords">
        /// The total records.
        /// </param>
        /// <returns>
        /// The System.Web.Profile.ProfileInfoCollection.
        /// </returns>
        public override ProfileInfoCollection GetAllProfiles(
            ProfileAuthenticationOption authenticationOption, int pageIndex, int pageSize, out int totalRecords)
        {
            ProfileInfoCollection collection = new ProfileInfoCollection();
            XmlNodeList profiles = this.doc.SelectNodes("/Profiles/Profile");
            if (profiles != null)
            {
                totalRecords = profiles.Count;
                int startIndex = pageIndex * pageSize;
                int endIndex = startIndex + pageSize - 1;

                if (totalRecords < (startIndex + 1))
                {
                    return collection;
                }

                if (endIndex > totalRecords - 1)
                {
                    endIndex = totalRecords - 1;
                }

                for (int curIndex = startIndex; curIndex <= endIndex; curIndex++)
                {
                    XmlNode profile = profiles[curIndex];

                    string userName = profile.Attributes["UserName"].Value;
                    DateTime actDate = DateTime.MinValue;
                    DateTime.TryParseExact(
                        profile.Attributes["LastActivity"].Value, 
                        this.dateFormat, 
                        CultureInfo.InvariantCulture, 
                        DateTimeStyles.None, 
                        out actDate);
                    DateTime updDate = DateTime.MinValue;
                    DateTime.TryParseExact(
                        profile.Attributes["LastUpdated"].Value, 
                        this.dateFormat, 
                        CultureInfo.InvariantCulture, 
                        DateTimeStyles.None, 
                        out updDate);

                    ProfileInfo info = new ProfileInfo(userName, true, actDate, updDate, profile.InnerXml.Length);
                    collection.Add(info);
                }
            }

            totalRecords = collection.Count;
            return collection;
        }

        /// <summary>
        /// Get all users whose activity date is before the inactive date
        /// </summary>
        /// <param name="authenticationOption">
        /// The authentication option
        /// </param>
        /// <param name="userInactiveSinceDate">
        /// The date from which user is inactive
        /// </param>
        /// <returns>
        /// The System.Int32.
        /// </returns>
        public override int GetNumberOfInactiveProfiles(
            ProfileAuthenticationOption authenticationOption, DateTime userInactiveSinceDate)
        {
            ProfileInfoCollection collection = new ProfileInfoCollection();
            XmlNodeList profiles = this.doc.SelectNodes("/Profiles/Profile");
            int inactiveCount = 0;
            if (profiles != null)
            {
                foreach (XmlNode profile in profiles)
                {
                    DateTime actDate = DateTime.MinValue;
                    DateTime.TryParseExact(
                        profile.Attributes["LastActivity"].Value, 
                        this.dateFormat, 
                        CultureInfo.InvariantCulture, 
                        DateTimeStyles.None, 
                        out actDate);
                    if (actDate < userInactiveSinceDate)
                    {
                        inactiveCount++;
                    }
                }
            }

            return inactiveCount;
        }

        /// <summary>
        /// The get property values.
        /// </summary>
        /// <param name="context">
        /// The context.
        /// </param>
        /// <param name="collection">
        /// The collection.
        /// </param>
        /// <returns>
        /// The System.Configuration.SettingsPropertyValueCollection.
        /// </returns>
        public override SettingsPropertyValueCollection GetPropertyValues(
            SettingsContext context, SettingsPropertyCollection collection)
        {
            this.Log("Read");
            lock (this)
            {
                // supports serializing as string, primitive types only
                SettingsPropertyValueCollection values = new SettingsPropertyValueCollection();

                string userName = string.Empty;
                if (context["UserName"] != null)
                {
                    userName = (string)context["UserName"];
                }
                else
                {
                    userName = Guid.NewGuid().ToString();
                }

                this.Log(userName);

                string xpath = string.Format(@"/Profiles/Profile[@UserName=""{0}""]", userName);
                XmlNode profile = this.doc.SelectSingleNode(xpath);
                if (profile == null)
                {
                    profile = this.SetDefaultProfile(userName, collection);
                    this.doc.DocumentElement.AppendChild(profile);
                }

                foreach (SettingsProperty prop in collection)
                {
                    SettingsPropertyValue val = new SettingsPropertyValue(prop);
                    XmlNode propNode = profile.SelectSingleNode(prop.Name);
                    if (propNode == null)
                    {
                        propNode = this.SetDefaultValue(prop);
                        profile.AppendChild(propNode);
                    }

                    switch (prop.PropertyType.ToString())
                    {
                        case "System.String":
                            val.PropertyValue = propNode.InnerText;
                            break;
                        case "System.DateTime":
                            DateTime dt = DateTime.MinValue;
                            DateTime.TryParseExact(
                                propNode.InnerText, 
                                this.dateFormat, 
                                CultureInfo.InvariantCulture, 
                                DateTimeStyles.None, 
                                out dt);
                            val.PropertyValue = dt;
                            break;
                        case "System.Int32":
                            int number = 0;
                            int.TryParse(propNode.InnerText, out number);
                            val.PropertyValue = number;
                            break;
                        default:
                            double number2 = 0;
                            double.TryParse(propNode.InnerText, out number2);
                            break;
                    }

                    values.Add(val);
                }

                profile.Attributes["LastActivity"].Value = LocalTime.GetCurrentTime(this._tzi).ToString(
                    this.dateFormat, CultureInfo.InvariantCulture);
                this.Save();
                this.Log("read complete");
                return values;
            }
        }

        /// <summary>
        /// The initialize.
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <param name="config">
        /// The config.
        /// </param>
        public override void Initialize(string name, NameValueCollection config)
        {
            if (this.firstLoad)
            {
                base.Initialize(name, config);
                this.firstLoad = false;
            }
            else
            {
                string dataPath = HttpRuntime.AppDomainAppPath + config["DataPath"];
                this.path = string.Format("{0}Profile.xml", dataPath);
                this.path2 = string.Format("{0}Log.txt", dataPath);
                this.doc.Load(this.path);
                this._tzi = TimeZoneInfo.FindSystemTimeZoneById(config["Timezone"]);
                this.Log("Initialized");
            }
        }

        /// <summary>
        /// The set property values.
        /// </summary>
        /// <param name="context">
        /// The context.
        /// </param>
        /// <param name="collection">
        /// The collection.
        /// </param>
        public override void SetPropertyValues(SettingsContext context, SettingsPropertyValueCollection collection)
        {
            lock (this)
            {
                this.Log("write");

                string userName = string.Empty;
                if (context["UserName"] != null)
                {
                    userName = (string)context["UserName"];
                }
                else
                {
                    return;
                }

                string xpath = string.Format(@"/Profiles/Profile[@UserName=""{0}""]", userName);
                XmlNode profile = this.doc.SelectSingleNode(xpath);
                if (profile == null)
                {
                    SettingsPropertyCollection propCollection = new SettingsPropertyCollection();
                    foreach (SettingsPropertyValue val in collection)
                    {
                        propCollection.Add(new SettingsProperty(val.Property));
                    }

                    profile = this.SetDefaultProfile(userName, propCollection);
                    this.doc.DocumentElement.AppendChild(profile);
                }

                foreach (SettingsPropertyValue val in collection)
                {
                    XmlNode propElem = profile.SelectSingleNode(val.Name);
                    if (propElem == null)
                    {
                        propElem = this.SetDefaultValue(val.Property);
                    }

                    switch (val.Property.PropertyType.ToString())
                    {
                        case "System.DateTime":
                            propElem.InnerText = ((DateTime)val.PropertyValue).ToString(this.dateFormat, null);
                            break;
                        default:
                            propElem.InnerText = val.PropertyValue.ToString();
                            break;
                    }
                }

                profile.Attributes["LastActivity"].Value = LocalTime.GetCurrentTime(this._tzi).ToString(
                    this.dateFormat, CultureInfo.InvariantCulture);
                profile.Attributes["LastUpdated"].Value = LocalTime.GetCurrentTime(this._tzi).ToString(
                    this.dateFormat, CultureInfo.InvariantCulture);
                this.Save();
                this.Log("write complete");
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// The log.
        /// </summary>
        /// <param name="text">
        /// The text.
        /// </param>
        private void Log(string text)
        {
            if (this.log)
            {
                using (FileStream fs = File.Open(this.path2, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                {
                    using (StreamWriter sw = new StreamWriter(fs))
                    {
                        sw.WriteLine(text);
                        sw.Flush();
                    }
                }
            }
        }

        /// <summary>
        /// The save.
        /// </summary>
        private void Save()
        {
            try
            {
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Indent = true;
                XmlWriter writer = XmlWriter.Create(this.path, settings);
                this.doc.Save(writer);
                writer.Close();
            }
            catch (Exception ex)
            {
                this.Log(ex.Message);
            }
        }

        /// <summary>
        /// The set default profile.
        /// </summary>
        /// <param name="userName">
        /// The user name.
        /// </param>
        /// <param name="collection">
        /// The collection.
        /// </param>
        /// <returns>
        /// The System.Xml.XmlNode.
        /// </returns>
        private XmlNode SetDefaultProfile(string userName, SettingsPropertyCollection collection)
        {
            XmlElement profile = this.doc.CreateElement("Profile");
            XmlAttribute nameAttr = this.doc.CreateAttribute("UserName");
            XmlAttribute actAttr = this.doc.CreateAttribute("LastActivity");
            XmlAttribute updAttr = this.doc.CreateAttribute("LastUpdated");

            nameAttr.Value = userName;
            actAttr.Value = LocalTime.GetCurrentTime(this._tzi).ToString(this.dateFormat, CultureInfo.InvariantCulture);
            updAttr.Value = LocalTime.GetCurrentTime(this._tzi).ToString(this.dateFormat, CultureInfo.InvariantCulture);

            profile.Attributes.Append(nameAttr);
            profile.Attributes.Append(actAttr);
            profile.Attributes.Append(updAttr);

            foreach (SettingsProperty prop in collection)
            {
                XmlNode propElem = this.SetDefaultValue(prop);
                profile.AppendChild(propElem);
            }

            return profile;
        }

        /// <summary>
        /// The set default value.
        /// </summary>
        /// <param name="prop">
        /// The prop.
        /// </param>
        /// <returns>
        /// The System.Xml.XmlNode.
        /// </returns>
        private XmlNode SetDefaultValue(SettingsProperty prop)
        {
            XmlElement propElem = this.doc.CreateElement(prop.Name);
            switch (prop.PropertyType.ToString())
            {
                case "System.DateTime":
                    propElem.InnerText = DateTime.MinValue.ToString(this.dateFormat, CultureInfo.InvariantCulture);
                    break;
                case "System.String":
                    propElem.InnerText = string.Empty;
                    break;
                default:
                    propElem.InnerText = "0";
                    break;
            }

            return propElem;
        }

        #endregion
    }
}