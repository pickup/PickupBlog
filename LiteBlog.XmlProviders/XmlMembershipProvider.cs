// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XmlMembershipProvider.cs" company="LiteBlog">
//   Copyright (c) 2012, LiteBlog. All Rights Reserved.
// </copyright>
// <summary>
//   The xml membership provider.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace LiteBlog.XmlProviders
{
    using System;
    using System.Collections.Specialized;
    using System.Configuration.Provider;
    using System.Globalization;
    using System.Web;
    using System.Web.Security;
    using System.Xml;

    using LiteBlog.Common;

    // using MvcLiteBlog.BlogEngine;

    /// <summary>
    /// The xml membership provider.
    /// </summary>
    public class XmlMembershipProvider : MembershipProvider
    {
        #region Constants

        /// <summary>
        /// The dat e_ format.
        /// </summary>
        private const string DATE_FORMAT = "dd/MM/yyyy hh:mm tt";

        #endregion

        #region Fields

        /// <summary>
        /// The _app name.
        /// </summary>
        private string _appName = string.Empty;

        /// <summary>
        /// The _doc.
        /// </summary>
        private XmlDocument _doc = new XmlDocument();

        /// <summary>
        /// The _name.
        /// </summary>
        private string _name = string.Empty;

        /// <summary>
        /// The _path.
        /// </summary>
        private string _path = string.Empty;

        /// <summary>
        /// The _tzi.
        /// </summary>
        private TimeZoneInfo _tzi;

        /// <summary>
        /// The first load
        /// </summary>
        private bool firstLoad = true;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the application name.
        /// </summary>
        public override string ApplicationName
        {
            get
            {
                return this._appName;
            }

            set
            {
                this._appName = value;
            }
        }

        /// <summary>
        /// Gets a value indicating whether enable password reset.
        /// </summary>
        public override bool EnablePasswordReset
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Gets a value indicating whether enable password retrieval.
        /// </summary>
        public override bool EnablePasswordRetrieval
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Gets the max invalid password attempts.
        /// </summary>
        public override int MaxInvalidPasswordAttempts
        {
            get
            {
                return 5;
            }
        }

        /// <summary>
        /// Gets the min required non alphanumeric characters.
        /// </summary>
        public override int MinRequiredNonAlphanumericCharacters
        {
            get
            {
                return 0;
            }
        }

        /// <summary>
        /// Gets the min required password length.
        /// </summary>
        public override int MinRequiredPasswordLength
        {
            get
            {
                return 0;
            }
        }

        /// <summary>
        /// Max number of attempts within these minutes before locking out
        /// </summary>
        public override int PasswordAttemptWindow
        {
            get
            {
                return 15;
            }
        }

        /// <summary>
        /// Gets the password format.
        /// </summary>
        public override MembershipPasswordFormat PasswordFormat
        {
            get
            {
                return MembershipPasswordFormat.Clear;
            }
        }

        /// <summary>
        /// Gets the password strength regular expression.
        /// </summary>
        public override string PasswordStrengthRegularExpression
        {
            get
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Gets a value indicating whether requires question and answer.
        /// </summary>
        public override bool RequiresQuestionAndAnswer
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Gets a value indicating whether requires unique email.
        /// </summary>
        public override bool RequiresUniqueEmail
        {
            get
            {
                return true;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The change password.
        /// </summary>
        /// <param name="username">
        /// The username.
        /// </param>
        /// <param name="oldPassword">
        /// The old password.
        /// </param>
        /// <param name="newPassword">
        /// The new password.
        /// </param>
        /// <returns>
        /// The System.Boolean.
        /// </returns>
        /// <exception cref="Exception">
        /// General exception
        /// </exception>
        /// <exception cref="MembershipPasswordException">
        /// Membership exception
        /// </exception>
        public override bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            ValidatePasswordEventArgs args = new ValidatePasswordEventArgs(username, newPassword, false);
            this.OnValidatingPassword(args);
            if (args.Cancel)
            {
                if (args.FailureInformation != null)
                {
                    throw args.FailureInformation;
                }
                else
                {
                    throw new MembershipPasswordException("Invalid password format");
                }
            }

            lock (this)
            {
                string xpath = @"/Users/User[@Name=""{0}""]";
                XmlNode userNode = this._doc.SelectSingleNode(string.Format(xpath, username));
                if (userNode == null)
                {
                    return false;
                }

                XmlNode pwdNode = userNode.SelectSingleNode("Password");
                string password = this.DecryptPassword(pwdNode.InnerText);
                if (oldPassword != password)
                {
                    return false;
                }

                pwdNode.InnerText = this.EncryptPassword(newPassword);

                userNode.SelectSingleNode("LastActivityTime").InnerText =
                    LocalTime.GetCurrentTime(this._tzi).ToString(DATE_FORMAT, CultureInfo.InvariantCulture);
                userNode.SelectSingleNode("LastPasswordTime").InnerText =
                    LocalTime.GetCurrentTime(this._tzi).ToString(DATE_FORMAT, CultureInfo.InvariantCulture);

                this._doc.Save(this._path);
            }

            return true;
        }

        /// <summary>
        /// The change password question and answer.
        /// </summary>
        /// <param name="username">
        /// The username.
        /// </param>
        /// <param name="password">
        /// The password.
        /// </param>
        /// <param name="newPasswordQuestion">
        /// The new password question.
        /// </param>
        /// <param name="newPasswordAnswer">
        /// The new password answer.
        /// </param>
        /// <returns>
        /// The System.Boolean.
        /// </returns>
        public override bool ChangePasswordQuestionAndAnswer(
            string username, string password, string newPasswordQuestion, string newPasswordAnswer)
        {
            return false;
        }

        /// <summary>
        /// The create user.
        /// </summary>
        /// <param name="username">
        /// The username.
        /// </param>
        /// <param name="password">
        /// The password.
        /// </param>
        /// <param name="email">
        /// The email.
        /// </param>
        /// <param name="passwordQuestion">
        /// The password question.
        /// </param>
        /// <param name="passwordAnswer">
        /// The password answer.
        /// </param>
        /// <param name="isApproved">
        /// The is approved.
        /// </param>
        /// <param name="providerUserKey">
        /// The provider user key.
        /// </param>
        /// <param name="status">
        /// The status.
        /// </param>
        /// <returns>
        /// The System.Web.Security.MembershipUser.
        /// </returns>
        public override MembershipUser CreateUser(
            string username, 
            string password, 
            string email, 
            string passwordQuestion, 
            string passwordAnswer, 
            bool isApproved, 
            object providerUserKey, 
            out MembershipCreateStatus status)
        {
            MembershipUser user = null;
            MembershipUser user2 = null;

            ValidatePasswordEventArgs args = new ValidatePasswordEventArgs(username, password, true);
            this.OnValidatingPassword(args);
            if (args.Cancel)
            {
                status = MembershipCreateStatus.InvalidPassword;
                return user;
            }

            if (providerUserKey == null)
            {
                providerUserKey = Guid.NewGuid();
            }
            else if (providerUserKey.GetType() != typeof(Guid))
            {
                status = MembershipCreateStatus.InvalidProviderUserKey;
                return user;
            }
            else
            {
                user2 = this.GetUser(providerUserKey, false);
                if (user2 != null)
                {
                    status = MembershipCreateStatus.DuplicateProviderUserKey;
                    return user;
                }
            }

            user2 = this.GetUser(username, false);
            if (user2 != null)
            {
                status = MembershipCreateStatus.DuplicateUserName;
                return user;
            }

            if (this.RequiresUniqueEmail)
            {
                if (this.GetUserNameByEmail(email) != string.Empty)
                {
                    status = MembershipCreateStatus.DuplicateEmail;
                    return user;
                }
            }

            lock (this)
            {
                XmlElement userElem = this._doc.CreateElement("User");
                XmlAttribute keyAttr = this._doc.CreateAttribute("PKey");
                XmlAttribute nameAttr = this._doc.CreateAttribute("Name");
                XmlAttribute emailAttr = this._doc.CreateAttribute("Email");

                keyAttr.Value = ((Guid)providerUserKey).ToString();
                nameAttr.Value = username;
                emailAttr.Value = email;

                userElem.Attributes.Append(keyAttr);
                userElem.Attributes.Append(nameAttr);
                userElem.Attributes.Append(emailAttr);

                XmlElement pwdElem = this._doc.CreateElement("Password");
                pwdElem.InnerText = this.EncryptPassword(password);
                userElem.AppendChild(pwdElem);

                XmlElement lockElem = this._doc.CreateElement("Locked");
                lockElem.InnerText = false.ToString();
                userElem.AppendChild(lockElem);

                XmlElement createdElem = this._doc.CreateElement("CreatedTime");
                createdElem.InnerText = LocalTime.GetCurrentTime(this._tzi).ToString(
                    DATE_FORMAT, CultureInfo.InvariantCulture);
                userElem.AppendChild(createdElem);

                XmlElement loginElem = this._doc.CreateElement("LastLoginTime");
                loginElem.InnerText = LocalTime.GetCurrentTime(this._tzi).ToString(
                    DATE_FORMAT, CultureInfo.InvariantCulture);
                userElem.AppendChild(loginElem);

                XmlElement actElem = this._doc.CreateElement("LastActivityTime");
                actElem.InnerText = LocalTime.GetCurrentTime(this._tzi).ToString(
                    DATE_FORMAT, CultureInfo.InvariantCulture);
                userElem.AppendChild(actElem);

                XmlElement lastPwdElem = this._doc.CreateElement("LastPasswordTime");
                lastPwdElem.InnerText = LocalTime.GetCurrentTime(this._tzi).ToString(
                    DATE_FORMAT, CultureInfo.InvariantCulture);
                userElem.AppendChild(lastPwdElem);

                XmlElement lastLockElem = this._doc.CreateElement("LastLockedTime");
                lastLockElem.InnerText = LocalTime.GetCurrentTime(this._tzi).ToString(
                    DATE_FORMAT, CultureInfo.InvariantCulture);
                userElem.AppendChild(lastLockElem);

                this._doc.DocumentElement.AppendChild(userElem);
                this._doc.Save(this._path);
            }

            status = MembershipCreateStatus.Success;
            user = new MembershipUser(
                this._name, 
                username, 
                providerUserKey, 
                email, 
                string.Empty, 
                string.Empty, 
                true, 
                false, 
                LocalTime.GetCurrentTime(this._tzi), 
                LocalTime.GetCurrentTime(this._tzi), 
                LocalTime.GetCurrentTime(this._tzi), 
                LocalTime.GetCurrentTime(this._tzi), 
                LocalTime.GetCurrentTime(this._tzi));
            return user;
        }

        /// <summary>
        /// The delete user.
        /// </summary>
        /// <param name="username">
        /// The username.
        /// </param>
        /// <param name="deleteAllRelatedData">
        /// The delete all related data.
        /// </param>
        /// <returns>
        /// The System.Boolean.
        /// </returns>
        /// <exception cref="ProviderException">
        /// Provider exception
        /// </exception>
        public override bool DeleteUser(string username, bool deleteAllRelatedData)
        {
            string xpath = @"/Users/User[@Name=""{0}""]";
            lock (this)
            {
                XmlNode userNode = this._doc.SelectSingleNode(string.Format(xpath, username));
                if (userNode == null)
                {
                    throw new ProviderException("User not found");
                }

                this._doc.DocumentElement.RemoveChild(userNode);
                this._doc.Save(this._path);
            }

            return true;
        }

        /// <summary>
        /// The find users by email.
        /// </summary>
        /// <param name="emailToMatch">
        /// The email to match.
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
        /// The System.Web.Security.MembershipUserCollection.
        /// </returns>
        public override MembershipUserCollection FindUsersByEmail(
            string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            string xpath = @"/Users/User[Contains(@Email,""{0}"")]";
            xpath = string.Format(xpath, emailToMatch);
            XmlNodeList userList = this._doc.SelectNodes(xpath);
            return this.GetUsers(userList, pageIndex, pageSize, out totalRecords);
        }

        /// <summary>
        /// Only one user
        /// </summary>
        /// <param name="usernameToMatch">
        /// The username To Match.
        /// </param>
        /// <param name="pageIndex">
        /// The page Index.
        /// </param>
        /// <param name="pageSize">
        /// The page Size.
        /// </param>
        /// <param name="totalRecords">
        /// The total Records.
        /// </param>
        /// <returns>
        /// The System.Web.Security.MembershipUserCollection.
        /// </returns>
        public override MembershipUserCollection FindUsersByName(
            string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            string xpath = @"/Users/User[Contains(@Name,""{0}"")]";
            xpath = string.Format(xpath, usernameToMatch);
            XmlNodeList userList = this._doc.SelectNodes(xpath);
            return this.GetUsers(userList, pageIndex, pageSize, out totalRecords);
        }

        /// <summary>
        /// The get all users.
        /// </summary>
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
        /// The System.Web.Security.MembershipUserCollection.
        /// </returns>
        public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            XmlNodeList userList = this._doc.SelectNodes("/Users/User");
            return this.GetUsers(userList, pageIndex, pageSize, out totalRecords);
        }

        /// <summary>
        /// The get number of users online.
        /// </summary>
        /// <returns>
        /// The System.Int32.
        /// </returns>
        public override int GetNumberOfUsersOnline()
        {
            DateTime onlineTime = LocalTime.GetCurrentTime(this._tzi).AddMinutes(-Membership.UserIsOnlineTimeWindow);
            XmlNodeList userList = this._doc.SelectNodes("/Users/User");
            int onlineCount = 0;
            foreach (XmlNode user in userList)
            {
                DateTime lastActTime = DateTime.MinValue;
                DateTime.TryParseExact(
                    user.SelectSingleNode("LastActivityTime").InnerText, 
                    DATE_FORMAT, 
                    CultureInfo.InvariantCulture, 
                    DateTimeStyles.None, 
                    out lastActTime);
                if (lastActTime > onlineTime)
                {
                    onlineCount++;
                }
            }

            return onlineCount;
        }

        /// <summary>
        /// The get password.
        /// </summary>
        /// <param name="username">
        /// The username.
        /// </param>
        /// <param name="answer">
        /// The answer.
        /// </param>
        /// <returns>
        /// The System.String.
        /// </returns>
        public override string GetPassword(string username, string answer)
        {
            string xpath = @"/Users/User[@Name=""{0}""]";
            XmlNode userNode = this._doc.SelectSingleNode(string.Format(xpath, username));
            if (userNode == null)
            {
                return string.Empty;
            }

            XmlNode pwdNode = userNode.SelectSingleNode("Password");
            return this.DecryptPassword(pwdNode.InnerText);
        }

        /// <summary>
        /// The get user.
        /// </summary>
        /// <param name="providerUserKey">
        /// The provider user key.
        /// </param>
        /// <param name="userIsOnline">
        /// The user is online.
        /// </param>
        /// <returns>
        /// The System.Web.Security.MembershipUser.
        /// </returns>
        public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            string xpath = @"/Users/User[@PKey=""{0}""]";
            xpath = string.Format(xpath, providerUserKey.ToString());
            XmlNode userNode = this._doc.SelectSingleNode(xpath);
            MembershipUser user = this.GetUserFromUserNode(userNode);
            if (userIsOnline)
            {
                this.SetLastActivity(user.UserName);
            }

            return user;
        }

        /// <summary>
        /// The get user.
        /// </summary>
        /// <param name="username">
        /// The username.
        /// </param>
        /// <param name="userIsOnline">
        /// The user is online.
        /// </param>
        /// <returns>
        /// The System.Web.Security.MembershipUser.
        /// </returns>
        public override MembershipUser GetUser(string username, bool userIsOnline)
        {
            string xpath = @"/Users/User[@Name=""{0}""]";
            xpath = string.Format(xpath, username);
            XmlNode userNode = this._doc.SelectSingleNode(xpath);
            MembershipUser user = this.GetUserFromUserNode(userNode);
            if (userIsOnline)
            {
                this.SetLastActivity(user.UserName);
            }

            return user;
        }

        /// <summary>
        /// The get user name by email.
        /// </summary>
        /// <param name="email">
        /// The email.
        /// </param>
        /// <returns>
        /// The System.String.
        /// </returns>
        public override string GetUserNameByEmail(string email)
        {
            string xpath = @"/Users/User[@Email=""{0}""]";
            xpath = string.Format(xpath, email);
            XmlNode userNode = this._doc.SelectSingleNode(xpath);
            if (userNode == null)
            {
                return string.Empty;
            }

            return userNode.Attributes["Name"].Value;
        }

        /// <summary>
        /// One of the important methods to implement
        /// Initializes XmlDocument with users data
        /// </summary>
        /// <param name="name">
        /// Provider name - 
        /// </param>
        /// <param name="config">
        /// The config
        /// </param>
        public override void Initialize(string name, NameValueCollection config)
        {
            if (string.IsNullOrEmpty(name))
            {
                name = "Xml Membership Provider";
            }

            if (this.firstLoad)
            {
                base.Initialize(name, config);
            }

            this._name = name;

            //this._path = @"C:\Users\zdy\Documents\GitHub\PickupBlog\MvcLiteBlog\App_Data\User.xml"; //HttpContext.Current.Server.MapPath("~/App_data/User.xml");
            this._path = string.Format("{0}User.xml", config["DataPath"]);

            try
            {
                this._doc.Load(this._path);
                this._appName = config["AppName"];
                this._tzi = TimeZoneInfo.FindSystemTimeZoneById(config["Timezone"]);
            }
            catch (Exception ex)
            {
                if (!this.firstLoad)
                {
                    Logger.Log("Membership data not loaded");
                    throw new ProviderException("Membership data not loaded", ex);
                }
                else
                {
                    this.firstLoad = false;
                }
            }

            // this._appName = HttpContext.Current.Request.ApplicationPath.Replace("/", string.Empty);

            // Ignoring all the config values and supply with defaults
            // Question / Answer, Password reset / retrieval, Password related features are all disabled.
        }

        /// <summary>
        /// The reset password.
        /// </summary>
        /// <param name="username">
        /// The username.
        /// </param>
        /// <param name="answer">
        /// The answer.
        /// </param>
        /// <returns>
        /// The System.String.
        /// </returns>
        public override string ResetPassword(string username, string answer)
        {
            return string.Empty;
        }

        /// <summary>
        /// The unlock user.
        /// </summary>
        /// <param name="userName">
        /// The user name.
        /// </param>
        /// <returns>
        /// The System.Boolean.
        /// </returns>
        /// <exception cref="ProviderException">
        /// Provider exception
        /// </exception>
        public override bool UnlockUser(string userName)
        {
            string xpath = @"/Users/User[@Name=""{0}""]";
            lock (this)
            {
                XmlNode userNode = this._doc.SelectSingleNode(string.Format(xpath, userName));
                if (userNode == null)
                {
                    throw new ProviderException("User not found");
                }

                userNode.SelectSingleNode("Locked").InnerText = false.ToString();
                userNode.SelectSingleNode("LastLockedTime").InnerText =
                    LocalTime.GetCurrentTime(this._tzi).ToString(DATE_FORMAT, CultureInfo.InvariantCulture);

                this._doc.Save(this._path);
            }

            return true;
        }

        /// <summary>
        /// Update email
        /// </summary>
        /// <param name="user">
        /// The user
        /// </param>
        public override void UpdateUser(MembershipUser user)
        {
            lock (this)
            {
                string xpath = @"/Users/User[@Name=""{0}""]";
                XmlNode userNode = this._doc.SelectSingleNode(string.Format(xpath, user.UserName));
                if (userNode == null)
                {
                    throw new ProviderException("User not found");
                }

                userNode.Attributes["Email"].Value = user.Email;
                this._doc.Save(this._path);
            }
        }

        /// <summary>
        /// The validate user.
        /// </summary>
        /// <param name="username">
        /// The username.
        /// </param>
        /// <param name="password">
        /// The password.
        /// </param>
        /// <returns>
        /// The System.Boolean.
        /// </returns>
        public override bool ValidateUser(string username, string password)
        {
            lock (this)
            {
                string xpath = @"/Users/User[@Name=""{0}""]";
                XmlNode userNode = this._doc.SelectSingleNode(string.Format(xpath, username));

                if (userNode == null)
                {
                    return false;
                }

                int attempts = -1; // HttpContext.Current is null
                if (HttpContext.Current != null)
                {
                    if (HttpContext.Current.Session["Attempts"] == null)
                    {
                        HttpContext.Current.Session["Attempts"] = 0;
                    }

                    attempts = (int)HttpContext.Current.Session["Attempts"];
                }

                string password2 = this.DecryptPassword(userNode.SelectSingleNode("Password").InnerText);
                bool locked = true;
                bool.TryParse(userNode.SelectSingleNode("Locked").InnerText, out locked);
                if (locked)
                {
                    return false;
                }

                if (password2 != password)
                {
                    if (attempts != -1)
                    {
                        attempts++;
                        HttpContext.Current.Session["Attempts"] = attempts;
                        if (attempts > this.MaxInvalidPasswordAttempts)
                        {
                            this.LockUser(username);
                        }
                    }

                    return false;
                }

                if (attempts != -1)
                {
                    HttpContext.Current.Session["Attempts"] = 0;
                }

                userNode.SelectSingleNode("LastActivityTime").InnerText =
                    LocalTime.GetCurrentTime(this._tzi).ToString(DATE_FORMAT, CultureInfo.InvariantCulture);
                this._doc.Save(this._path);
            }

            return true;
        }

        #endregion

        #region Methods

        /// <summary>
        /// The decrypt password.
        /// </summary>
        /// <param name="password">
        /// The password.
        /// </param>
        /// <returns>
        /// The System.String.
        /// </returns>
        private string DecryptPassword(string password)
        {
            // encrypt / decrypt using own algorithm
            return password;
        }

        /// <summary>
        /// To enable encryption, machine key should be defined
        /// </summary>
        /// <param name="password">
        /// The password.
        /// </param>
        /// <returns>
        /// The System.String.
        /// </returns>
        private string EncryptPassword(string password)
        {
            // encrypt / decrypt using your own algorithm
            return password;
        }

        /// <summary>
        /// The get user from user node.
        /// </summary>
        /// <param name="userNode">
        /// The user node.
        /// </param>
        /// <returns>
        /// The System.Web.Security.MembershipUser.
        /// </returns>
        /// <exception cref="ProviderException">
        /// Provider exception
        /// </exception>
        private MembershipUser GetUserFromUserNode(XmlNode userNode)
        {
            MembershipUser user = null;
            if (userNode == null)
            {
                return user;
            }

            try
            {
                Guid pKey;
                Guid.TryParse(userNode.Attributes["PKey"].Value, out pKey);

                string userName = userNode.Attributes["Name"].Value;
                string email = userNode.Attributes["Email"].Value;
                bool isLocked = false;
                bool.TryParse(userNode.SelectSingleNode("Locked").InnerText, out isLocked);
                DateTime creationTime, lastLoginTime, lastActivityTime, lastPwdTime, lastLockedTime;
                DateTime.TryParseExact(
                    userNode.SelectSingleNode("CreatedTime").InnerText, 
                    DATE_FORMAT, 
                    CultureInfo.InvariantCulture, 
                    DateTimeStyles.None, 
                    out creationTime);
                DateTime.TryParseExact(
                    userNode.SelectSingleNode("LastLoginTime").InnerText, 
                    DATE_FORMAT, 
                    CultureInfo.InvariantCulture, 
                    DateTimeStyles.None, 
                    out lastLoginTime);
                DateTime.TryParseExact(
                    userNode.SelectSingleNode("LastActivityTime").InnerText, 
                    DATE_FORMAT, 
                    CultureInfo.InvariantCulture, 
                    DateTimeStyles.None, 
                    out lastActivityTime);
                DateTime.TryParseExact(
                    userNode.SelectSingleNode("LastPasswordTime").InnerText, 
                    DATE_FORMAT, 
                    CultureInfo.InvariantCulture, 
                    DateTimeStyles.None, 
                    out lastPwdTime);
                DateTime.TryParseExact(
                    userNode.SelectSingleNode("LastLockedTime").InnerText, 
                    DATE_FORMAT, 
                    CultureInfo.InvariantCulture, 
                    DateTimeStyles.None, 
                    out lastLockedTime);

                user = new MembershipUser(
                    this._name, 
                    userName, 
                    pKey, 
                    email, 
                    string.Empty, 
                    string.Empty, 
                    true, 
                    isLocked, 
                    creationTime, 
                    lastLoginTime, 
                    lastActivityTime, 
                    lastPwdTime, 
                    lastLockedTime);
            }
            catch (Exception ex)
            {
                throw new ProviderException("The user repository has data issues", ex);
            }

            return user;
        }

        /// <summary>
        /// Helper method for all the FindUsers, GetAllUsers functions
        /// </summary>
        /// <param name="userList">
        /// The user List.
        /// </param>
        /// <param name="pageIndex">
        /// The page Index.
        /// </param>
        /// <param name="pageSize">
        /// The page Size.
        /// </param>
        /// <param name="totalRecords">
        /// The total Records.
        /// </param>
        /// <returns>
        /// The System.Web.Security.MembershipUserCollection.
        /// </returns>
        private MembershipUserCollection GetUsers(
            XmlNodeList userList, int pageIndex, int pageSize, out int totalRecords)
        {
            if (pageIndex < 0)
            {
                throw new ProviderException("Invalid page params");
            }

            if (pageSize < 1)
            {
                throw new ProviderException("Invalid page size");
            }

            MembershipUserCollection users = new MembershipUserCollection();
            totalRecords = 0;

            if (userList == null)
            {
                return users;
            }

            totalRecords = userList.Count;
            int startIndex = pageIndex * pageSize;
            int endIndex = startIndex + pageSize - 1;

            totalRecords = userList.Count;

            if (totalRecords < (startIndex + 1))
            {
                return users;
            }

            if (endIndex > totalRecords - 1)
            {
                endIndex = totalRecords - 1;
            }

            // Junky code without Linq, optimizing performance if there are large number of users
            // int curIndex = 0;
            // foreach (XmlNode userNode in userList)
            // {
            // if (curIndex < startIndex)
            // continue;

            // MembershipUser user = GetUserFromUserNode(userNode);
            // users.Add(user);

            // curIndex++;
            // if (curIndex > endIndex)
            // break;
            // }
            for (int curIndex = startIndex; curIndex <= endIndex; curIndex++)
            {
                MembershipUser user = this.GetUserFromUserNode(userList[curIndex]);
                users.Add(user);
            }

            return users;
        }

        /// <summary>
        /// The lock user.
        /// </summary>
        /// <param name="userName">
        /// The user name.
        /// </param>
        /// <exception cref="ProviderException">
        /// Provider exception
        /// </exception>
        private void LockUser(string userName)
        {
            string xpath = @"/Users/User[@Name=""{0}""]";
            XmlNode userNode = this._doc.SelectSingleNode(string.Format(xpath, userName));
            if (userNode == null)
            {
                throw new ProviderException("User not found");
            }

            userNode.SelectSingleNode("Locked").InnerText = true.ToString();
            this._doc.Save(this._path);
        }

        /// <summary>
        /// The set last activity.
        /// </summary>
        /// <param name="userName">
        /// The user name.
        /// </param>
        /// <exception cref="ProviderException">
        /// Provider exception
        /// </exception>
        private void SetLastActivity(string userName)
        {
            lock (this)
            {
                string xpath = @"/Users/User[@Name=""{0}""]";
                xpath = string.Format(xpath, userName);
                XmlNode userNode = this._doc.SelectSingleNode(xpath);
                try
                {
                    XmlNode actNode = userNode.SelectSingleNode("LastActivityTime");
                    actNode.InnerText = LocalTime.GetCurrentTime(this._tzi).ToString(
                        DATE_FORMAT, CultureInfo.InvariantCulture);
                }
                catch (Exception ex)
                {
                    throw new ProviderException("XML is not in right format", ex);
                }

                this._doc.Save(this._path);
            }
        }

        #endregion
    }
}