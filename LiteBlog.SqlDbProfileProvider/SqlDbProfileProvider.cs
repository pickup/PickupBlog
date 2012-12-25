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

namespace LiteBlog.SqlDbProfileProvider
{
    public class SqlDbProfileProvider : ProfileProvider
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

        private UserDbContext dbContext = new UserDbContext();
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
        public override int DeleteInactiveProfiles(ProfileAuthenticationOption authenticationOption, DateTime userInactiveSinceDate)
        {
            var users = dbContext.UserSet.Where(u => u.LastActivityTime < userInactiveSinceDate);
            foreach (var user in users)
            {
                dbContext.UserSet.Remove(user);
            }
            return dbContext.SaveChanges();
        }

        public override int DeleteProfiles(string[] usernames)
        {
            var users = dbContext.UserSet.Where(u => usernames.Contains(u.Name));
            foreach (var user in users)
            {
                dbContext.UserSet.Remove(user);
            }
            return dbContext.SaveChanges();
        }

        public override int DeleteProfiles(ProfileInfoCollection profiles)
        {
            List<string> userNames = new List<string>();
            foreach (ProfileInfo profile in profiles)
            {
                userNames.Add(profile.UserName);
            }

            return this.DeleteProfiles(userNames.Distinct<string>().ToArray<string>());
        }

        public override ProfileInfoCollection FindInactiveProfilesByUserName(
            ProfileAuthenticationOption authenticationOption, string usernameToMatch, 
            DateTime userInactiveSinceDate, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override ProfileInfoCollection FindProfilesByUserName(
            ProfileAuthenticationOption authenticationOption, string usernameToMatch, 
            int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override ProfileInfoCollection GetAllInactiveProfiles(ProfileAuthenticationOption authenticationOption, DateTime userInactiveSinceDate, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override ProfileInfoCollection GetAllProfiles(ProfileAuthenticationOption authenticationOption, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override int GetNumberOfInactiveProfiles(ProfileAuthenticationOption authenticationOption, DateTime userInactiveSinceDate)
        {
            throw new NotImplementedException();
        }

        public override SettingsPropertyValueCollection GetPropertyValues(SettingsContext context, SettingsPropertyCollection collection)
        {
            throw new NotImplementedException();
        }

        public override void SetPropertyValues(SettingsContext context, SettingsPropertyValueCollection collection)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
