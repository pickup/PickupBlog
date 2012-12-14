// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ICacheContext.cs" company="LiteBlog">
//   Copyright (c) 2012, LiteBlog. All Rights Reserved.
// </copyright>
// <summary>
//   The cache type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace LiteBlog.Common.Contracts
{
    using System;
    using System.Collections.Generic;
    using System.Web.Caching;

    /// <summary>
    /// The CacheContext interface.
    /// </summary>
    public interface ICacheContext
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
        CacheDependency GetDependency(CacheType type);

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
        CacheDependency GetDependency(CacheType type, string id);

        #endregion
    }
}