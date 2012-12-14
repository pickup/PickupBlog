// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CacheContext.cs" company="LiteBlog">
//   Copyright (c) 2012, LiteBlog. All Rights Reserved.
// </copyright>
// <summary>
//   This class is used by CacheHelper object to create any dependencies
//   Only datalayer is aware of dependencies
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace LiteBlog.XmlLayer
{
    using System.Web.Caching;

    using LiteBlog.Common;
    using LiteBlog.Common.Contracts;

    /// <summary>
    /// This class is used by CacheHelper object to create any dependencies
    /// Only datalayer is aware of dependencies
    /// </summary>
    public class CacheContext : ICacheContext
    {
        #region Public Methods and Operators

        /// <summary>
        /// The get dependency.
        /// </summary>
        /// <param name="type">
        /// The type.
        /// </param>
        /// <returns>
        /// The System.Web.Caching.CacheDependency.
        /// </returns>
        public CacheDependency GetDependency(CacheType type)
        {
            return this.GetDependency(type, string.Empty);
        }

        /// <summary>
        /// The get dependency.
        /// </summary>
        /// <param name="type">
        /// The type.
        /// </param>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The System.Web.Caching.CacheDependency.
        /// </returns>
        public CacheDependency GetDependency(CacheType type, string id)
        {
            switch (type)
            {
                case CacheType.Blog:
                    return new CacheDependency(BlogData.Path);
                case CacheType.Rss:
                    return new CacheDependency(BlogData.Path);
                case CacheType.Categories:
                    return new CacheDependency(CategoryData.Path);
                case CacheType.Settings:
                    return new CacheDependency(SettingsData.Path);
                case CacheType.Pages:
                    return new CacheDependency(PageData.Path);
                case CacheType.Post:
                    return new CacheDependency(PostData.GetCachePath(id));
                case CacheType.Comments:
                    return new CacheDependency(CommentData.Path);
                default:
                    return null;
            }
        }

        #endregion
    }
}