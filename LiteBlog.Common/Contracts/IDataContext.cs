// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IDataContext.cs" company="LiteBlog">
//   Copyright (c) 2012, LiteBlog. All Rights Reserved.
// </copyright>
// <summary>
//   The cache type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace LiteBlog.Common.Contracts
{
    /// <summary>
    /// The DataContext interface.
    /// </summary>
    public interface IDataContext
    {
        #region Public Properties

        /// <summary>
        /// Gets the archive data.
        /// </summary>
        IArchiveData ArchiveData { get; }

        /// <summary>
        /// Gets the blog data.
        /// </summary>
        IBlogData BlogData { get; }

        /// <summary>
        /// Gets the category data.
        /// </summary>
        ICategoryData CategoryData { get; }

        /// <summary>
        /// Gets the comment data.
        /// </summary>
        ICommentData CommentData { get; }

        /// <summary>
        /// Gets the draft data.
        /// </summary>
        IDraftData DraftData { get; }

        /// <summary>
        /// Gets the draft post data.
        /// </summary>
        IDraftPostData DraftPostData { get; }

        /// <summary>
        /// Gets the page data
        /// </summary>
        IPageData PageData { get; }

        /// <summary>
        /// Gets the post data.
        /// </summary>
        IPostData PostData { get; }

        /// <summary>
        /// Gets the service data.
        /// </summary>
        IServiceData ServiceData { get; }

        /// <summary>
        /// Gets the settings data.
        /// </summary>
        ISettingsData SettingsData { get; }

        /// <summary>
        /// Gets the stat data.
        /// </summary>
        IStatData StatData { get; }

        #endregion
    }
}