// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CacheType.cs" company="LiteBlog">
//   Copyright (c) 2012, LiteBlog. All Rights Reserved.
// </copyright>
// <summary>
//   The cache type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace LiteBlog.Common
{
    /// <summary>
    /// The cache type.
    /// </summary>
    public enum CacheType
    {
        /// <summary>
        /// The data context.
        /// </summary>
        DataContext, 

        /// <summary>
        /// The cache context.
        /// </summary>
        CacheContext, 

        /// <summary>
        /// The blog.
        /// </summary>
        Blog, 

        /// <summary>
        /// The categories.
        /// </summary>
        Categories, 

        /// <summary>
        /// The archive.
        /// </summary>
        Archive, 

        /// <summary>
        /// The post.
        /// </summary>
        Post, 

        /// <summary>
        /// The page
        /// </summary>
        Pages,

        /// <summary>
        /// The comments.
        /// </summary>
        Comments, 

        /// <summary>
        /// The rss.
        /// </summary>
        Rss, 

        /// <summary>
        /// The atom
        /// </summary>
        Atom,

        /// <summary>
        /// The settings.
        /// </summary>
        Settings
    }
}