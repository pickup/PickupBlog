// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CacheHelper.cs" company="LiteBlog">
//   Copyright (c) 2012, LiteBlog. All Rights Reserved.
// </copyright>
// <summary>
//   This class is in Common namespace because it
//   is tightly coupled with XmlLayer namespace
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace MvcLiteBlog.Helpers
{
    using System;
    using System.Web;
    using System.Web.Caching;

    using LiteBlog.Common;
    using LiteBlog.Common.Contracts;

    /// <summary>
    /// This class is in Common namespace because it 
    /// is tightly coupled with XmlLayer namespace
    /// </summary>
    public class CacheHelper
    {
        #region Public Methods and Operators

        /// <summary>
        /// The get.
        /// </summary>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <typeparam name="T">
        /// The T
        /// </typeparam>
        /// <returns>
        /// The T.
        /// </returns>
        public static T Get<T>(string key) where T : class
        {
            if (HttpContext.Current != null)
            {
                return (T)HttpContext.Current.Cache[key];
            }

            return null;
        }

        /// <summary>
        /// The get.
        /// </summary>
        /// <param name="type">
        /// The type.
        /// </param>
        /// <typeparam name="T">
        /// The T
        /// </typeparam>
        /// <returns>
        /// The T.
        /// </returns>
        public static T Get<T>(CacheType type) where T : class
        {
            if (HttpContext.Current != null)
            {
                return (T)HttpContext.Current.Cache[type.ToString()];
            }

            return null;
        }

        /// <summary>
        /// The put.
        /// </summary>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <param name="value">
        /// The value.
        /// </param>
        public static void Put(string key, object value)
        {
            if (HttpContext.Current != null)
            {
                HttpContext.Current.Cache[key] = value;
            }
        }

        /// <summary>
        /// The put.
        /// </summary>
        /// <param name="type">
        /// The type.
        /// </param>
        /// <param name="value">
        /// The value.
        /// </param>
        public static void Put(CacheType type, object value)
        {
            if (HttpContext.Current == null)
            {
                return;
            }

            ICacheContext context = ConfigHelper.CacheContext;
            if (context != null)
            {
                // Data layer may not support cache dependencies
                CacheDependency dep = context.GetDependency(type);
                if (dep != null)
                {
                    HttpContext.Current.Cache.Insert(type.ToString(), value, dep);
                }
            }
            else
            {
                HttpContext.Current.Cache.Insert(type.ToString(), value);
            }
        }

        /// <summary>
        /// The put post.
        /// </summary>
        /// <param name="fileID">
        /// The file id.
        /// </param>
        /// <param name="post">
        /// The post.
        /// </param>
        public static void PutPost(string fileID, Post post)
        {
            if (HttpContext.Current == null)
            {
                return;
            }

            ICacheContext context = ConfigHelper.CacheContext;
            if (context != null)
            {
                // Data layer may not support cache dependencies
                CacheDependency dep = context.GetDependency(CacheType.Post, fileID);
                if (dep != null)
                {
                    HttpContext.Current.Cache.Insert(
                        fileID, post, dep, Cache.NoAbsoluteExpiration, new TimeSpan(0, 5, 0));
                }
            }
            else
            {
                HttpContext.Current.Cache.Insert(fileID, post, null, Cache.NoAbsoluteExpiration, new TimeSpan(0, 5, 0));
            }
        }

        #endregion
    }
}