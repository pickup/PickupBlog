// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataContext.cs" company="LiteBlog">
//   Copyright (c) 2012, LiteBlog. All Rights Reserved.
// </copyright>
// <summary>
//   This class is present in common layer because it is tightly
//   coupled with XmlLayer. This has to change if XmlLayer is replaced
//   with say SQLLayer
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace LiteBlog.XmlLayer
{
    using LiteBlog.Common;
    using LiteBlog.Common.Contracts;

    /// <summary>
    /// This class is present in common layer because it is tightly 
    /// coupled with XmlLayer. This has to change if XmlLayer is replaced
    /// with say SQLLayer
    /// </summary>
    public class DataContext : IDataContext
    {
        #region Static Fields

        /// <summary>
        /// The date time format.
        /// </summary>
        public static string DateTimeFormat = "dd-MMM-yyyy hh:mm tt";

        /// <summary>
        /// The _path.
        /// </summary>
        private static string _path = string.Empty;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DataContext"/> class.
        /// </summary>
        /// <param name="path">
        /// The path.
        /// </param>
        public DataContext(string path)
        {
            _path = path;
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
                return _path;
            }
        }

        /// <summary>
        /// Gets the archive data.
        /// </summary>
        public IArchiveData ArchiveData
        {
            get
            {
                return new ArchiveData();
            }
        }

        /// <summary>
        /// Gets the blog data.
        /// </summary>
        public IBlogData BlogData
        {
            get
            {
                return new BlogData();
            }
        }

        /// <summary>
        /// Gets the category data.
        /// </summary>
        public ICategoryData CategoryData
        {
            get
            {
                return new CategoryData();
            }
        }

        /// <summary>
        /// Gets the comment data.
        /// </summary>
        public ICommentData CommentData
        {
            get
            {
                return new CommentData();
            }
        }

        /// <summary>
        /// Gets the draft data.
        /// </summary>
        public IDraftData DraftData
        {
            get
            {
                return new DraftData();
            }
        }

        /// <summary>
        /// Gets the draft post data.
        /// </summary>
        public IDraftPostData DraftPostData
        {
            get
            {
                return new DraftPostData();
            }
        }

        /// <summary>
        /// Gets the page data
        /// </summary>
        public IPageData PageData
        {
            get
            {
                return new PageData();
            }
        }

        /// <summary>
        /// Gets the post data.
        /// </summary>
        public IPostData PostData
        {
            get
            {
                return new PostData();
            }
        }

        /// <summary>
        /// Gets the service data.
        /// </summary>
        public IServiceData ServiceData
        {
            get
            {
                return new ServiceData();
            }
        }

        /// <summary>
        /// Gets the settings data.
        /// </summary>
        public ISettingsData SettingsData
        {
            get
            {
                return new SettingsData();
            }
        }

        /// <summary>
        /// Gets the stat data.
        /// </summary>
        public IStatData StatData
        {
            get
            {
                return new StatData();
            }
        }

        #endregion
    }
}